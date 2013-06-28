namespace Evolution_War
{
	public class FollowController : WaypointController
	{
		private MovingObject TargetShip;

		public FollowController(MovingObject pTargetShip)
		{
			TargetShip = pTargetShip;
		}

		public override void Loop(MovingObject pShip)
		{
			InputStates.Clear();

			if ((TargetShip.Position - pShip.Position).Length > 16) // chase the player ship if it gets too far
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
			InputStates.DetectPresses();
		}
	}
}