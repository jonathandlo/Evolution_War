using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Forms;
using Axiom.Math;

namespace Evolution_War
{
	public abstract class Controller
	{
		protected InputStates inputStates;
		public InputStates InputStates { get { return inputStates; } }

		public Controller()
		{
			inputStates = new InputStates();
		}

		public abstract void Loop(Ship pShip, World pWorld); // updates the input states for external access
	}

	public class PlayerController : Controller
	{
		public override void Loop(Ship pShip, World pWorld)
		{
			inputStates.Up = Input.GetKey(Keys.Up);
			inputStates.Down = Input.GetKey(Keys.Down);
			inputStates.Left = Input.GetKey(Keys.Left);
			inputStates.Right = Input.GetKey(Keys.Right);
			inputStates.X = Input.GetKey(Keys.X);
			inputStates.Z = Input.GetKey(Keys.Z);
		}
	}

	public class FollowController : Controller
	{
		private List<Vector2> Targets;

		public FollowController()
		{
			Targets = new List<Vector2>();
		}

		public override void Loop(Ship pShip, World pWorld)
		{
			if (Targets.Count == 0 || (pWorld.PlayerShip.Position - Targets[Targets.Count - 1]).Length > 8)
			{
				Debug.WriteLine("     AI : Tracking Added");
				Targets.Add(pWorld.PlayerShip.Position);
			}

			inputStates = MoveToNextTarget(pShip);
		}
		private InputStates MoveToNextTarget(Ship pShip)
		{
			var state = new InputStates();

			if (Targets.Count > 0)
			{
				var nextTarget = Targets[0];
				var vectorToTarget = nextTarget - pShip.Position;
				var vectorToFacing = Methods.AngleToVector(pShip.Angle * Methods.DegreesToRadians); // may need to average with velocity vector for improved navigation
				var vectorAlong = Methods.Projection(vectorToFacing, vectorToTarget);
				var vectorAside = vectorToTarget - vectorAlong;

				if (vectorToTarget.Dot(vectorToFacing) > 0 || vectorToTarget.Length > 64) // the ship is facing its target || far enough away
				{
					Debug.WriteLine("     AI : Up - " + vectorToTarget.Length.ToString());

					if (vectorToFacing.Perpendicular.Dot(vectorToTarget) < 0) // target is to the left
					{
						state.Left = true;
					}
					else
					{
						state.Right = true;
					}

					if (vectorAside.Length < vectorAlong.Length) // thrust if target is in frontal cone
					{
						state.Up = true;
					}
				}
				else // to prevent orbit, fly away first
				{
					Debug.WriteLine("     AI : Turn");
					state.Up = true;

					if (vectorToFacing.Perpendicular.Dot(vectorToTarget) > 0) // target is to the left
					{
						state.Left = true;
					}
					else
					{
						state.Right = true;
					}
				}

				if ((nextTarget - pShip.Position).Length < 32)
				{
					Debug.WriteLine("     AI : Waypoint Reached");
					Targets.RemoveAt(0);
				}
			}
			else
			{
				state.Down = true;
			}

			return state;
		}
	}

}
