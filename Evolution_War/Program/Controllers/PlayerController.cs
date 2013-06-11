using System.Windows.Forms;

namespace Evolution_War
{
	public class PlayerController : Controller
	{
		public override void Loop(MovingObject pShip, World pWorld)
		{
			InputStates.Up = InputStates.GetKey(Keys.Up);
			InputStates.Down = InputStates.GetKey(Keys.Down);
			InputStates.Left = InputStates.GetKey(Keys.Left);
			InputStates.Right = InputStates.GetKey(Keys.Right);
			InputStates.Fire = InputStates.GetKey(Keys.C);
			InputStates.Secondary = InputStates.GetKey(Keys.X);
			InputStates.Upgrade = InputStates.GetKey(Keys.Z);

			InputStates.DetectPresses();
		}
	}
}