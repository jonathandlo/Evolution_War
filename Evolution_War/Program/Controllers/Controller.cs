using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Evolution_War
{
	public abstract class Controller
	{
		public InputStates InputStates = new InputStates(); // determines what keys the AI will press after every Loop()

		public abstract void Loop(MovingObject pShip, World pWorld); // updates the input states for external access
	}
}
