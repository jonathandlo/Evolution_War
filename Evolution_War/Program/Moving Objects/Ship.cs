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
		public UpgradeGroup UpgradeGroup { get; protected set; }
		public Int32 AvailableUpgradePoints = 0;
		protected Cannon cannon;

		public Ship(SceneManager pSceneManager, Controller pController)
			: base(pController)
		{
			var name = Methods.GenerateUniqueID.ToString();

			Node = pSceneManager.RootSceneNode.CreateChildSceneNode(name);
			MeshNode = Node.CreateChildSceneNode();
			MeshNode.Orientation = new Quaternion(0.5, 0.5, -0.5, -0.5);
			MeshNode.AttachObject(pSceneManager.CreateEntity(name, "ship_assault_1.mesh"));

			UpgradeGroup = new UpgradeGroup
			{
				CannonAutoFire = { Level = 10 },
				CannonMultiFire = { Level = 10 },
				CannonSpeed = { Level = 5 }
			};

			cannon = new Cannon(this);
			UpgradeGroup.UpgradeCannon(ref cannon, UpgradeGroup);
		}

		protected override void LoopControlPhysics()
		{
			base.LoopControlPhysics();

			if (controller.InputStates.Fire) cannon.TryShoot();
			if (controller.InputStates.DeltaSecondary) World.Instance.HUD.AddPoints();
			if (controller.InputStates.DeltaUpgrade) World.Instance.HUD.PressUpgrade();

			cannon.ShootResiduals();
		}
	}
}
