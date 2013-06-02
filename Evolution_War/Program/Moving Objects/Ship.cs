using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Axiom.Core;
using Axiom.Graphics;
using Axiom.Math;

namespace Evolution_War
{
	public class Ship : MovingObject, BasicDrawable
	{
		public Ship(SceneManager pSceneManager, Controller pController, String pPrefix = "Ship ")
			: base(pController)
		{
			string name = pPrefix + Methods.GenerateUniqueID();

			Node = pSceneManager.RootSceneNode.CreateChildSceneNode(name);
			MeshNode = Node.CreateChildSceneNode();
			MeshNode.Orientation = new Quaternion(0.5, 0.5, -0.5, -0.5);
			MeshNode.AttachObject(pSceneManager.CreateEntity(name, "ship_assault_1.mesh"));
		}
	}
}
