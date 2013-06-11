﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiom.Core;
using Axiom.Math;

namespace Evolution_War
{
	public abstract class MovingObject
	{
		public LoopResultStates LoopResultStates = new LoopResultStates(); // tells the World what to do after Loop().
		public SceneNode Node { get; protected set; } // actual position and orientation.
		public SceneNode MeshNode { get; protected set; } // normalizes mesh orientation.

		protected Controller controller;
		protected Double x, y, ox, oy;		// position.
		protected Double dx, dy, odx, ody;	// velocity.
		protected Double a, oa;				// angle (degrees).
		protected Double da, oda;			// angular velocity.

		public Vector2 Position { get { return new Vector2(x, y); } }
		public Vector2 OldPosition { get { return new Vector2(ox, oy); } }
		public Vector2 Velocity { get { return new Vector2(dx, dy); } }
		public Vector2 OldVelocity { get { return new Vector2(odx, ody); } }
		public Double Angle { get { return a; } }
		public Double AngleVelocity { get { return da; } }

		public MovingObject(Controller pController)
		{
			controller = pController;
		}

		public void Loop(World pWorld)
		{
			LoopResultStates.Clear();

			// update input.
			controller.Loop(this, pWorld);

			// position and angle memory.
			ox = x;
			oy = y;
			odx = dx;
			ody = dy;
			oa = a;
			oda = da;

			// overridable control and friction physics.
			LoopControlPhysics(pWorld);
			LoopFrictionPhysics();
			LoopCollisionPhysics(pWorld);

			// advance position.
			x += dx;
			y += dy;
			oa = a % 360 + oa - a;
			a = a % 360 + da;
		}

		protected virtual void LoopControlPhysics(World pWorld)
		{
			// thrust.
			dx += 0.2 * Math.Cos(a * Constants.DegreesToRadians) * (controller.InputStates.Up ? 1 : 0);
			dy += 0.2 * Math.Sin(a * Constants.DegreesToRadians) * (controller.InputStates.Up ? 1 : 0);
			da += 0.6 * ((controller.InputStates.Right ? -1 : 0) + (controller.InputStates.Left ? 1 : 0));
		}

		protected virtual void LoopFrictionPhysics()
		{
			// dynamic friction.
			dx *= (1 - 0.08) - (controller.InputStates.Down ? 0.1 : 0);
			dy *= (1 - 0.08) - (controller.InputStates.Down ? 0.1 : 0);
			da *= (1 - 0.12);

			// static friction.
			dx -= dx > 0 ? Math.Min(0.01, Math.Abs(dx)) * Math.Sign(dx) : 0;
			dy -= dy > 0 ? Math.Min(0.01, Math.Abs(dy)) * Math.Sign(dy) : 0;
			da -= da > 0 ? Math.Min(0.01, Math.Abs(da)) * Math.Sign(da) : 0;
		}

		protected virtual void LoopCollisionPhysics(World pWorld)
		{
			// wall collision.
			if (x + dx > pWorld.Arena.Right) dx = -Math.Abs(dx);
			else if (x + dx < pWorld.Arena.Left) dx = Math.Abs(dx);
			if (y + dy > pWorld.Arena.Bottom) dy = -Math.Abs(dy);
			else if (y + dy < pWorld.Arena.Top) dy = Math.Abs(dy);
		}

		public virtual void Draw(Double pPercent)
		{
			Node.Position = new Vector3(Methods.CubicStep(ox, odx, x, dx, pPercent), Methods.CubicStep(oy, ody, y, dy, pPercent), 0);
			Node.Orientation =
				Quaternion.FromAngleAxis(Methods.CubicStep(oa, oda, a, da, pPercent) * Constants.DegreesToRadians, Vector3.UnitZ) *
				Quaternion.FromEulerAnglesInDegrees(-16 * Methods.LinearStep(oda, da, pPercent), 0.0, 0.0);
		}
	}
}
