using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Axiom.Core;
using Axiom.Graphics;
using Axiom.Math;

namespace Evolution_War
{
	public class Ship
	{
		public SceneNode node { get; private set; }

		private double x, y, ox, oy; // positions
		private double dx, dy, odx, ody; // speeds
		private double a, oa; // angles (degrees)
		private double da, oda; // angular speeds

		public Ship(SceneNode pNode)
		{
			node = pNode;
		}

		public void Loop(Controller pController)
		{
			// update input
			pController.Loop();

			// position and angle memory
			ox = x;
			oy = y;
			odx = dx;
			ody = dy;
			oa = a;
			oda = da;

			// thrust
			dx += 0.1 * Math.Cos(a * Methods.DegreesToRadians) * (pController.InputStates.Up ? 1 : 0);
			dy += 0.1 * Math.Sin(a * Methods.DegreesToRadians) * (pController.InputStates.Up ? 1 : 0);
			da += 0.8 * ((pController.InputStates.Right ? -1 : 0) + (pController.InputStates.Left ? 1 : 0));

			// dynamic friction
			dx *= (1 - 0.04) - (pController.InputStates.Down ? 0.1 : 0);
			dy *= (1 - 0.04) - (pController.InputStates.Down ? 0.1 : 0);
			da *= (1 - 0.16) - (pController.InputStates.Down ? 0.1 : 0);

			// static friction
			dx -= dx > 0 ? Math.Min(0.01, Math.Abs(dx)) * Math.Sign(dx) : 0;
			dy -= dy > 0 ? Math.Min(0.01, Math.Abs(dy)) * Math.Sign(dy) : 0;
			da -= da > 0 ? Math.Min(0.01, Math.Abs(da)) * Math.Sign(da) : 0;

			// advance position
			x += dx;
			y += dy;
			oa = a % 360 + oa - a;
			a = a % 360 + da;
		}

		public void Draw(double pPercent)
		{
			node.Position = new Vector3(Methods.CubicStep(ox, odx, x, dx, pPercent), Methods.CubicStep(oy, ody, y, dy, pPercent), 0);
			node.Orientation = Quaternion.FromEulerAnglesInDegrees(90.0, 0.0, 0.0);
			node.Rotate(Quaternion.FromEulerAnglesInDegrees(0.0, -16 * Methods.LinearStep(oda, da, pPercent), 0.0), TransformSpace.World);
			node.Rotate(Quaternion.FromEulerAnglesInDegrees(0.0, 0.0, Methods.CubicStep(oa, oda, a, da, pPercent) - 90.0f), TransformSpace.World);
		}
	}
}
