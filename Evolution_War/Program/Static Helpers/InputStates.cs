using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Evolution_War
{
	public class InputStates
	{
		[DllImport("user32.dll")]
		private static extern Int32 GetAsyncKeyState(Keys pKey);
		public static Boolean GetKey(Keys pKey) { return GetAsyncKeyState(pKey) < 0; }

		public Boolean Up;			// thrust.
		public Boolean Down;		// brake.
		public Boolean Left;		// turn.
		public Boolean Right;		// turn.
		public Boolean Fire;		// fire main.
		public Boolean Secondary;	// fire secondary.
		public Boolean Upgrade;		// upgrade.

		public Boolean DeltaUp;
		public Boolean DeltaDown;
		public Boolean DeltaLeft;
		public Boolean DeltaRight;
		public Boolean DeltaFire;
		public Boolean DeltaSecondary;
		public Boolean DeltaUpgrade;

		// For detecting "key down" events
		private Boolean	OldUp;
		private Boolean	OldDown;
		private Boolean	OldLeft;
		private Boolean	OldRight;
		private Boolean	OldFire;
		private Boolean	OldSecondary;
		private Boolean	OldUpgrade;

		public void Add(InputStates pOther)
		{
			Up = Up || pOther.Up;
			Down = Down || pOther.Down;
			Left = Left || pOther.Left;
			Right = Right || pOther.Right;
			Fire = Fire || pOther.Fire;
			Secondary = Secondary || pOther.Secondary;
			Upgrade = Upgrade || pOther.Upgrade;
		}

		public void DetectPresses() // Determine "key down" events during a physics step.
		{
			DeltaUp = Up && !OldUp;
			DeltaDown = Down && !OldDown;
			DeltaLeft = Left && !OldLeft;
			DeltaRight = Right && !OldRight;
			DeltaFire = Fire && !OldFire;
			DeltaSecondary = Secondary && !OldSecondary;
			DeltaUpgrade = Upgrade && !OldUpgrade;

			OldUp = Up;
			OldDown = Down;
			OldLeft = Left;
			OldRight = Right;
			OldFire = Fire;
			OldSecondary = Secondary;
			OldUpgrade = Upgrade;
		}

		public void Clear()
		{
			Up = false;
			Down = false;
			Left = false;
			Right = false;
			Fire = false;
			Secondary = false;
			Upgrade = false;
		}
	}
}
