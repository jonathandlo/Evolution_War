using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiom.Collections;
using Axiom.Core;
using Axiom.Fonts;
using Axiom.Overlays;
using Axiom.Overlays.Elements;

namespace Evolution_War
{
	public class HUDManager
	{
		private UpgradeHUD upgradeHUD;

		public HUDManager(World pWorld, Ship pPlayerShip)
		{
			var overlay = OverlayManager.Instance.Create("Overlay");

			// Create HUD elements.
			upgradeHUD = new UpgradeHUD(pWorld, pPlayerShip, overlay);

			PopulateHUDPanels();
		}

		public void PopulateHUDPanels()
		{
			upgradeHUD.PopulateUpgradePanel();
		}

		public void PressUpgrade()
		{
			upgradeHUD.PressUpgrade();	
		}

		public void AddPoints()
		{
			upgradeHUD.AddPoints();
		}

		public void Draw(Double pPercent)
		{
			upgradeHUD.Draw(pPercent);
		}

		public void Loop()
		{
			upgradeHUD.Loop();
		}
	}

	public class UpgradeHUD
	{
		// MainHUD variables.
		private World world;
		private Ship playerShip;
		private Overlay overlay;

		// UpgradeHUD panels.
		private Panel masterUpgradePanel;
		private Dictionary<Int32, List<Upgrade>> upgradesByCost; // Used for displaying indicator dots.
		private Dictionary<Int32, List<BorderPanel>> upgradePanelsByCost; // Used for toggling colors.
		private Dictionary<Int32, BorderPanel> costSelectorPanels; // Used for toggling colors.

		// Constants.
		private readonly ColorEx white = new ColorEx(0.8f, 1f, 1f, 1f);
		private readonly ColorEx yellow = new ColorEx(0.8f, 1f, 1f, 0f);
		private const String whiteMaterialName = "White80";
		private const String yellowMaterialName = "Yellow80";

		// State Variables.
		private const Int32 rotationDelay = 30;
		private Int64 lastRotationFrame = 0;
		private Int32 selectedUpgrade;
		private Int32 selectedCost;
		private Boolean inSelectionMode;

		public UpgradeHUD(World pWorld, Ship pPlayerShip, Overlay pOverlay)
		{
			// Initialize local variables.
			world = pWorld;
			playerShip = pPlayerShip;
			overlay = pOverlay;
			inSelectionMode = false;
			selectedUpgrade = 0;
			upgradesByCost = new Dictionary<int, List<Upgrade>>();
			upgradePanelsByCost = new Dictionary<int, List<BorderPanel>>();
			costSelectorPanels = new Dictionary<int, BorderPanel>();

			// Create the upgrade panel.
			masterUpgradePanel = new PanelFactory().Create("Upgrade Panel") as Panel;
		}

		public void PopulateUpgradePanel()
		{
			var upgradeCounter = 0;
			overlay.Hide();

			// (Re)create cost-sorted collections.
			upgradePanelsByCost = new Dictionary<int, List<BorderPanel>>();
			upgradesByCost = playerShip.Upgrades.GetUpgradesByCost();
			foreach (var cost in upgradesByCost.Keys)
				upgradePanelsByCost.Add(cost, new List<BorderPanel>());

			// Clear upgrades.
			masterUpgradePanel.Children.Clear();
			costSelectorPanels.Clear();

			// UpgradeHUD panel.
			masterUpgradePanel.MetricsMode = MetricsMode.Relative;
			masterUpgradePanel.VerticalAlignment = VerticalAlignment.Bottom;
			masterUpgradePanel.HorizontalAlignment = HorizontalAlignment.Center;
			masterUpgradePanel.Top = -0.09f;
			masterUpgradePanel.Left = -0.5f;
			masterUpgradePanel.Width = 1.0f;
			masterUpgradePanel.Height = 0.09f;
			overlay.AddElement(masterUpgradePanel);

			// Cost indicators.
			foreach (var list in upgradesByCost)
			{
				var indicatorPanel = new BorderPanelFactory().Create("Cost Indicator " + list.Key) as BorderPanel;
				if (indicatorPanel == null) return;
				indicatorPanel.MetricsMode = MetricsMode.Relative;
				indicatorPanel.BorderMaterialName = whiteMaterialName;
				indicatorPanel.SetBorderSize(0.0005f, 0.0005f, 0.0005f, 0f);
				indicatorPanel.Top = 0f;
				indicatorPanel.Left = masterUpgradePanel.Width * upgradeCounter / Upgrades.NumberOfUpgrades + 0.006f;
				indicatorPanel.Width = masterUpgradePanel.Width * list.Value.Count / Upgrades.NumberOfUpgrades - 0.012f;
				indicatorPanel.Height = 0.015f;
				indicatorPanel.IsVisible = false;
				masterUpgradePanel.AddChild(indicatorPanel);
				costSelectorPanels.Add(list.Key, indicatorPanel);

				var indicatorLabel = new TextAreaFactory().Create(list.Key + " Indicator Label") as TextArea;
				if (indicatorLabel == null) return;
				indicatorLabel.MetricsMode = MetricsMode.Pixels;
				indicatorLabel.FontName = "Candara";
				indicatorLabel.Text = list.Key.ToString();
				indicatorLabel.TextAlign = HorizontalAlignment.Center;
				indicatorLabel.HorizontalAlignment = HorizontalAlignment.Center;
				indicatorLabel.VerticalAlignment = VerticalAlignment.Top;
				indicatorLabel.CharHeight = 36;
				indicatorLabel.Top = -28.0f;
				indicatorLabel.Left = 0.0f;
				indicatorLabel.Width = indicatorPanel.Width;
				indicatorLabel.Height = indicatorPanel.Height;
				indicatorLabel.Color = white;
				indicatorLabel.IsVisible = false;
				indicatorPanel.AddChild(indicatorLabel);

				// Upgrades.
				for (var i = 0; i < list.Value.Count; i++)
				{
					var panel = new BorderPanelFactory().Create("Upgrade " + list.Value[i].Name) as BorderPanel;
					if (panel == null) return;
					panel.MetricsMode = MetricsMode.Relative;
					panel.BorderMaterialName = whiteMaterialName;
					panel.SetBorderSize(0.0005f, 0.0005f, 0.0005f, 0.004f);
					panel.Top = 0.02f;
					panel.Left = masterUpgradePanel.Width * upgradeCounter / Upgrades.NumberOfUpgrades + 0.002f;
					panel.Width = masterUpgradePanel.Width / Upgrades.NumberOfUpgrades - 0.004f;
					panel.Height = masterUpgradePanel.Height - 0.04f;
					masterUpgradePanel.AddChild(panel);
					upgradePanelsByCost[list.Key].Add(panel);

					var text = new TextAreaFactory().Create(list.Value[i].Name + " Text") as TextArea;
					if (text == null) return;
					text.MetricsMode = MetricsMode.Pixels;
					text.FontName = "Candara";
					text.Text = list.Value[i].Name;
					text.TextAlign = HorizontalAlignment.Center;
					text.HorizontalAlignment = HorizontalAlignment.Center;
					text.VerticalAlignment = VerticalAlignment.Bottom;
					text.CharHeight = 28;
					text.Top = -28.0f;
					text.Left = 0.0f;
					text.Width = panel.Width;
					text.Height = panel.Height;
					text.Color = white;
					panel.AddChild(text);

					upgradeCounter++;
				}
			}

			SetAvailableUpgradePoints(playerShip.AvailableUpgradePoints);
			overlay.Show();
		}

