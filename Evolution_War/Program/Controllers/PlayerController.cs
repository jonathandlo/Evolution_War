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
			InputStates.C = InputStates.GetKey(Keys.C);
			InputStates.X = InputStates.GetKey(Keys.X);
			InputStates.Z = InputStates.GetKey(Keys.Z);
		}
	}
}