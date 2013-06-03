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

		public static Bullet NewBullet(SceneManager pSceneManager, Controller pController, Gun pOwnerGun)
		{
			if (freeBullets.Count == 0)
			{
				return new Bullet(pSceneManager, pController, pOwnerGun);
			}

			var bullet = freeBullets.Dequeue();
			bullet.Reinitialize(pController, pOwnerGun);
			return bullet;
		}

		public static void Recycle(Bullet pBullet)
		{
			pBullet.Recycle();
			freeBullets.Enqueue(pBullet);
		}
	}
}
