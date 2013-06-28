using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Axiom.Collections;
using Axiom.Core;
using Axiom.Fonts;
using Axiom.Media;
using Config = Axiom.Framework.Configuration;
using Axiom.Graphics;
using Axiom.Math;

namespace Evolution_War
{
	public class Game : IDisposable, IWindowEventListener
	{
		// Axiom Setup.
		private Root root;
		private Config.IConfigurationManager configManager;
		private SceneManager sceneManager;
		private Viewport viewport;
		private RenderWindow window;
		private RenderSystem renderSystem;

		// Timer Setup.
		private Stopwatch stopwatch;
		private Double lastPhysicsStepTicks;
		private Double physicsDelayTicks;
		private Double ticksToMilliFactor;
		private Double ticksToPhysicsStepPercentFactor;

		// Lights, Camera.
		private SmoothCamera camera;
		private Light sunlight;

		public void InitializeSystem()
		{
			configManager = new Config.DefaultConfigurationManager();
			root = new Root(configManager.LogFilename);
			root.FrameRenderingQueued += RootFrameRenderingQueued;

			// Load Config.
			configManager.RestoreConfiguration(root);

			// Render System.
			if (root.RenderSystem == null)
				renderSystem = root.RenderSystem = root.RenderSystems.First().Value;
			else
				renderSystem = root.RenderSystem;

			// Render Window.
			Root.Instance.Initialize(false);
			var parameterList = new NamedParameterList
			{
				{"vsync", "true"},
				{"Anti aliasing", "Level 2"},
				{"FSAA", 1},
				{"colorDepth", 32},
				{"border", "fixed"}
			};
			window = Root.Instance.CreateRenderWindow("EvolutionWarWindow", Constants.Width, Constants.Height, false, parameterList);
			WindowEventMonitor.Instance.RegisterListener(window, this);

			// Content.
			ResourceGroupManager.Instance.AddResourceLocation("Meshes", "Folder", true);
			ResourceGroupManager.Instance.AddResourceLocation("Fonts", "Folder", true);
			ResourceGroupManager.Instance.InitializeAllResourceGroups();

			// Fonts.
			var font = FontManager.Instance.Create("Candara", ResourceGroupManager.DefaultResourceGroupName) as Font;
			if (font != null)
			{
				font.Type = FontType.TrueType;
				font.Source = "Candarab.ttf";
				font.TrueTypeSize = 36;
				font.TrueTypeResolution = 96;
				font.AntialiasColor = false;
				font.Load();
			}

			// Scene Manager.
			sceneManager = root.CreateSceneManager("DefaultSceneManager", "GameSMInstance");
			sceneManager.ClearScene();
		}

		public void InitializeScene()
		{
			// Start the Stopwatch.
			stopwatch = new Stopwatch();
			stopwatch.Start();
			physicsDelayTicks = Stopwatch.Frequency / 30.0; // 30 physics steps per second
			ticksToMilliFactor = 1.0 / (Stopwatch.Frequency / 1000.0);
			ticksToPhysicsStepPercentFactor = 1.0 / physicsDelayTicks;
			lastPhysicsStepTicks = 0.0;

			// Create the World.
			World.Instance = new World(sceneManager);
			World.Instance.Initialize();
			World.Instance.Arena = new Arena(sceneManager, 500, 4);

			// Create a AI Ships.
			for (var i = 0; i < 36; i++)
			{
				World.Instance.AddShip(new Ship(sceneManager, new RandomController()));
			}

			// Camera.
			camera = new SmoothCamera("Camera", sceneManager, World.Instance.PlayerShip, 6);

			// Lighting.
			sunlight = sceneManager.CreateLight("Sunlight");
			sunlight.Type = LightType.Spotlight;
			sunlight.Specular = ColorEx.White;
			sunlight.Diffuse = new ColorEx(0.85f, 0.77f, 0.60f);
			sunlight.Position = new Vector3(0, 0, 0);
			sunlight.Direction = Vector3.NegativeUnitZ;
			sunlight.SetSpotlightRange(15, 60);
			sceneManager.AmbientLight = ColorEx.Black;
			camera.Node.AttachObject(sunlight);

			// Viewport.
			viewport = window.AddViewport(camera, 0, 0, 1.0f, 1.0f, 100);
			viewport.BackgroundColor = ColorEx.Black;
		}

		public void Run()
		{
			InitializeSystem();
			InitializeScene();
			root.StartRendering();
		}

		private void RootFrameRenderingQueued(object source, FrameEventArgs e) // Gameloop, called during every render. CPU is free during render.
		{
			var ticksAhead = stopwatch.ElapsedTicks - lastPhysicsStepTicks; // ticksAhead goes from 0 to PhysicsDelayTicks.

			while (ticksAhead >= physicsDelayTicks)
			{
				lastPhysicsStepTicks += physicsDelayTicks;
				ticksAhead -= physicsDelayTicks;

				World.Instance.Loop();
				camera.Loop();
			}

			var percent = ticksAhead * ticksToPhysicsStepPercentFactor;

			// Continue to draw as fast as possible.
			World.Instance.Draw(percent);
			camera.Draw(percent);
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
					if (root != null)
						root.FrameStarted -= RootFrameRenderingQueued;
					if (sceneManager != null)
						sceneManager.RemoveAllCameras();
					camera = null;
					if (Root.Instance != null)
						Root.Instance.RenderSystem.DetachRenderTarget(window);
					if (window != null)
					{
						WindowEventMonitor.Instance.UnregisterWindow(window);
						window.Dispose();
					}
					if (root != null)
						root.Dispose();
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
			if (rw == window)
			{
				Root.Instance.QueueEndRendering();
			}
		}
		public void WindowFocusChange(RenderWindow rw) { }

		#endregion
	}
}