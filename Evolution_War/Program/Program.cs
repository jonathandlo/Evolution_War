using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evolution_War
{
	internal class Program
	{
		private static void Main()
		{
			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);
			(new Game()).Run();
		}
	}
}
