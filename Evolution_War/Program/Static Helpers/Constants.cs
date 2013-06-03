using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_War
{
	public static class Constants
	{
		public static readonly StringBuilder StringBuilder = new StringBuilder(32);
		public const Double DegreesToRadians = Math.PI / 180.0;

		private const Double a = 10, b = 20, c = 30;
		public static readonly double[][] MultiFireAngles = // angle pattern for multifire trait
		{
			new double[]                     { -0,  0 }, // default
			new double[]                   { -a,  0,  a }, // level 1
			new double[]                 { -a, -0,  0,  a },
			new double[]               { -b, -a,  0,  a,  b },
			new double[]             { -b, -a, -0,  0,  a,  b },
			new double[]           { -c, -b, -a,  0,  a,  b,  c }, // level 5
			new double[]         { -c, -b, -a, -0,  0,  a,  b,  c },
			new double[]       { -c, -b, -a, -0,  0,  0,  a,  b,  c },
			new double[]     { -c, -b, -a, -0, -0,  0,  0,  a,  b,  c },
			new double[]   { -c, -b, -a, -0, -0,  0,  0,  0,  a,  b,  c },
			new double[] { -c, -b, -a, -0, -0, -0,  0,  0,  0,  a,  b,  c }, // level 10
		};

		private const Double d = 10, e = 20, f = 30, x = 5, y = 25, z = 60;
		public static readonly double[][] MultiFireOffsets = // angle pattern for multifire trait
		{
			new double[]                     { -d,  d }, // default
			new double[]                   { -d,  0,  d }, // level 1
			new double[]                 { -e, -d,  d,  e },
			new double[]               { -e, -d,  0,  d,  e },
			new double[]             { -f, -e, -d,  d,  e,  f },
			new double[]           { -f, -e, -d,  0,  d,  e,  f }, // level 5
			new double[]         { -f, -e, -d, -x,  x,  d,  e,  f },
			new double[]       { -f, -e, -d, -x,  0,  x,  d,  e,  f },
			new double[]     { -f, -e, -d, -y, -x,  x,  y,  d,  e,  f },
			new double[]   { -f, -e, -d, -y, -x,  0,  x,  y,  d,  e,  f },
			new double[] { -f, -e, -d, -z, -y, -x,  x,  y,  z,  d,  e,  f }, // level 10
		};
	}
}
