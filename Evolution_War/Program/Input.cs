using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Evolution_War
{
	public static class Input
	{
		[DllImport("user32.dll")]
		private static extern short GetAsyncKeyState(Keys pKey);

		public static Boolean GetKey(Keys pKey)
		{
			return GetAsyncKeyState(pKey) < 0;
		}
	}

	public struct InputStates
	{
		public Boolean Up;
		public Boolean Down;
		public Boolean Left;
		public Boolean Right;
		public Boolean X;
		public Boolean Z;
	}

	public abstract class Controller
	{
		protected InputStates inputStates;
		public InputStates InputStates { get { return inputStates; } }

		public Controller()
		{
			inputStates = new InputStates();
		}

		public abstract void Loop(); // updates the input states for external access
	}

	public class PlayerController : Controller
	{
		public override void Loop()
		{
			inputStates.Up = Input.GetKey(Keys.Up);
			inputStates.Down = Input.GetKey(Keys.Down);
			inputStates.Left = Input.GetKey(Keys.Left);
			inputStates.Right = Input.GetKey(Keys.Right);
			inputStates.X = Input.GetKey(Keys.X);
			inputStates.Z = Input.GetKey(Keys.Z);
		}
	}
}
