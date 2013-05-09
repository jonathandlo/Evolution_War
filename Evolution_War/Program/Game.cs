using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Axiom.Core;
using Config = Axiom.Framework.Configuration;
using Axiom.Graphics;
using Axiom.Math;

namespace Evolution_War
{
	public class Game : IDisposable, IWindowEventListener
	{
		// Axiom Setup
		protected Root Engine;
		protected Config.IConfigurationManager ConfigurationManager;
		protected ResourceGroupManager Content;
		protected SceneManager SceneManager;
		protected Camera Camera;
		protected Viewport Viewport;
		protected RenderWindow Window;
		protected RenderSystem RenderSystem;

		// Timer Setup
		protected Stopwatch Stopwatch;
		protected Double LastPhysicsStepTicks;
		protected Double PhysicsDelayTicks;
		protected Double TicksToMilliFactor;
		protected Double TicksToPhysicsStepPercentFactor;

		public void InitializeSystem()
		{
			ConfigurationManager = new Config.DefaultConfigurationManager();
			Engine = new Root(ConfigurationManager.LogFilename);
			Engine.FrameStarted += EngineOnFrameStarted;
			Engine.FrameRenderingQueued += Engine_FrameRenderingQueued;
			Engine.FrameEnded += EngineOnFrameEnded;

			// Load Config
			ConfigurationManager.RestoreConfiguration(Engine);

			// Render System
			if (Engine.RenderSystem == null)
				RenderSystem = Engine.RenderSystem = Engine.RenderSystems.First().Value;
			else
				RenderSystem = Engine.RenderSystem;

			// Render Window
			Window = Root.Instance.Initialize(true, "Axiom Framework Window");
			WindowEventMonitor.Instance.RegisterListener(Window, this);

			// Content
			ResourceGroupManager.Instance.AddResourceLocation("Meshes", "Folder", true);
			ResourceGroupManager.Instance.InitializeAllResourceGroups();

			// Scene Manager
			SceneManager = Engine.CreateSceneManager("DefaultSceneManager", "GameSMInstance");
			SceneManager.ClearScene();
		}

		public void InitializeScene()
		{
			// Camera
			Camera = SceneManager.CreateCamera("MainCamera");
			Camera.Position = new Vector3(0, -4, 128);
			Camera.LookAt(new Vector3(0, 0, 0));
			Camera.Near = 5;
			Camera.AutoAspectRatio = true;

			// Viewport
			Viewport = Window.AddViewport(Camera, 0, 0, 1.0f, 1.0f, 100);
			Viewport.BackgroundColor = ColorEx.Black;
		}

		public void CreateScene()
		{
			SceneManager.AmbientLight = ColorEx.Black;
			SceneManager.DefaultMaterialSettings.ShadingMode = Shading.Gouraud;

			// Spot Lighting
			var light = SceneManager.CreateLight("spotLight");
			light.Type = LightType.Spotlight;
			light.Diffuse = ColorEx.White;
			light.Specular = ColorEx.Yellow;
			light.Position = new Vector3(0, 0, 128);
			light.Direction = new Vector3(0, 0, -1);
			light.SetSpotlightRange(5.0f, 90.0f, 8.0f);

			// Start the Stopwatch
			Stopwatch = new Stopwatch();
			Stopwatch.Start();
			PhysicsDelayTicks = Stopwatch.Frequency / 30.0; // 30 physics steps per second
			TicksToMilliFactor = 1.0 / (Stopwatch.Frequency / 1000.0);
			TicksToPhysicsStepPercentFactor = 1.0 / PhysicsDelayTicks;
			LastPhysicsStepTicks = 0.0;

			// Create Player Ship
			var ent = SceneManager.CreateEntity("ship", "ship_assault_1.mesh");
			var node = SceneManager.RootSceneNode.CreateChildSceneNode("ship");
			node.Position = new Vector3(0, 0, 0);
			node.Rotate(new Vector3(1, 0, 0), 90.0f);
			ent.DisplaySkeleton = true;
			node.AttachObject(ent);
		}

		public void Run()
		{
			InitializeSystem();
			InitializeScene();

			CreateScene();
			Engine.StartRendering();
		}

		private void EngineOnFrameStarted(object sender, FrameEventArgs frameEventArgs)
		{

		}

		private double x, y, ox, oy; // positions
		private double dx, dy, odx, ody; // speeds
		private double a, oa; // angles (degrees)
		private double da, oda; // angular speeds

