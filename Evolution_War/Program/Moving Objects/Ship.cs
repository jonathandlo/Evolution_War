using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Axiom.Core;
using Axiom.Graphics;
using Axiom.Math;

namespace Evolution_War
{
	public class Ship : MovingObject
	{
		public Upgrades Upgrades { get; protected set; }
		public Int32 AvailableUpgradePoints = 0;
		protected Cannon cannon;

		public Ship(SceneManager pSceneManager, Controller pController, String pPrefix = "Ship ")
			: base(pController)
		{
			var name = pPrefix + Methods.GenerateUniqueID();

			Node = pSceneManager.RootSceneNode.CreateChildSceneNode(name);
			MeshNode = Node.CreateChildSceneNode();
			MeshNode.Orientation = new Quaternion(0.5, 0.5, -0.5, -0.5);
			MeshNode.AttachObject(pSceneManager.CreateEntity(name, "ship_assault_1.mesh"));

			Upgrades = new Upgrades();
			Upgrades.CannonAutoFire.Level = 5;
			Upgrades.CannonMultiFire.Level = 2;
			Upgrades.CannonSpeed.Level = 4;

			cannon = new Cannon(this);
			Upgrades.UpgradeCannon(ref cannon, Upgrades);
		}

		protected override void LoopControlPhysics(World pWorld)
		{
			base.LoopControlPhysics(pWorld);

			if (controller.InputStates.Fire) cannon.TryShoot(pWorld);
			if (controller.InputStates.DeltaSecondary) pWorld.HUD.AddPoints();
			if (controller.InputStates.DeltaUpgrade) pWorld.HUD.PressUpgrade();
		}
	}
}
