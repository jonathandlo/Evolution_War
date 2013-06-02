using System;
using Axiom.Core;
using Axiom.Math;

namespace Evolution_War
{
	public class Bullet : MovingObject, BasicDrawable
	{
		public Ship Owner { get; protected set; }

		public Bullet(SceneManager pSceneManager, Controller pController, Ship pShip, String pPrefix = "Bullet ")
			: base(pController)
		{
			string name = pPrefix + Methods.GenerateUniqueID();

			Node = pSceneManager.RootSceneNode.CreateChildSceneNode(name);
			MeshNode = Node.CreateChildSceneNode();
			MeshNode.Orientation = new Quaternion(0.5, 0.5, -0.5, -0.5);
			MeshNode.AttachObject(pSceneManager.CreateEntity(name, "ship_assault_1.mesh"));

			Owner = pShip;
		}
	}
}