using System.Collections.Generic;
using Axiom.Math;

namespace Evolution_War
{
	public abstract class BaseWaypointController : Controller
	{
		protected List<Vector2> Targets = new List<Vector2>();

		protected InputStates MoveToNextTarget(MovingObject pShip)
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
					Targets.RemoveAt(0);
				}
			}

			return state;
		}
	}
}