using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiom.Core;
using Axiom.Math;

namespace Evolution_War
{
	public class SmoothCamera : Camera
	{
		public SceneNode Node { get; set; }
		private MovingObject target;

		private Int32 framesBehind; // number of recorded position values - 1. x[framesBehind] will give you the oldest x position
		private Vector3 cameraOffset = new Vector3(0, -32, 256);
		private List<Double> x, y, dx, dy;

		public SmoothCamera(String pName, SceneManager pSceneManager, MovingObject pTarget, Int32 pFramesBehind)
			: base(pName, pSceneManager)
		{
			Node = pSceneManager.RootSceneNode.CreateChildSceneNode();
			Node.Position = cameraOffset;
			Node.AttachObject(this);

			x = new List<double>(pFramesBehind);
			y = new List<double>(pFramesBehind);
			dx = new List<double>(pFramesBehind);
			dy = new List<double>(pFramesBehind);
			framesBehind = Math.Max(1, pFramesBehind);
			target = pTarget;

			for (var i = 0; i < pFramesBehind; i++)
			{
				x.Add(pTarget.Position.x);
				y.Add(pTarget.Position.y);
				dx.Add(0);
				dy.Add(0);
			}

			x.Insert(0, pTarget.Position.x);
			y.Insert(0, pTarget.Position.y);
			dx.Insert(0, pTarget.Velocity.x);
			dy.Insert(0, pTarget.Velocity.y);

			isYawFixed = true;
			FixedYawAxis = Vector3.UnitZ;
			Near = 5;
			AutoAspectRatio = true;
		}

		public void Loop()
		{
			x.RemoveAt(x.Count - 1);
			y.RemoveAt(y.Count - 1);
			dx.RemoveAt(dx.Count - 1);
			dy.RemoveAt(dy.Count - 1);

			x.Insert(0, target.Position.x);
			y.Insert(0, target.Position.y);
			dx.Insert(0, target.Velocity.x);
			dy.Insert(0, target.Velocity.y);
		}

		public void Draw(Double pPercent)
		{
			Node.Position = new Vector3(
				cameraOffset.x + Methods.CubicStep(x[framesBehind], dx[framesBehind], x[framesBehind - 1], dx[framesBehind - 1], pPercent),
				cameraOffset.y + Methods.CubicStep(y[framesBehind], dy[framesBehind], y[framesBehind - 1], dy[framesBehind - 1], pPercent),
				cameraOffset.z + 32 * Methods.LinearStep(target.OldVelocity.LengthSquared, target.Velocity.LengthSquared, pPercent));
			Node.LookAt(new Vector3(
				Methods.CubicStep(x[framesBehind / 2 + 1], dx[framesBehind / 2 + 1], x[framesBehind / 2], dx[framesBehind / 2], pPercent),
				Methods.CubicStep(y[framesBehind / 2 + 1], dy[framesBehind / 2 + 1], y[framesBehind / 2], dy[framesBehind / 2], pPercent),
				0), TransformSpace.Parent);
		}
	}
}
