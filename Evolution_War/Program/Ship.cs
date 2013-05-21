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
		public SceneNode Node { get; private set; }

		private Controller controller;
		private Double x, y, ox, oy; // position
		private Double dx, dy, odx, ody; // velocity
		private Double a, oa; // angle (degrees)
		private Double da, oda; // angular velocity

		public Vector2 Position
		{
			get { return new Vector2(x, y); }
		}
		public Vector2 Velocity
		{
			get { return new Vector2(dx, dy); }
		}
		public Double Angle
		{
			get { return a; }
		}

		public Ship(SceneNode pNode, Controller pController)
		{
			Node = pNode;
			controller = pController;

			x = ox = pNode.Position.x;
			y = oy = pNode.Position.y;
		}

		public void Loop(World pWorld)
		{
			// update input
			controller.Loop(this, pWorld);

			// position and angle memory
			ox = x;
			oy = y;
			odx = dx;
			ody = dy;
			oa = a;
			oda = da;

			// thrust
			dx += 0.1 * Math.Cos(a * Methods.DegreesToRadians) * (controller.InputStates.Up ? 1 : 0);
			dy += 0.1 * Math.Sin(a * Methods.DegreesToRadians) * (controller.InputStates.Up ? 1 : 0);
			da += 0.8 * ((controller.InputStates.Right ? -1 : 0) + (controller.InputStates.Left ? 1 : 0));

			// dynamic friction
			dx *= (1 - 0.04) - (controller.InputStates.Down ? 0.1 : 0);
			dy *= (1 - 0.04) - (controller.InputStates.Down ? 0.1 : 0);
			da *= (1 - 0.16) - (controller.InputStates.Down ? 0.1 : 0);

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

		public void Draw(Double pPercent)
		{
			Node.Position = new Vector3(Methods.CubicStep(ox, odx, x, dx, pPercent), Methods.CubicStep(oy, ody, y, dy, pPercent), 0);
			Node.Orientation = Quaternion.FromEulerAnglesInDegrees(90.0, 0.0, 0.0);
			Node.Rotate(Quaternion.FromEulerAnglesInDegrees(0.0, -16 * Methods.LinearStep(oda, da, pPercent), 0.0), TransformSpace.World);
			Node.Rotate(Quaternion.FromEulerAnglesInDegrees(0.0, 0.0, Methods.CubicStep(oa, oda, a, da, pPercent) - 90.0f), TransformSpace.World);
		}
	}
}
