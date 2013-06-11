using System;

namespace Evolution_War
{
	public class Cannon : Gun
	{
		public Int32 Delay = 0;
		public Int32 Damage = 0;
		public Double Speed = 0;
		public Int32 MultiGuns = 0;
		public Int32 HomingAngle = 0;

		protected Int64 basicShotAvailableFrame = 0;

		public Cannon(Ship pOwner)
			: base(pOwner)
		{
		}

		public override void TryShoot(World pWorld)
		{
			if (pWorld.FrameCount < basicShotAvailableFrame) return;

			basicShotAvailableFrame = pWorld.FrameCount + Delay;

			for (var i = 0; i < MultiGuns; i++)
			{
				var bullet = RecycleFactory.NewBullet(pWorld.SceneManager, new FollowController(Owner), this);
				bullet.Launch(
					Owner.Position.x,
					Owner.Position.y,
					Owner.Velocity.x,
					Owner.Velocity.y,
					Owner.Angle + Math.Cos(8 * Owner.AngleVelocity * Constants.DegreesToRadians) * Constants.MultiFireAngles[Owner.Upgrades.CannonMultiFire.Level][i],
					Math.Cos(8 * Constants.DegreesToRadians) * Constants.MultiFireOffsets[Owner.Upgrades.CannonMultiFire.Level][i], // offset from starting position
					Speed);
				pWorld.AddBullet(bullet);
			}
		}
	}
}