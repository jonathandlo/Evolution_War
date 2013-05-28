using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiom.Core;
using Axiom.Math;

namespace Evolution_War
{
	public class Level
	{
		public SceneNode Node { get; set; }

		public Int32 Width { get; private set; }
		public Int32 Height { get; private set; }
		public Int32 Left { get; private set; }
		public Int32 Right { get; private set; }
		public Int32 Top { get; private set; }
		public Int32 Bottom { get; private set; }

		public Level(SceneManager pSceneManager, Int32 pScale, Int32 pNumAdditionalGrids)
		{
			var random = new Random();
			var topgrid = pSceneManager.CreateEntity("grid top", "grid.mesh");
			var topgridnode = pSceneManager.RootSceneNode.CreateChildSceneNode("grid top");
			
			topgridnode.Position = new Vector3(0, 0, -10);
			topgridnode.Orientation = Quaternion.FromAxes(Vector3.UnitX, Vector3.UnitZ, Vector3.NegativeUnitY);
			topgridnode.Scale = new Vector3(pScale, 1, pScale);
			topgridnode.AttachObject(topgrid);

			Width = (Int32)(topgrid.BoundingBox.Size.x * pScale);
			Height = (Int32)(topgrid.BoundingBox.Size.z * pScale);

			Right = Width / 2;
			Bottom = Height / 2;
			Left = -Right;
			Top = -Bottom;

			for (var i = 1; i <= pNumAdditionalGrids; i++)
			{
				var grid = pSceneManager.CreateEntity("grid" + i, "grid.mesh");
				var gridnode = pSceneManager.RootSceneNode.CreateChildSceneNode("grid" + i);

				gridnode.Position = new Vector3(
					random.Next(-pScale / 5, pScale / 5),
					random.Next(-pScale / 5, pScale / 5),
					-10 - pScale * i / 10);
				gridnode.Orientation = Quaternion.FromAxes(
					Vector3.UnitX,
					Vector3.UnitZ,
					Vector3.NegativeUnitY);
				gridnode.Scale = new Vector3(
					pScale + i * random.Next(pScale / 8, pScale / 4),
					1,
					pScale + i * random.Next(pScale / 8, pScale / 4));
				gridnode.AttachObject(grid);
			}
		}
	}
}
