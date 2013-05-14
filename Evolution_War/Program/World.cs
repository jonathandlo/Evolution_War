using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_War
{
	public class World
	{
		public Ship PlayerShip, AIShip;

		public void Loop()
		{
			PlayerShip.Loop(this);
			AIShip.Loop(this);
		}

		public void Draw(Double pPercent)
		{
			PlayerShip.Draw(pPercent);
			AIShip.Draw(pPercent);
		}
	}
}
