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

		// variables to manage the billboardchain
		public BillboardChain Chain { get; protected set; }
		protected int count, deadcountdown;
		protected bool stopping;

		// visual parameters
		protected Single maxwidth;

		public Trail(MovingObject pObjectToFollow, Single pMaxWidth)
		{
			LoopResultStates = new LoopResultStates();

			Chain = new BillboardChain(Methods.GenerateUniqueID.ToString());
			Chain.Material.SetSceneBlending(SceneBlendType.Replace);
			Chain.NumberOfChains = 1;
			Chain.IsVisible = false;
			World.Instance.SceneManager.RootSceneNode.AttachObject(Chain);

			Relaunch(pObjectToFollow, pMaxWidth);
		}

		public void Relaunch(MovingObject pObjectToFollow, Single pMaxWidth)
		{
			objectToFollow = pObjectToFollow;
			maxwidth = pMaxWidth;
			stopping = false;
			count = 0;
			deadcountdown = Chain.MaxChainElements = 6;

			DropTrail();
		}

		public void Draw(Double pPercent)
		{
			if (!stopping)
			{
				var oldelement = Chain.GetChainElement(0, 0);
				oldelement.Position = objectToFollow.Node.Position;
				oldelement.Width = (float)pPercent * maxwidth;
			}
		}

		public void Loop()
		{
			LoopResultStates.Clear();

			for (var i = 0; i < count; i++)
			{
				var element = Chain.GetChainElement(0, i);

				element.Width *= 0.8f;
				element.Color *= 0.8f;
			}

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
		}

		private void DropTrail()
		{
			var oldelement = Chain.GetChainElement(0, 0);
			oldelement.Position = objectToFollow.OldPosition;
			oldelement.Color = new ColorEx((Single)Methods.Random.NextDouble(), (Single)Methods.Random.NextDouble(), (Single)Methods.Random.NextDouble());
			oldelement.Width = maxwidth;
			
			Chain.AddChainElement(0, new BillboardChain.Element(objectToFollow.OldPosition, 0, 1, new ColorEx(1.0f, 1.0f, 1.0f)));
			count++;
		}

		public void ObjectToFollowDisappeared()
		{
			// stop creating.
			stopping = true;
			deadcountdown = Math.Min(count, Chain.MaxChainElements);
		}

		public void Recycle()
		{
			Chain.IsVisible = false;
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
