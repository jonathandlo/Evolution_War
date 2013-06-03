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
		public UpgradeLevels UpgradeLevels { get; protected set; }
		protected Cannon cannon;

		public Ship(SceneManager pSceneManager, Controller pController, String pPrefix = "Ship ")
			: base(pController)
		{
			var name = pPrefix + Methods.GenerateUniqueID();

			Node = pSceneManager.RootSceneNode.CreateChildSceneNode(name);
			MeshNode = Node.CreateChildSceneNode();
			MeshNode.Orientation = new Quaternion(0.5, 0.5, -0.5, -0.5);
			MeshNode.AttachObject(pSceneManager.CreateEntity(name, "ship_assault_1.mesh"));

			UpgradeLevels = new UpgradeLevels()
							{
								LevelCannonAutoFire = 7,
								LevelCannonSpeed = 5,
								LevelCannonMultiFire = 4
							};
			cannon = new Cannon(this);
			UpgradeLevels.UpgradeCannon(ref cannon, UpgradeLevels);
		}

		protected override void LoopControlPhysics(World pWorld)
		{
			base.LoopControlPhysics(pWorld);

			if (controller.InputStates.C)
			{
				cannon.TryShoot(pWorld);
			}
		}
	}
}
