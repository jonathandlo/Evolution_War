using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiom.Core;
using Axiom.Graphics;
using Axiom.Math;

namespace Evolution_War
{
	public class Trail
	{
		public LoopResultStates LoopResultStates { get; private set; }
		protected MovingObject objectToFollow;

		// BillboardChain management.
		public BillboardChain Chain { get; protected set; }
		protected Vector3 deadPosition;
		protected int deadcountdown;
		protected bool stopping;

		protected List<BillboardChain.Element> elementz = new List<BillboardChain.Element>();

		// Visuals.
		protected Single maxwidth;
		protected ColorEx color;

		public Trail(MovingObject pObjectToFollow, Single pMaxWidth, ColorEx pColor)
		{
			LoopResultStates = new LoopResultStates();

			Chain = new BillboardChain(Methods.GenerateUniqueID.ToString());
			Chain.Material.SetSceneBlending(SceneBlendType.Replace);
			Chain.Material.DepthCheck = false;
			Chain.Material.DepthWrite = false;
			Chain.NumberOfChains = 1;
			Chain.IsVisible = false;
			World.Instance.SceneManager.RootSceneNode.AttachObject(Chain);

			Relaunch(pObjectToFollow, pMaxWidth, pColor);
		}

		public void Relaunch(MovingObject pObjectToFollow, Single pMaxWidth, ColorEx pColor)
		{
			objectToFollow = pObjectToFollow;
			maxwidth = pMaxWidth;
			color = pColor;
			stopping = false;
			elementz.Clear();
			deadcountdown = Chain.MaxChainElements = 7;

			DropTrail();
		}

		public void Draw(Double pPercent)
		{
			if (!stopping)
			{
				var oldelement = elementz[elementz.Count - 1];
				oldelement.Position = objectToFollow.Node.Position;
			}
		}

		public void Loop()
		{
			LoopResultStates.Clear();

			if (stopping) // allow decay.
			{
				deadcountdown--;

				if (deadcountdown == 0) // remove completely.
				{
					LoopResultStates.Remove = true;
				}
			}
			else // continue to follow.
			{
				DropTrail();
			}

			for (var i = 0; i < elementz.Count; i++)
			{
				var element = elementz[i];

				if (i == 0 || i < elementz.Count - deadcountdown)
				{
					// hide if stopping
					element.Width = 0;
					element.Color = ColorEx.Black;
				}
				else
				{
					// regular decay.
					element.Width *= 0.9f;
					element.Color *= 0.8f;
				}
			}
		}

		private void DropTrail()
		{
			if (elementz.Count > 0)
			{
				// set old element.
				var oldelement = elementz[elementz.Count - 1];
				oldelement.Position = objectToFollow.OldPosition;
				oldelement.Color = color;
			}

			// add new element.
			var newelement = new BillboardChain.Element();
			newelement.Position = objectToFollow.OldPosition;
			newelement.Width = maxwidth;
			newelement.Color = ColorEx.White;

			Chain.AddChainElement(0, newelement);
			elementz.Add(newelement);

			if (elementz.Count > Chain.MaxChainElements)
			{
				elementz.RemoveAt(0);
			}
		}

		public void ObjectToFollowDisappeared()
		{
			// stop creating.
			stopping = true;
			deadcountdown = Math.Min(elementz.Count, Chain.MaxChainElements);
			deadPosition = objectToFollow.Position;

			if (elementz.Count != 0) elementz[elementz.Count - 1].Color = color;
		}

		public void Recycle()
		{
			Chain.IsVisible = false;
			elementz.Clear();
		}
	}

	//	public class CustomBillboardChain : BillboardChain
	//	{
	//		public CustomBillboardChain(string name, int maxElements, int numberOfChains, bool useTextureCoords, bool useColors, bool dynamic) : base(name, maxElements, numberOfChains, useTextureCoords, useColors, dynamic) { }
	//		public CustomBillboardChain(string name, int maxElements, int numberOfChains, bool useTextureCoords, bool useColors) : base(name, maxElements, numberOfChains, useTextureCoords, useColors) { }
	//		public CustomBillboardChain(string name, int maxElements, int numberOfChains, bool useTextureCoords) : base(name, maxElements, numberOfChains, useTextureCoords) { }
	//		public CustomBillboardChain(string name, int maxElements, int numberOfChains) : base(name, maxElements, numberOfChains) { }
	//		public CustomBillboardChain(string name, int maxElements) : base(name, maxElements) { }
	//		public CustomBillboardChain(string name) : base(name) { }
	//
	//		public void ClearChain(int chainIndex)
	//		{
	//			if (chainIndex >= chainCount)
	//			{
	//				throw new Exception("ERR_ITEM_NOT_FOUND : chainIndex out of bounds : BillboardChain.ClearChain");
	//			}
	//			var seg = chainSegmentList[chainIndex];
	//
	//			// Just reset head & tail
	//			seg.tail = seg.head = SEGMENT_EMPTY;
	//
	//			// we removed an entry so indexes need updating
	//			vertexDeclDirty = true;
	//			indexContentDirty = true;
	//			boundsDirty = true;
	//			// tell parent node to update bounds
	//			if (parentNode != null)
	//				parentNode.NeedUpdate();
	//
	//			SetupChainContainers();
	//		}
	//
	//		public void ClearAllChains()
	//		{
	//			for (var i = 0; i < chainCount; ++i)
	//			{
	//				ClearChain(i);
	//			}
	//		}
	//
	//		public ChainSegment GetChainSegment(int pChainIndex)
	//		{
	//			return chainSegmentList[pChainIndex];
	//		}
	//
	//		public List<Element> GetChainElements()
	//		{
	//			return chainElementList;
	//		}
	//	}
}
