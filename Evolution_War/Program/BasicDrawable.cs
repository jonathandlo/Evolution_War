using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiom.Core;

namespace Evolution_War
{
	public interface BasicDrawable
	{
		SceneNode Node { get; }

		void Loop(World pWorld);
		void Draw(Double pPercent);
	}
}
