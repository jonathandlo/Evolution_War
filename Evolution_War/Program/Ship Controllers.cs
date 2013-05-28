using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Forms;
using Axiom.Math;

namespace Evolution_War
{
	public abstract class Controller
	{
		public InputStates InputStates = new InputStates(); // determines what keys the AI will press after every Loop()

		public abstract void Loop(Ship pShip, World pWorld); // updates the input states for external access
	}

	public class PlayerController : Controller
	{
		public override void Loop(Ship pShip, World pWorld)
		{
			InputStates.Up = Input.GetKey(Keys.Up);
			InputStates.Down = Input.GetKey(Keys.Down);
			InputStates.Left = Input.GetKey(Keys.Left);
			InputStates.Right = Input.GetKey(Keys.Right);
			InputStates.X = Input.GetKey(Keys.X);
			InputStates.Z = Input.GetKey(Keys.Z);
		}
	}

	public abstract class BaseWaypointController : Controller
	{
		protected List<Vector2> Targets = new List<Vector2>();

		protected InputStates MoveToNextTarget(Ship pShip)
		{
			var state = new InputStates();

			if (Targets.Count > 0) 
			{
				var nextTarget = Targets[0];
				var vectorToTarget = nextTarget - pShip.Position; // direction to target
				var vectorToFacing = pShip.Velocity + 2 * Methods.AngleToVector(pShip.Angle * Methods.DegreesToRadians); // ship's velocity plus a little bit of it's rotation
				var vectorAlong = Methods.Projection(vectorToFacing, vectorToTarget); // the part of the facing vector that is in line with the target (how close we are to pointing to the target)
				var vectorAside = vectorToTarget - vectorAlong; // the part of the facing vector that is looking to the side of the target (how close we are to looking 90 degrees away from the target)

				if (vectorAside.Length < vectorAlong.Length && vectorToTarget.Dot(vectorToFacing) > 0) // generally facing target
					state.Up = true;

				if (vectorAside.Length * 8 > vectorAlong.Length || vectorToTarget.Dot(vectorToFacing) < 0) // accurately facing target
				{
					if (vectorToFacing.Perpendicular.Dot(vectorToTarget) < 0) // target is to the left
						state.Left = true;
					else
						state.Right = true;
				}

				if ((nextTarget - pShip.Position).Length < (Targets.Count > 1 ? 32 : 16)) // are we there yet? more lenient if there are more waypoints to come
				{
					// Debug.WriteLine("     AI: Waypoint Reached");
					Targets.RemoveAt(0);
				}
			}

			return state;
		}
	}

	public class FollowController : BaseWaypointController
	{
		private Ship TargetShip;

		public FollowController(Ship pTargetShip)
		{
			TargetShip = pTargetShip;
		}

		public override void Loop(Ship pShip, World pWorld)
		{
			InputStates.Clear();

//			Debug.WriteLine("angle between: " + Methods.AngleDifference(pShip.Angle, TargetShip.Angle));
//			Debug.WriteLine("targets: " + Targets.Count + " distance: " + (TargetShip.Position - pShip.Position).Length.ToString());

			if ((TargetShip.Position - pShip.Position).Length > 32) // chase the player ship if it gets too far
			{
				Targets.Clear();
				Targets.Add(TargetShip.Position);
			}
			else if (Targets.Count == 0) // face the same way as the player ship once you reach it
			{
				InputStates.Left = Methods.AngleDifference(pShip.Angle, TargetShip.Angle) < -15;
				InputStates.Right = Methods.AngleDifference(pShip.Angle, TargetShip.Angle) > 15;
			}

			InputStates.Add(MoveToNextTarget(pShip));
		}
	}

}
