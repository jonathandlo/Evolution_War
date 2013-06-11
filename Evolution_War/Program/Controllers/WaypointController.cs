using System.Collections.Generic;
using Axiom.Math;

namespace Evolution_War
{
	public abstract class WaypointController : Controller
	{
		protected List<Vector2> Targets = new List<Vector2>();
		private InputStates temporaryAdditionInputState = new InputStates();

		protected InputStates MoveToNextTarget(MovingObject pShip)
		{
			temporaryAdditionInputState.Clear();

			if (Targets.Count <= 0) return temporaryAdditionInputState;

			var nextTarget = Targets[0];
			var vectorToTarget = nextTarget - pShip.Position; // Direction to target.
			var vectorToFacing = pShip.Velocity + 4 * Methods.AngleToVector(pShip.Angle * Constants.DegreesToRadians); // Ship's velocity plus a little bit of it's rotation.
			var vectorAlong = Methods.Projection(vectorToFacing, vectorToTarget); // The part of the facing vector that is in line with the target (how close we are to pointing to the target).
			var vectorAside = vectorToTarget - vectorAlong; // The part of the facing vector that is looking to the side of the target (how close we are to looking 90 degrees away from the target).

			if (vectorAside.Length < vectorAlong.Length && vectorToTarget.Dot(vectorToFacing) > 0) // Generally facing target.
				temporaryAdditionInputState.Up = true;
			if (vectorAside.Length * 8 > vectorAlong.Length || vectorToTarget.Dot(vectorToFacing) < 0) // Accurately facing target.
				if (vectorToFacing.Perpendicular.Dot(vectorToTarget) < 0) // Target is to the left.
					temporaryAdditionInputState.Left = true;
				else
					temporaryAdditionInputState.Right = true;
			if ((nextTarget - pShip.Position).Length < (Targets.Count > 1 ? 32 : 16)) // Are we there yet? More lenient if there are more waypoints to come.
				Targets.RemoveAt(0);

			return temporaryAdditionInputState;
		}
	}
}