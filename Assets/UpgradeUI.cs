using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeUI : MonoBehaviour {

	public TextMeshProUGUI upgradeInfo;
	public string selectedUpgrade;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (selectedUpgrade == "fuel") {
			displayFuelInfo();
		}
		
	}


	public void displayFuelInfo() {
		
		float fuel = Player.me.fuel / Player.me.fueltankSize;
		float price = (Player.me.fueltankSize - Player.me.fuel) * Master.me.fuelRate;
		upgradeInfo.SetText("FUEL PERCENTAGE: " + fuel + " %\nFUEL COST: " + price);
		selectedUpgrade = "fuel";

	}

	public void displayDrillUpgradeInfo() {

		Drill drill = Master.me.drills[0];
		string oreNames = "";
		foreach(int i in drill.requiredOre) {
			oreNames += Master.me.getOreName(i) + "\n";
		}

		upgradeInfo.SetText("DRILL NAME: " + drill.name + "\nDRILL SPEED: " + drill.speed + "\nREQUIRED ORE: \n" + oreNames);
		selectedUpgrade = "drill";

	}

	public void displayHullUpgradeInfo() {
		
		Hull hull = Master.me.hulls[0];
		string oreNames = "";
		foreach(int i in hull.requiredOre) {
			oreNames += Master.me.getOreName(i) + "\n";
		}

		upgradeInfo.SetText("HULL NAME: " + hull.name + "\nHULL ARMOR: " + hull.armor + "\nREQUIRED ORE: \n" + oreNames);

		selectedUpgrade = "hull";

	}

	public void buy() {

		if (selectedUpgrade == "fuel") {

			buyFuel();
		}

		if (selectedUpgrade == "drill") {

			buyDrill();	
		}

		if (selectedUpgrade == "hull") {

			buyHull();	
		}
	}

	public void sendOres() {

		if (!Master.me.oreInSpace) {
			float totalOreValue = 0;
			int totalOre = 0;
			string oreNames = "";

			if (Player.me.inventory.Count == 0) { return; }

			foreach(int i in Player.me.inventory) {
				totalOreValue += Master.me.ores[i].value;
				totalOre ++;
				oreNames += Master.me.getOreName(i) + "\n";
			}
			
			Player.me.inventory.Clear();
			upgradeInfo.SetText("ORES SENT HOME:\n" + oreNames + "\nTOTAL VALUE: " + totalOreValue);
			Master.me.sendOresHome(totalOreValue, totalOre);
		}

	}

	public void buyFuel() {

		float price = (Player.me.fueltankSize - Player.me.fuel) * Master.me.fuelRate;

		if (Player.me.money > price) {
			Player.me.money -= price;
			Player.me.fuel = Player.me.fueltankSize;
		}

	}

	public void buyDrill() {

		Drill drill = Master.me.drills[0];

			foreach(int ore in drill.requiredOre) {

				if (Player.me.inventory.Contains(ore)) {
					continue;
				} else {
					string text = upgradeInfo.text + "\n\n\nmissing 1 " + Master.me.getOreName(ore);
					upgradeInfo.SetText(text);
					return;
				}
			}
		
			Debug.Log("cha ching!");
			foreach(int ore in drill.requiredOre) {

				if (Player.me.inventory.Contains(ore)) {
					Player.me.inventory.Remove(ore);
				}

			}

			Player.me.digSpeed = drill.speed;
			Master.me.drills.Remove(drill);	
	}

	public void buyHull() {

		Hull hull = Master.me.hulls[0];

			foreach(int ore in hull.requiredOre) {

				if (Player.me.inventory.Contains(ore)) {
					continue;
				} else {
					string text = upgradeInfo.text + "\n\n\nmissing 1 " + Master.me.getOreName(ore);
					upgradeInfo.SetText(text);
					return;
				}
			}
		
			Debug.Log("cha ching!");
			foreach(int ore in hull.requiredOre) {

				if (Player.me.inventory.Contains(ore)) {
					Player.me.inventory.Remove(ore);
				}

			}

			Player.me.armor = hull.armor;
			Master.me.hulls.Remove(hull);	
	}


		

}
