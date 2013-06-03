using System;
using Axiom.Core;
using Axiom.Math;

namespace Evolution_War
{
	public class Bullet : MovingObject
	{
		public Gun OwnerGun;
		protected const Int32 expirationFrames = 16;
		protected Int32 framesAlive = 0;

		public Bullet(SceneManager pSceneManager, Controller pController, Gun pOwnerGun)
			: base(pController)
		{
			var name = "Bullet " + Methods.GenerateUniqueID();

			Node = pSceneManager.RootSceneNode.CreateChildSceneNode(name);
			MeshNode = Node.CreateChildSceneNode();
			MeshNode.Scale = new Vector3(0.3, 0.3, 0.3);
			MeshNode.AttachObject(pSceneManager.CreateEntity(name, "bullet.mesh"));

			OwnerGun = pOwnerGun;
		}

		public void Reinitialize(Controller pController, Gun pOwnerGun) // used by Factory to recycle Bullet objects
		{
			controller = pController;
			OwnerGun = pOwnerGun;
			Node.AddChild(MeshNode);
		}

		public void Recycle()
		{
			Node.RemoveAllChildren();
		}

		public void Launch(Double pX, Double pY, Double pDx, Double pDy, Double pAngleDegrees, Double pAngledOffset, Double pSpeed)
		{
			var offsetAngle = (pAngleDegrees + pAngledOffset) * Constants.DegreesToRadians;
			var degreeAngle = pAngleDegrees * Constants.DegreesToRadians;

			framesAlive = 0;
			x = ox = pX + 5 * Math.Cos(offsetAngle);
			y = oy = pY + 5 * Math.Sin(offsetAngle);
			a = oa = pAngleDegrees;
			dx = pDx + pSpeed * Math.Cos(degreeAngle);
			dy = pDy + pSpeed * Math.Sin(degreeAngle);
		}

		protected override void LoopControlPhysics(World pWorld)
		{
			if (++framesAlive > expirationFrames)
			{
				LoopResultStates.Remove = true;
			}
		}

		protected override void LoopFrictionPhysics()
		{
			// no friction
		}

		protected override void LoopCollisionPhysics(World pWorld)
		{
			if (x + dx > pWorld.Arena.Right ||
				x + dx < pWorld.Arena.Left ||
				y + dy > pWorld.Arena.Bottom ||
				y + dy < pWorld.Arena.Top)
				LoopResultStates.Remove = true;
		}
	}
}