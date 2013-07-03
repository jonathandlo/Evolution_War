using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiom.Core;
using Axiom.Graphics;

namespace Evolution_War
{
	public static class Constants
	{
		// Configuration constants.
		public static Int32 Width = 1280;
		public static Int32 Height = 720;

		// Optimization constants.
		public static readonly StringBuilder StringBuilder = new StringBuilder(32);
		public const Double DegreesToRadians = Math.PI / 180.0;

		// Gameplay constants.
		private const Double a = 8, b = 10, c = 16;
		public static readonly double[][] MultiFireAngles = // angle pattern for multifire trait.
		{
			new double[]              { -0,  0 }, // default.
			new[]                   { -a,  0,  a },
			new[]                 { -a, -0,  0,  a },
			new[]               { -b, -a,  0,  a,  b },
			new[]             { -b, -a, -0,  0,  a,  b },
			new[]           { -c, -b, -a,  0,  a,  b,  c }, // level 5.
			new[]         { -c, -b, -a, -0,  0,  a,  b,  c },
			new[]       { -c, -b, -a, -0,  0,  0,  a,  b,  c },
			new[]     { -c, -b, -a, -0, -0,  0,  0,  a,  b,  c },
			new[]   { -c, -b, -a, -0, -0,  0,  0,  0,  a,  b,  c },
			new[] { -c, -b, -a, -0, -0, -0,  0,  0,  0,  a,  b,  c }, // level 10.
		};

		private const Double d = 10, e = 20, f = 30, x = 5, y = 25, z = 60;
		public static readonly double[][] MultiFireOffsets = // offset pattern for multifire trait.
		{
			new[]                     { -d,  d }, // default.
			new[]                   { -d,  0,  d },
			new[]                 { -e, -d,  d,  e },
			new[]               { -e, -d,  0,  d,  e },
			new[]             { -f, -e, -d,  d,  e,  f },
			new[]           { -f, -e, -d,  0,  d,  e,  f }, // level 5.
			new[]         { -f, -e, -d, -x,  x,  d,  e,  f },
			new[]       { -f, -e, -d, -x,  0,  x,  d,  e,  f },
			new[]     { -f, -e, -d, -y, -x,  x,  y,  d,  e,  f },
			new[]   { -f, -e, -d, -y, -x,  0,  x,  y,  d,  e,  f },
			new[] { -f, -e, -d, -z, -y, -x,  x,  y,  z,  d,  e,  f }, // level 10.
		};

		// Visual constants.
		public static readonly ColorEx[] DamageColors = // bullet color for power trait.
		{
			ColorEx.White, ColorEx.White, ColorEx.White,
			ColorEx.White, ColorEx.White, ColorEx.White,
			ColorEx.White, ColorEx.White, ColorEx.White,
			ColorEx.White, ColorEx.White,
		};

		public static String[] DamageMaterialNames = // bullet color for power trait.
		{
			"Blue",
			"Azure",
			"Cyan",
			"SpringGreen",
			"Green",
			"Chartreuse",
			"Yellow",
			"Orange",
			"Red",
			"Rose",
			"Magenta",
		};

		public static void Load()
		{
			var colornames = new[]
			{
				"Blue",
				"Azure",
				"Cyan",
				"SpringGreen",
				"Green",
				"Chartreuse",
				"Yellow",
				"Orange",
				"Red",
				"Rose",
				"Magenta",
				"Violet",
			};

			var colors = new[]
			{
				new ColorEx(0.0f, 0.0f, 1.0f),
				new ColorEx(0.0f, 0.5f, 1.0f),
				new ColorEx(0.0f, 1.0f, 1.0f),
				new ColorEx(0.0f, 1.0f, 0.5f),
				new ColorEx(0.0f, 1.0f, 0.0f),
				new ColorEx(0.5f, 1.0f, 0.0f),
				new ColorEx(1.0f, 1.0f, 0.0f),
				new ColorEx(1.0f, 0.5f, 0.0f),
				new ColorEx(1.0f, 0.0f, 0.0f),
				new ColorEx(1.0f, 0.0f, 0.5f),
				new ColorEx(1.0f, 0.0f, 1.0f),
				new ColorEx(0.5f, 0.0f, 1.0f),
			};

			for (var i = 0; i < 12; i++)
			{
				// Create material resources.
				var material = MaterialManager.Instance.Create(colornames[i], ResourceGroupManager.DefaultResourceGroupName) as Material;
				if (material == null) return;

				material.Diffuse = colors[i] * 0.6f + new ColorEx(0.4f, 0.4f, 0.4f);
			}

			for (var i = 0; i < 11; i++)
			{
				// Set color constants.
				DamageColors[i] = colors[i];
			}
		}
	}
}
