using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_War
{
	public class UpgradeLevels
	{
		public Int32 LevelCannonPower = 0;
		public Int32 LevelCannonSpeed = 0;
		public Int32 LevelCannonAutoFire = 0;
		public Int32 LevelCannonHoming = 0;
		public Int32 LevelCannonMultiFire = 0;

		public Int32 LevelShipThrust = 0;
		public Int32 LevelShipTurn = 0;
		public Int32 LevelShipArmor = 0;
		public Int32 LevelShipShield = 0;
		public Int32 LevelShipShieldRegen = 0;


		public static void UpgradeCannon(ref Cannon pCannon, UpgradeLevels pLevels)
		{
			switch (pLevels.LevelCannonPower)
			{
				case 0: pCannon.Damage = 3; break;

				case 1: pCannon.Damage = 4; break;
				case 2: pCannon.Damage = 6; break;
				case 3: pCannon.Damage = 8; break;
				case 4: pCannon.Damage = 11; break;
				case 5: pCannon.Damage = 14; break;

				case 6: pCannon.Damage = 14; break;
				case 7: pCannon.Damage = 14; break;
				case 8: pCannon.Damage = 14; break;
				case 9: pCannon.Damage = 14; break;
				case 10: pCannon.Damage = 14; break;
			}
			switch (pLevels.LevelCannonSpeed)
			{
				case 0: pCannon.Speed = 3; break;

				case 1: pCannon.Speed = 4; break;
				case 2: pCannon.Speed = 5; break;
				case 3: pCannon.Speed = 6; break;
				case 4: pCannon.Speed = 7; break;
				case 5: pCannon.Speed = 8; break;

				case 6: pCannon.Speed = 8; break;
				case 7: pCannon.Speed = 8; break;
				case 8: pCannon.Speed = 8; break;
				case 9: pCannon.Speed = 8; break;
				case 10: pCannon.Speed = 8; break;
			}
			switch (pLevels.LevelCannonAutoFire)
			{
				case 0: pCannon.Delay = 14; break;

				case 1: pCannon.Delay = 11; break;
				case 2: pCannon.Delay = 8; break;
				case 3: pCannon.Delay = 6; break;
				case 4: pCannon.Delay = 4; break;
				case 5: pCannon.Delay = 3; break;

				case 6: pCannon.Delay = 3; break;
				case 7: pCannon.Delay = 3; break;
				case 8: pCannon.Delay = 3; break;
				case 9: pCannon.Delay = 3; break;
				case 10: pCannon.Delay = 3; break;
			}
			switch (pLevels.LevelCannonHoming)
			{
				case 0: pCannon.HomingAngle = 4; break;

				case 1: pCannon.HomingAngle = 8; break;
				case 2: pCannon.HomingAngle = 12; break;
				case 3: pCannon.HomingAngle = 16; break;
				case 4: pCannon.HomingAngle = 20; break;
				case 5: pCannon.HomingAngle = 24; break;

				case 6: pCannon.HomingAngle = 24; break;
				case 7: pCannon.HomingAngle = 24; break;
				case 8: pCannon.HomingAngle = 24; break;
				case 9: pCannon.HomingAngle = 24; break;
				case 10: pCannon.HomingAngle = 24; break;
			}
			switch (pLevels.LevelCannonMultiFire)
			{
				case 0: pCannon.MultiGuns = 2; break;

				case 1: pCannon.MultiGuns = 3; break;
				case 2: pCannon.MultiGuns = 4; break;
				case 3: pCannon.MultiGuns = 5; break;
				case 4: pCannon.MultiGuns = 6; break;
				case 5: pCannon.MultiGuns = 7; break;

				case 6: pCannon.MultiGuns = 8; break;
				case 7: pCannon.MultiGuns = 9; break;
				case 8: pCannon.MultiGuns = 10; break;
				case 9: pCannon.MultiGuns = 11; break;
				case 10: pCannon.MultiGuns = 12; break;
			}
		}
	}
}
