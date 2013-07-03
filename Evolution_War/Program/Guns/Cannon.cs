using System;
using System.Collections.Generic;

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
		protected List<Bullet> FireQueue;
		protected List<Int32> GunIndexQueue; 

		public Cannon(Ship pOwner)
			: base(pOwner)
		{
			FireQueue = new List<Bullet>(16);
			GunIndexQueue = new List<Int32>(16);
		}

		public override void ShootResiduals()
		{
			for (var i = FireQueue.Count / 5 + Math.Min(1, FireQueue.Count); i != 0; i--)
			{
				FireQueue[0].SetLaunchParameters(Owner.Position, Owner.Velocity,
					Owner.Angle + Math.Cos(16 * Owner.AngleVelocity * Constants.DegreesToRadians) * Constants.MultiFireAngles[Owner.UpgradeGroup.CannonMultiFire.Level][GunIndexQueue[0]],
					Constants.MultiFireOffsets[Owner.UpgradeGroup.CannonMultiFire.Level][GunIndexQueue[0]], // offset from starting position
					Speed);

				World.Instance.AddBullet(FireQueue[0]);
				FireQueue.RemoveAt(0);
				GunIndexQueue.RemoveAt(0);
			}
		}

		public override void TryShoot()
		{
			if (World.Instance.FrameCount < basicShotAvailableFrame) return;

			basicShotAvailableFrame = World.Instance.FrameCount + Delay;

			for (var i = 0; i < MultiGuns; i++)
			{
				var bullet = RecycleFactory.NewBullet(new DumbController(), this, Owner.UpgradeGroup.CannonPower.Level);
				var insertindex = Methods.Random.Next(FireQueue.Count);
				FireQueue.Insert(insertindex, bullet);
				GunIndexQueue.Insert(insertindex, i);
			}
		}
	}
}