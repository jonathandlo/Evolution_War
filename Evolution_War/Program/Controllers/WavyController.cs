namespace Evolution_War
{
	public class WavyController : Controller {
		public override void Loop(MovingObject pShip)
		{
			if (Methods.Random.Next(32) == 0)
			{
				InputStates.Clear();

				if (Methods.Random.Next() > 0)
					InputStates.Left = true;
				else
					InputStates.Right = true;
				
				InputStates.Up = true;
			}
		}
	}
}