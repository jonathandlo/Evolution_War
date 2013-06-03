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
		public static Boolean GetKey(Keys pKey){ return GetAsyncKeyState(pKey) < 0; }

		public Boolean Up;	  // thrust
		public Boolean Down;  // brake
		public Boolean Left;  // turn
		public Boolean Right; // turn
		public Boolean C;	  // fire main
		public Boolean X;	  // fire secondary
		public Boolean Z;	  // upgrade

		public void Add(InputStates pOther)
		{
			Up = Up || pOther.Up;
			Down = Down || pOther.Down;
			Left = Left || pOther.Left;
			Right = Right || pOther.Right;
			X = C || pOther.C;
			X = X || pOther.X;
			Z = Z || pOther.Z;
		}

		public void Clear()
		{
			Up = false;
			Down = false;
			Left = false;
			Right = false;
			C = false;
			X = false;
			Z = false;
		}
	}
}
