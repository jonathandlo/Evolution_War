using System.Windows.Forms;

namespace Evolution_War
{
	public class PlayerController : Controller
	{
		public override void Loop(MovingObject pShip, World pWorld)
		{
			InputStates.Up = Input.GetKey(Keys.Up);
			InputStates.Down = Input.GetKey(Keys.Down);
			InputStates.Left = Input.GetKey(Keys.Left);
			InputStates.Right = Input.GetKey(Keys.Right);
			InputStates.C = Input.GetKey(Keys.C);
			InputStates.X = Input.GetKey(Keys.X);
			InputStates.Z = Input.GetKey(Keys.Z);
		}
	}
}