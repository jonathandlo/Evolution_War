using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_War
{
	public class Upgrades
	{
		public const Int32 NumberOfUpgrades = 10;

		public Upgrade CannonPower		= new Upgrade("Power",		1, 0);
		public Upgrade CannonSpeed		= new Upgrade("Projector",	1, 0);
		public Upgrade CannonAutoFire	= new Upgrade("AutoFire",	2, 0);
		public Upgrade CannonHoming		= new Upgrade("Aimbot",		2, 0);
		public Upgrade CannonMultiFire	= new Upgrade("MultiFire",	2, 0);

		public Upgrade ShipThrust		= new Upgrade("Thrust",		3, 0);
		public Upgrade ShipTurn			= new Upgrade("Turn",		4, 0);
		public Upgrade ShipArmor		= new Upgrade("Armor",		4, 0);
		public Upgrade ShipShield		= new Upgrade("Shield",		6, 0);
		public Upgrade ShipShieldRegen	= new Upgrade("Regen",		6, 0);

		public Dictionary<Int32, List<Upgrade>> GetUpgradesByCost()
		{
			var masterList = new Dictionary<Int32, List<Upgrade>>();

			AddUpgradeByCost(CannonPower, masterList);
			AddUpgradeByCost(CannonSpeed, masterList);
			AddUpgradeByCost(CannonAutoFire, masterList);
			AddUpgradeByCost(CannonHoming, masterList);
			AddUpgradeByCost(CannonMultiFire, masterList);

			AddUpgradeByCost(ShipThrust, masterList);
			AddUpgradeByCost(ShipTurn, masterList);
			AddUpgradeByCost(ShipArmor, masterList);
			AddUpgradeByCost(ShipShield, masterList);
			AddUpgradeByCost(ShipShieldRegen, masterList);

			return masterList;
		}
		private void AddUpgradeByCost(Upgrade pUpgrade, Dictionary<Int32, List<Upgrade>> pMasterList)
		{
			if (!pMasterList.ContainsKey(pUpgrade.Cost)) pMasterList.Add(pUpgrade.Cost, new List<Upgrade>());
			pMasterList[pUpgrade.Cost].Add(pUpgrade);
		}

		public static void UpgradeCannon(ref Cannon pCannon, Upgrades pLevels)
		{
			switch (pLevels.CannonPower.Level)
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
			switch (pLevels.CannonSpeed.Level)
			{
				case 0: pCannon.Speed = 4; break;

				case 1: pCannon.Speed = 4.5; break;
				case 2: pCannon.Speed = 5; break;
				case 3: pCannon.Speed = 5.5; break;
				case 4: pCannon.Speed = 6; break;
				case 5: pCannon.Speed = 7; break;

				case 6: pCannon.Speed = 8; break;
				case 7: pCannon.Speed = 8; break;
				case 8: pCannon.Speed = 8; break;
				case 9: pCannon.Speed = 8; break;
				case 10: pCannon.Speed = 8; break;
			}
			switch (pLevels.CannonAutoFire.Level)
			{
				case 0: pCannon.Delay = 18; break;

				case 1: pCannon.Delay = 16; break;
				case 2: pCannon.Delay = 14; break;
				case 3: pCannon.Delay = 12; break;
				case 4: pCannon.Delay = 10; break;
				case 5: pCannon.Delay = 8; break;

				case 6: pCannon.Delay = 7; break;
				case 7: pCannon.Delay = 6; break;
				case 8: pCannon.Delay = 5; break;
				case 9: pCannon.Delay = 4; break;
				case 10: pCannon.Delay = 3; break;
			}
			switch (pLevels.CannonHoming.Level)
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
			switch (pLevels.CannonMultiFire.Level)
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

	public class Upgrade
	{
		public String Name;
		public Int32 Cost;
		public Int32 Level;

		public Upgrade(String pName, Int32 pCost, Int32 pLevel)
		{
			Name = pName;
			Cost = pCost;
			Level = pLevel;
		}
	}
}
