using System;
using Axiom.Animating;
using Axiom.Core;
using Axiom.Graphics;
using Axiom.Math;

namespace Evolution_War
{
	public class Bullet : MovingObject
	{
		// Link to other objects.
		public Gun OwnerGun;
		protected Entity bulletMesh;
		protected Trail trail;

		// Visuals.
		protected Int32 expirationFrames = 20;
		protected Int32 colorIndex;

		// Expiration.
		protected Int32 framesAlive = 0;

		public Bullet(Controller pController, Gun pOwnerGun, Int32 pColorIndex)
			: base(pController)
		{
			var bulletname = Methods.GenerateUniqueID.ToString();

			Node = World.Instance.SceneManager.RootSceneNode.CreateChildSceneNode(bulletname);
			Node.IsVisible = false;
			MeshNode = Node.CreateChildSceneNode();
			MeshNode.Scale = new Vector3(0.3, 0.3, 0.3);
			bulletMesh = World.Instance.SceneManager.CreateEntity(bulletname, "bullet.mesh");
			MeshNode.AttachObject(bulletMesh);

			Reinitialize(pController, pOwnerGun, pColorIndex);
		}

		public void Reinitialize(Controller pController, Gun pOwnerGun, Int32 pColorIndex) // used by Factory to recycle Bullet objects.
		{
			controller = pController;
			OwnerGun = pOwnerGun;
			colorIndex = pColorIndex;
			framesAlive = 0;
			bulletMesh.MaterialName = Constants.DamageMaterialNames[pColorIndex];
		}

		public void CreateTrail()
		{
			// bullet trail.
			trail = RecycleFactory.NewTrail(this, 0.8f, Constants.DamageColors[colorIndex]);
			World.Instance.AddTrail(trail);
		}

		public void Recycle()
		{
			trail.ObjectToFollowDisappeared();
			Node.IsVisible = false;
		}

		public void SetLaunchParameters(Vector3 pPosition, Vector3 pVelocity, Double pAngleDegrees, Double pAngledOffset, Double pSpeed)
		{
			pAngleDegrees += Methods.Random.NextDouble() * 4 - 2;
			pSpeed *= Methods.Random.NextDouble() * 0.4 + 0.7;

			var offsetAngle = (pAngleDegrees + pAngledOffset) * Constants.DegreesToRadians;
			var degreeAngle = pAngleDegrees * Constants.DegreesToRadians;

			x = ox = pPosition.x + Methods.Random.Next(4, 7) * Math.Cos(offsetAngle);
			y = oy = pPosition.y + Methods.Random.Next(4, 7) * Math.Sin(offsetAngle);
			a = oa = pAngleDegrees;
			dx = pVelocity.x + pSpeed * Math.Cos(degreeAngle);
			dy = pVelocity.y + pSpeed * Math.Sin(degreeAngle);
		}

		protected override void LoopControlPhysics()
		{
			base.LoopControlPhysics();

			if (++framesAlive > expirationFrames)
			{
				LoopResultStates.Remove = true;
			}
		}

		protected override void LoopFrictionPhysics()
		{
			// no friction.
		}

		protected override void LoopCollisionPhysics()
		{
			if (x + dx > World.Instance.Arena.Right ||
				x + dx < World.Instance.Arena.Left ||
				y + dy > World.Instance.Arena.Bottom ||
				y + dy < World.Instance.Arena.Top)
				LoopResultStates.Remove = true;
		}
	}
}