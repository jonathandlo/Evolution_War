using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiom.Core;
using Axiom.Math;

namespace Evolution_War
{
	public class World // the world contains all of the objects and processes each one's drawing and physics
	{
		public Int64 FrameCount { get; private set; }
		public SceneManager SceneManager { get; private set; }

		public List<Ship> Ships;
		public List<Bullet> Bullets;
		public Arena Arena;
		public Ship PlayerShip;

		public World(SceneManager pSceneManager)
		{
			SceneManager = pSceneManager;
			Ships = new List<Ship>(48);
			Bullets = new List<Bullet>(256);
		}

		public void Loop()
		{
			FrameCount++;
			PlayerShip.Loop(this);

			for (var i = Ships.Count - 1; i >= 0; i--)
			{
				Ships[i].Loop(this);

				if (Ships[i].LoopResultStates.Remove)
				{
					Ships.RemoveAt(i);
				}
			}
			for (var i = Bullets.Count - 1; i >= 0; i--)
			{
				Bullets[i].Loop(this);

				if (Bullets[i].LoopResultStates.Remove)
				{
					RecycleFactory.Recycle(Bullets[i]);
					Bullets.RemoveAt(i);
				}
			}
		}

		public void Draw(Double pPercent)
		{
			PlayerShip.Draw(pPercent);

			for (var i = 0; i < Ships.Count; i++)
			{
				Ships[i].Draw(pPercent);
			}
			for (var i = 0; i < Bullets.Count; i++)
			{
				Bullets[i].Draw(pPercent);
			}
		}

		public void AddShip(Ship pShip)
		{
			Ships.Add(pShip);
		}

		public void AddBullet(Bullet pBullet)
		{
			Bullets.Add(pBullet);
		}
	}
}
