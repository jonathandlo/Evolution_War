using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Evolution_War
{
	public static class Input
	{
		[DllImport("user32.dll")]
		private static extern short GetAsyncKeyState(Keys pKey);
		public static Boolean GetKey(Keys pKey){ return GetAsyncKeyState(pKey) < 0; }
	}

	public struct InputStates
	{
		public Boolean Up;
		public Boolean Down;
		public Boolean Left;
		public Boolean Right;
		public Boolean X;
		public Boolean Z;

		public void Add(InputStates pOther)
		{
			Up = Up || pOther.Up;
			Down = Down || pOther.Down;
			Left = Left || pOther.Left;
			Right = Right || pOther.Right;
			X = X || pOther.X;
			Z = Z || pOther.Z;
		}

        public void Clear()
        {
            Up = false;
            Down = false;
            Left = false;
            Right = false;
            X = false;
            Z = false;
        }
	}
}
