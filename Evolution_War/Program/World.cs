using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiom.Math;

namespace Evolution_War
{
	public class World // the world contains all of the objects and processes each one's drawing and physics
	{
		public Arena Arena;
		public Ship PlayerShip;
		public List<Ship> Ships;
		public List<Bullet> Bullets;

		public World()
		{
			Ships = new List<Ship>();
		}

		public void Loop(World pWorld)
		{
			PlayerShip.Loop(this);

			foreach (var ship in Ships)
			{
				ship.Loop(pWorld);
			}
		}

		public void Draw(Double pPercent)
		{
			PlayerShip.Draw(pPercent);

			foreach (var ship in Ships)
			{
				ship.Draw(pPercent);
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
