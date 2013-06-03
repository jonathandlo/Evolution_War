using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evolution_War
{
	public abstract class Gun
	{
		public Ship Owner { get; private set; }

		public Gun(Ship pOwner)
		{
			Owner = pOwner;
		}

		public abstract void TryShoot(World pWorld);
	}
}
