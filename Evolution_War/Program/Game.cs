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
		protected Root Engine;
		protected Config.IConfigurationManager ConfigurationManager;
		protected ResourceGroupManager Content;
		protected SceneManager SceneManager;
		protected Camera Camera;
		protected Viewport Viewport;
		protected RenderWindow Window;
		protected RenderSystem RenderSystem;

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
			Camera.Position = new Vector3(0, 0, 16);
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
			light.Position = new Vector3(0, 0, 15);
			light.Direction = new Vector3(0, 0, -1);
			light.SetSpotlightRange(30.0f, 90.0f);

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

		private void Engine_FrameRenderingQueued(object source, FrameEventArgs e)
		{
			var dist = e.TimeSinceLastFrame * 10;
			var x = (Input.getKey(Keys.Left) ? -1 : 0) + (Input.getKey(Keys.Right) ? 1 : 0);
			int y = (Input.getKey(Keys.Down) ? -1 : 0) + (Input.getKey(Keys.Up) ? 1 : 0);

			SceneManager.GetSceneNode("ship").Translate(new Vector3(x * dist, y * dist, 0));
			
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