		public void SetAvailableUpgradePoints(Int32 pUpgradePoints)
		{
			if (inSelectionMode)
				StopRotation();

			// Find the largest upgrade group that the player can afford.
			var maxCost = 0;
			foreach (var key in upgradesByCost.Keys)
				if (key <= pUpgradePoints) maxCost = Math.Max(key, maxCost);

			// Highlight the selected cost group.
			if (selectedCost > 0) ClearHighlight(selectedCost);
			if (maxCost > 0) Highlight(maxCost);

			// Update the selected cost variable.
			selectedCost = maxCost;
		}

		public void PressUpgrade()
		{
			if (inSelectionMode)
			{
				playerShip.AvailableUpgradePoints -= selectedCost;
				SetAvailableUpgradePoints(playerShip.AvailableUpgradePoints);
			}
			else if (selectedCost > 0)
				StartRotation();
		}

		public void AddPoints() // temporary.
		{
			if (playerShip.AvailableUpgradePoints >= 6) playerShip.AvailableUpgradePoints = 0;
			else playerShip.AvailableUpgradePoints++;

			SetAvailableUpgradePoints(playerShip.AvailableUpgradePoints);
		}

		public void Draw(Double pPercent)
		{

		}

		public void Loop()
		{
			if (!inSelectionMode || (world.FrameCount - lastRotationFrame <= rotationDelay)) return; // Check if it's time to switch highlighted upgrade.

			if (selectedUpgrade < upgradesByCost[selectedCost].Count - 1)
			{
				// Switch highlighted upgrade.
				ClearHighlight(selectedCost, selectedUpgrade);
				selectedUpgrade++;
				Highlight(selectedCost, selectedUpgrade);
			}
			else
			{
				StopRotation();
			}

			lastRotationFrame += rotationDelay;
		}

		private void StartRotation()
		{
			inSelectionMode = true;
			selectedUpgrade = 0;
			lastRotationFrame = world.FrameCount;

			// Update visuals.
			costSelectorPanels[selectedCost].SetBorderSize(0.0005f, 0.0005f, 0.004f, 0f);
			costSelectorPanels[selectedCost].BorderMaterialName = yellowMaterialName;
			costSelectorPanels[selectedCost].Children.First().Value.Color = yellow;
			Highlight(selectedCost, 0);
		}

		private void StopRotation()
		{
			inSelectionMode = false;

			// Update visuals.
			costSelectorPanels[selectedCost].SetBorderSize(0.0005f, 0.0005f, 0.0005f, 0f);
			costSelectorPanels[selectedCost].BorderMaterialName = whiteMaterialName;
			costSelectorPanels[selectedCost].Children.First().Value.Color = white;
			ClearHighlight(selectedCost, selectedUpgrade);
		}

		private void Highlight(Int32 pCost)
		{
			costSelectorPanels[pCost].IsVisible = true;
			costSelectorPanels[pCost].Children.First().Value.IsVisible = true;
		}

		private void Highlight(Int32 pCost, Int32 pUpgrade)
		{
			upgradePanelsByCost[pCost][pUpgrade].BorderMaterialName = yellowMaterialName;
			upgradePanelsByCost[pCost][pUpgrade].Children.First().Value.Color = yellow;
		}

		private void ClearHighlight(Int32 pCost)
		{
			costSelectorPanels[pCost].IsVisible = false;
			costSelectorPanels[pCost].Children.First().Value.IsVisible = true;

			foreach (var upgradePanel in upgradePanelsByCost[pCost])
			{
				upgradePanel.BorderMaterialName = whiteMaterialName;
				upgradePanel.Children.First().Value.Color = white;
			}
		}

		private void ClearHighlight(Int32 pCost, Int32 pUpgrade)
		{
			upgradePanelsByCost[pCost][pUpgrade].BorderMaterialName = whiteMaterialName;
			upgradePanelsByCost[pCost][pUpgrade].Children.First().Value.Color = white;
		}
	}
}
