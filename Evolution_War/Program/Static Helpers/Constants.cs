using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_War
{
	public static class Constants
	{
		// Configuration constants.
		public static Int32 Width = 1280;
		public static Int32 Height = 720;

		// Optimzation constants.
		public static readonly StringBuilder StringBuilder = new StringBuilder(32);
		public const Double DegreesToRadians = Math.PI / 180.0;

		// Gameplay constants.
		private const Double a = 8, b = 10, c = 16;
		public static readonly double[][] MultiFireAngles = // angle pattern for multifire trait.
		{
			new double[]              { -0,  0 }, // default
			new[]                   { -a,  0,  a }, // level 1
			new[]                 { -a, -0,  0,  a },
			new[]               { -b, -a,  0,  a,  b },
			new[]             { -b, -a, -0,  0,  a,  b },
			new[]           { -c, -b, -a,  0,  a,  b,  c }, // level 5
			new[]         { -c, -b, -a, -0,  0,  a,  b,  c },
			new[]       { -c, -b, -a, -0,  0,  0,  a,  b,  c },
			new[]     { -c, -b, -a, -0, -0,  0,  0,  a,  b,  c },
			new[]   { -c, -b, -a, -0, -0,  0,  0,  0,  a,  b,  c },
			new[] { -c, -b, -a, -0, -0, -0,  0,  0,  0,  a,  b,  c }, // level 10
		};

		private const Double d = 10, e = 20, f = 30, x = 5, y = 25, z = 60;
		public static readonly double[][] MultiFireOffsets = // offset pattern for multifire trait.
		{
			new[]                     { -d,  d }, // default
			new[]                   { -d,  0,  d }, // level 1
			new[]                 { -e, -d,  d,  e },
			new[]               { -e, -d,  0,  d,  e },
			new[]             { -f, -e, -d,  d,  e,  f },
			new[]           { -f, -e, -d,  0,  d,  e,  f }, // level 5
			new[]         { -f, -e, -d, -x,  x,  d,  e,  f },
			new[]       { -f, -e, -d, -x,  0,  x,  d,  e,  f },
			new[]     { -f, -e, -d, -y, -x,  x,  y,  d,  e,  f },
			new[]   { -f, -e, -d, -y, -x,  0,  x,  y,  d,  e,  f },
			new[] { -f, -e, -d, -z, -y, -x,  x,  y,  z,  d,  e,  f }, // level 10
		};
	}
}
