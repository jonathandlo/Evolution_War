using System;
using System.Linq;
using Axiom.Core;
using Axiom.Framework.Configuration;
using Axiom.Graphics;
using Vector3 = Axiom.Math.Vector3;

namespace Evolution_War
{
	public class Game : IDisposable, IWindowEventListener
	{
		protected Root Engine;
		protected IConfigurationManager ConfigurationManager;
		protected ResourceGroupManager Content;
		protected SceneManager SceneManager;
		protected Camera Camera;
		protected Viewport Viewport;
		protected RenderWindow Window;
		protected Axiom.Graphics.RenderSystem RenderSystem;
		protected SharpInputSystem.InputManager InputManager;
		protected SharpInputSystem.Mouse mouse;
		protected SharpInputSystem.Keyboard keyboard;

		public void Run()
		{
			InitializeSystem();
			InitializeScene();
			//InitializeInput();

			CreateScene();
			Engine.StartRendering();
		}

		private void Engine_FrameRenderingQueued(object source, FrameEventArgs e)
		{
			Update(e.TimeSinceLastFrame);
		}

		public void InitializeSystem()
		{
			ConfigurationManager = new DefaultConfigurationManager();
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
			ResourceGroupManager.Instance.InitializeAllResourceGroups();

			// Scene Manager
			SceneManager = Engine.CreateSceneManager("DefaultSceneManager", "GameSMInstance");
			SceneManager.ClearScene();
		}

		public void InitializeScene()
		{
			// Camera
			Camera = SceneManager.CreateCamera("MainCamera");
			Camera.Position = new Vector3(0, 0, 500);
			Camera.LookAt(new Vector3(0, 0, -300));
			Camera.Near = 5;
			Camera.AutoAspectRatio = true;

			// Viewport
			Viewport = Window.AddViewport(Camera, 0, 0, 1.0f, 1.0f, 100);
			Viewport.BackgroundColor = ColorEx.SteelBlue;
		}

		public void InitializeInput()
		{
			SharpInputSystem.ParameterList pl = new SharpInputSystem.ParameterList();
			pl.Add(new SharpInputSystem.Parameter("WINDOW", Window["WINDOW"]));

			if (RenderSystem.Name.Contains("DirectX"))
			{
				//Default mode is foreground exclusive..but, we want to show mouse - so nonexclusive
				pl.Add(new SharpInputSystem.Parameter("w32_mouse", "CLF_BACKGROUND"));
				pl.Add(new SharpInputSystem.Parameter("w32_mouse", "CLF_NONEXCLUSIVE"));
			}

			//This never returns null.. it will raise an exception on errors
			InputManager = SharpInputSystem.InputManager.CreateInputSystem(pl);
			//mouse = InputManager.CreateInputObject<SharpInputSystem.Mouse>( true, "" );
			//keyboard = InputManager.CreateInputObject<SharpInputSystem.Keyboard>( true, "" );
		}

		public void CreateScene()
		{
			
		}

		public void Update(float timeSinceLastFrame)
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

		public void WindowMoved(RenderWindow rw) {}
		public void WindowResized(RenderWindow rw) {}
		public void WindowClosed(RenderWindow rw)
		{
			// Only do this for the Main Window
			if (rw == Window)
			{
				Root.Instance.QueueEndRendering();
			}
		}
		public void WindowFocusChange(RenderWindow rw) {}

		#endregion
	}
}