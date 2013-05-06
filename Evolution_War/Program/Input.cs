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

		public static Boolean getKey(Keys pKey)
		{
			return Convert.ToBoolean(GetAsyncKeyState(pKey));
		}
	}
}
