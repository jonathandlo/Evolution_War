using Axiom.Math;

namespace Evolution_War
{
	public class RandomController : WaypointController
	{
		public override void Loop(MovingObject pShip)
		{
			if (Targets.Count == 0)
				Targets.Add(new Vector3((Methods.Random.NextDouble() - 0.5) * World.Instance.Arena.Width, (Methods.Random.NextDouble() - 0.5) * World.Instance.Arena.Height, 0));

			InputStates.Clear();
			//InputStates.Fire = Methods.Random.Next(128) == 0; // Temporary random fire.

			InputStates.Add(MoveToNextTarget(pShip));
			InputStates.DetectPresses();
		}
	}
}