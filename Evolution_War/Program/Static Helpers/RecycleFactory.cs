using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiom.Core;

namespace Evolution_War
{
	public static class RecycleFactory
	{
		private static Queue<Bullet> freeBullets = new Queue<Bullet>(256);
		private static Queue<Trail> freeTrails = new Queue<Trail>(256);

		public static Bullet NewBullet(Controller pController, Gun pOwnerGun, Int32 pColorIndex)
		{
			if (freeBullets.Count == 0)
			{
				return new Bullet(pController, pOwnerGun, pColorIndex);
			}

			var bullet = freeBullets.Dequeue();
			bullet.Reinitialize(pController, pOwnerGun, pColorIndex);
			return bullet;
		}

		public static Trail NewTrail(MovingObject pObjectToFollow, Single pMaxWidth, ColorEx pColor)
		{
			if (freeTrails.Count == 0)
			{
				return new Trail(pObjectToFollow, pMaxWidth, pColor);
			}

			var trail = freeTrails.Dequeue();
			trail.Relaunch(pObjectToFollow, pMaxWidth, pColor);
			return trail;
		}

		public static void Recycle(Bullet pBullet)
		{
			pBullet.Recycle();
			freeBullets.Enqueue(pBullet);
		}

		public static void Recycle(Trail pTrail)
		{
			pTrail.Recycle();
			freeTrails.Enqueue(pTrail);
		}
	}
}
