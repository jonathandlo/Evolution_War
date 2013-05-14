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

		// Game World
		protected World World;

		public void InitializeSystem()
		{
			ConfigurationManager = new Config.DefaultConfigurationManager();
			Engine = new Root(ConfigurationManager.LogFilename);
			Engine.FrameRenderingQueued += Engine_FrameRenderingQueued;

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
			Camera.Position = new Vector3(0, -4, 256);
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

			// Lighting
			var light = SceneManager.CreateLight("spotLight");
			light.Type = LightType.Spotlight;
			light.Diffuse = ColorEx.White;
			light.Specular = ColorEx.Yellow;
			light.Position = new Vector3(0, 0, 256);
			light.Direction = new Vector3(0, 0, -1);
			light.SetSpotlightRange(5.0f, 90.0f, 8.0f);

			var sunLight = SceneManager.CreateLight("sunLight");
			sunLight.Type = LightType.Directional;
			sunLight.Diffuse = new ColorEx(0.13f, 0.1f, 0.0f);
			sunLight.Direction = new Vector3(1, -1, -4);

			// Start the Stopwatch
			Stopwatch = new Stopwatch();
			Stopwatch.Start();
			PhysicsDelayTicks = Stopwatch.Frequency / 30.0; // 30 physics steps per second
			TicksToMilliFactor = 1.0 / (Stopwatch.Frequency / 1000.0);
			TicksToPhysicsStepPercentFactor = 1.0 / PhysicsDelayTicks;
			LastPhysicsStepTicks = 0.0;

			// Create the World
			World = new World();

			// Create Player Ship
			var ent = SceneManager.CreateEntity("ship", "ship_assault_1.mesh");
			var node = SceneManager.RootSceneNode.CreateChildSceneNode("ship");
			node.Position = new Vector3(0, 0, 0);
			node.Rotate(new Vector3(1, 0, 0), 90.0f);
			node.AttachObject(ent);
			World.PlayerShip = new Ship(node, new PlayerController());

			// Create an AI Ship
			var ent2 = SceneManager.CreateEntity("ship2", "ship_assault_1.mesh");
			var node2 = SceneManager.RootSceneNode.CreateChildSceneNode("ship2");
			node2.Position = new Vector3(0, -16, 0);
			node2.Rotate(new Vector3(1, 0, 0), 90.0f);
			node2.AttachObject(ent2);
			World.AIShip = new Ship(node2, new FollowController());
		}

		public void Run()
		{
			InitializeSystem();
			InitializeScene();

			CreateScene();
			Engine.StartRendering();
		}

		private void Engine_FrameRenderingQueued(object source, FrameEventArgs e) // Gameloop, called during every render. CPU is free during render.
		{
			var ticksAhead = Stopwatch.ElapsedTicks - LastPhysicsStepTicks; // ticksAhead goes from 0 to PhysicsDelayTicks

			if (ticksAhead < PhysicsDelayTicks) // continue to draw as fast as possible
			{
				var percent = ticksAhead * TicksToPhysicsStepPercentFactor;
				World.Draw(percent);
			}
			else // compute the next physics step
			{
				LastPhysicsStepTicks += PhysicsDelayTicks;
				World.Loop();
			}

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