		private void Engine_FrameRenderingQueued(object source, FrameEventArgs e) // Gameloop, called during every render. CPU is free during render.
		{
			var ticksAhead = Stopwatch.ElapsedTicks - LastPhysicsStepTicks; // tickAhead goes from 0 to PhysicsDelayTicks

			if (ticksAhead < PhysicsDelayTicks) // continue to draw until the next physics step
			{
				var percent = ticksAhead * TicksToPhysicsStepPercentFactor;
				var ship = SceneManager.GetSceneNode("ship");

				ship.Position = new Vector3(Methods.CubicStep(ox, odx, x, dx, percent), Methods.CubicStep(oy, ody, y, dy, percent), 0);
				ship.Orientation = Quaternion.FromEulerAnglesInDegrees(90.0, 0.0, 0.0);
				ship.Rotate(Quaternion.FromEulerAnglesInDegrees(0.0, - 16 * Methods.LinearStep(oda, da, percent), 0.0), TransformSpace.World);
				ship.Rotate(Quaternion.FromEulerAnglesInDegrees(0.0, 0.0, Methods.CubicStep(oa, oda, a, da, percent) - 90.0f), TransformSpace.World);

			}
			else // compute the next physics step
			{
				LastPhysicsStepTicks += PhysicsDelayTicks;

				// position and angle memory
				ox = x;
				oy = y;
				odx = dx;
				ody = dy;
				oa = a;
				oda = da;

				// thrust
				dx += 0.1 * Math.Cos(a * Methods.DegreesToRadians) * (Input.getKey(Keys.Up) ? 1 : 0);
				dy += 0.1 * Math.Sin(a * Methods.DegreesToRadians) * (Input.getKey(Keys.Up) ? 1 : 0);
				da += 0.8 * ((Input.getKey(Keys.Right) ? -1 : 0) + (Input.getKey(Keys.Left) ? 1 : 0));

				// dynamic friction
				dx *= (1 - 0.04) - (Input.getKey(Keys.Down) ? 0.1 : 0);
				dy *= (1 - 0.04) - (Input.getKey(Keys.Down) ? 0.1 : 0);
				da *= (1 - 0.16) - (Input.getKey(Keys.Down) ? 0.1 : 0);

				// static friction
				dx -= dx > 0 ? Math.Min(0.01, Math.Abs(dx)) * Math.Sign(dx) : 0;
				dy -= dy > 0 ? Math.Min(0.01, Math.Abs(dy)) * Math.Sign(dy) : 0;
				da -= da > 0 ? Math.Min(0.01, Math.Abs(da)) * Math.Sign(da) : 0;

				// advance position
				x += dx;
				y += dy;
				oa = a % 360 + oa - a;
				a = a % 360 + da;

				Debug.WriteLine(oa.ToString("F3") + "+" + oda.ToString("F3") + "      " + a.ToString("F3") + "+" + da.ToString("F3"));
			}

		}

		private void EngineOnFrameEnded(object sender, FrameEventArgs frameEventArgs)
		{

		}















		#region IDisposable Implementation

		private bool _disposed = false;

		public bool IsDisposed
		{
			get { return _disposed; }
			set { _disposed = value; }
		}

		protected void dispose(bool disposeManagedResources)
		{
			if (!IsDisposed)
			{
				if (disposeManagedResources)
				{
					if (Engine != null)
						Engine.FrameStarted -= Engine_FrameRenderingQueued;
					if (SceneManager != null)
						SceneManager.RemoveAllCameras();
					Camera = null;
					if (Root.Instance != null)
						Root.Instance.RenderSystem.DetachRenderTarget(Window);
					if (Window != null)
					{
						WindowEventMonitor.Instance.UnregisterWindow(Window);
						Window.Dispose();
					}
					if (Engine != null)
						Engine.Dispose();
				}

				// There are no unmanaged resources to release, but
				// if we add them, they need to be released here.
			}
			IsDisposed = true;
		}

		public void Dispose()
		{
			dispose(true);
			GC.SuppressFinalize(this);
		}

		~Game()
		{
			dispose(false);
		}

		#endregion IDisposable Implementation

		#region IWindowEventListener Implementation

		public void WindowMoved(RenderWindow rw) { }
		public void WindowResized(RenderWindow rw) { }
		public void WindowClosed(RenderWindow rw)
		{
			// Only do this for the Main Window
			if (rw == Window)
			{
				Root.Instance.QueueEndRendering();
			}
		}
		public void WindowFocusChange(RenderWindow rw) { }

		#endregion
	}
}