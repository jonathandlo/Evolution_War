using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
		[Flags] protected enum TrailState
		{
			Growing = 1,
			Moving = 2,
			Stopping = 4
		}

		public LoopResultStates LoopResultStates { get; private set; }
		protected MovingObject objectToFollow;

		// BillboardChain management.
		public BillboardChain Chain { get; protected set; }
		protected List<BillboardChain.Element> elementList = new List<BillboardChain.Element>();
		protected List<Ray> elementPositions = new List<Ray>();
		protected Vector3 finalPosition;
		protected int elementsToDestroy;

		// Visuals.
		protected TrailState trailState;
		protected Single maxwidth;
		protected ColorEx color;

		public Trail(MovingObject pObjectToFollow, Single pMaxWidth, ColorEx pColor)
		{
			LoopResultStates = new LoopResultStates();

			Chain = new BillboardChain(Methods.GenerateUniqueID.ToString(CultureInfo.InvariantCulture));
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
			trailState = TrailState.Growing;
			elementList.Clear();
			elementPositions.Clear();
			elementsToDestroy = Chain.MaxChainElements = 4;

			AddHead();
		}

		public void Draw(Double pPercent)
		{
			for (var i = 0; i < elementPositions.Count; i ++)
			{
				elementList[i].Position = elementPositions[i].Origin + elementPositions[i].Direction * pPercent;
			}
		}

		public void Loop()
		{
			LoopResultStates.Clear();

			for (var i = 0; i < elementList.Count; i++)
			{
				var element = elementList[i];

				if (i == elementList.Count - elementsToDestroy) // hide the tail elements.
				{
					element.Width = 0;
					element.Color = ColorEx.Black;
				}
				else // decay the visible elements.
				{
					element.Width *= 0.65f;
					element.Color *= 0.6f;
				}
			}

			if (trailState == TrailState.Stopping) // allow decay.
			{
				elementsToDestroy--;

				elementPositions[elementList.Count - 1].Origin = finalPosition;
				elementPositions[elementList.Count - 1].Direction = Vector3.Zero;

				if (elementsToDestroy == 0) // remove completely.
					LoopResultStates.Remove = true;
				else
					RemoveTail();
			}
			else // continue to follow.
			{
				AddHead();
				RemoveTail();
			}
		}

		private void AddHead()
		{
			Vector3 newheadvelocity;

			if (elementList.Count > 0) // set old head element color.
			{
				var oldeadelement = elementList[elementList.Count - 1];
				oldeadelement.Color = color;
				newheadvelocity = objectToFollow.Velocity;
			}
			else
			{
				newheadvelocity = Vector3.Zero;
			}

			// add new white head element.
			var newelement = new BillboardChain.Element();
			newelement.Position = objectToFollow.OldPosition;
			newelement.Width = maxwidth;
			newelement.Color = ColorEx.White;

			Chain.AddChainElement(0, newelement);
			elementList.Add(newelement);
			elementPositions.Add(new Ray(newelement.Position, newheadvelocity));
		}

		private void RemoveTail()
		{
			if (trailState == TrailState.Growing)
			{
				if (elementList.Count == Chain.MaxChainElements) // check if our state is advancing.
				{
					trailState = TrailState.Moving;
				}
			}
			else // trail will exceed max length, record information of new tail.
			{
				elementList.RemoveAt(0);
				elementPositions.RemoveAt(0);
			}
		}

		public void ObjectToFollowDisappeared()
		{
			// stop creating.
			trailState = TrailState.Stopping;
			elementsToDestroy = Math.Min(elementList.Count, Chain.MaxChainElements);
			finalPosition = objectToFollow.OldPosition;

			if (elementList.Count != 0)
			{
				elementList[elementList.Count - 1].Color = color;
			}
		}

		public void Recycle()
		{
			Chain.IsVisible = false;
			elementList.Clear();
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
