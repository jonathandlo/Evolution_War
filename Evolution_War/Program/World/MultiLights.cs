using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiom.Core;
using Axiom.Graphics;
using Axiom.Math;

namespace Evolution_War
{
	public class MultiLights
	{
		public ColorEx PlayerLightColor { get; set; }
		public ColorEx CamLightColor { get; set; }
		private ColorEx oldCamLightColor;

		private Light playerLight;
		private List<Light> camLights;
		private SceneNode playerLightNode;
		private SceneNode camInnerLightNode;
		private SceneNode camOuterLightNode;

		private Double baseCamLightAngle;
		private Int32 outerLights;
		private Int32 innerLights;

		public MultiLights(SceneManager pSceneManager, SceneNode pCamNode, MovingObject pPlayerShip, Int32 pNumberOfLights)
		{
			oldCamLightColor = CamLightColor = new ColorEx(0.13f, 0.1f, 0.05f);
			PlayerLightColor = ColorEx.White;
			camLights = new List<Light>(pNumberOfLights);

			innerLights = (Int32)Math.Round(pNumberOfLights / 3.0f, MidpointRounding.AwayFromZero);
			outerLights = pNumberOfLights - innerLights;

			// create the playership's light.
			playerLight = pSceneManager.CreateLight("playerSpotLight");
			playerLight.Type = LightType.Spotlight;
			playerLight.Diffuse = PlayerLightColor;
			playerLight.Specular = ColorEx.White;
			playerLight.SetSpotlightRange(0.0f, 120.0f);
			playerLight.Direction = Vector3.NegativeUnitZ;

			playerLightNode = pPlayerShip.Node.CreateChildSceneNode();
			playerLightNode.AttachObject(playerLight);
			playerLightNode.Position = new Vector3(0, 0, 0);
			playerLightNode.SetDirection(new Vector3(1, 0, 0), TransformSpace.Local);

			// create the camera spotlights around the camera's direction.
			camInnerLightNode = pCamNode.CreateChildSceneNode();
			camInnerLightNode.Position = new Vector3(0, 0, 0);
			camOuterLightNode = pCamNode.CreateChildSceneNode();
			camOuterLightNode.Position = new Vector3(0, 0, 0);

			for (var i = 0; i < innerLights; i++)
			{
				var light = pSceneManager.CreateLight("camInnerLight " + (i + 1));
				light.Type = LightType.Spotlight;
				light.Diffuse = CamLightColor;
				light.Specular = ColorEx.White;
				light.SetSpotlightRange(0.0f, 25.0f);
				light.Direction = Quaternion.FromAngleAxis(360.0 * i / innerLights * Constants.DegreesToRadians, Vector3.UnitZ) *
					Quaternion.FromAngleAxis(10.0 * Constants.DegreesToRadians, Vector3.UnitX) *
					Vector3.NegativeUnitZ;

				camLights.Add(light);
				camInnerLightNode.AttachObject(light);
			}
			for (var i = 0; i < outerLights; i++)
			{
				var light = pSceneManager.CreateLight("camOuterLight " + (i + 1));
				light.Type = LightType.Spotlight;
				light.Diffuse = CamLightColor;
				light.Specular = ColorEx.White;
				light.SetSpotlightRange(0.0f, 25.0f);
				light.Direction = Quaternion.FromAngleAxis(360.0 * i / outerLights * Constants.DegreesToRadians, Vector3.UnitZ) *
					Quaternion.FromAngleAxis(20.0 * Constants.DegreesToRadians, Vector3.UnitX) *
					Vector3.NegativeUnitZ;

				camLights.Add(light);
				camOuterLightNode.AttachObject(light);
			}
		}

		public void Loop()
		{
			baseCamLightAngle += 0.4 - (baseCamLightAngle > 360 ? 360 : 0);
			playerLight.Diffuse = PlayerLightColor;

			foreach (var camLight in camLights)
			{
				camLight.Diffuse = CamLightColor;
			}
		}

		public void Draw(Double pPercent)
		{
			// rotate the camera spotlights around the camera's direction.
			camInnerLightNode.Orientation = Quaternion.FromAngleAxis((baseCamLightAngle + 0.4 * pPercent) * Constants.DegreesToRadians, Vector3.UnitZ);
			camOuterLightNode.Orientation = Quaternion.FromAngleAxis((baseCamLightAngle + 0.4 * pPercent) * -Constants.DegreesToRadians, Vector3.UnitZ);
		}
	}
}
