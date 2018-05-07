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
		
		float fuel = Mathf.Round((Player.me.fuel / Player.me.fueltankSize) * 100);
		float price = (Player.me.fueltankSize - Player.me.fuel) * Master.me.fuelRate;
		upgradeInfo.SetText("FUEL PERCENTAGE: " + fuel + " %\nFUEL COST: " + price);
		selectedUpgrade = "fuel";

	}

	public void displayDrillUpgradeInfo() {

		Drill drill = Master.me.drills[0];
		string requiredOreNames = "";
		string playerOreNames = "";

		for(int i = 0; i < drill.requiredOre.Length; i++) {
			requiredOreNames += Master.me.getOreName(i) + ": " + drill.requiredOre[i] + "\n";
		}

		for(int i = 0; i < Player.me.inventory.Length; i++) {
			playerOreNames += Master.me.getOreName(i) + ": " + Player.me.inventory[i] + "\n";
		}

		upgradeInfo.SetText
		("DRILL NAME: " + drill.name + 
		"\nDRILL SPEED: " + drill.speed + 
		"\n--------------" + 
		"\nREQUIRED ORE: " + 
		"\n--------------\n" +
		requiredOreNames +
		"\n\nINVENTORY : " + 
		"\n--------------\n" +
		playerOreNames);

		selectedUpgrade = "drill";

	}

	public void displayHullUpgradeInfo() {
		
		Hull hull = Master.me.hulls[0];
		string requiredOreNames = "";
		string playerOreNames = "";

		for(int i = 0; i < hull.requiredOre.Length; i++) {
			requiredOreNames += Master.me.getOreName(i) + ": " + hull.requiredOre[i] + "\n";
		}

		for(int i = 0; i < Player.me.inventory.Length; i++) {
			playerOreNames += Master.me.getOreName(i) + ": " + Player.me.inventory[i] + "\n";
		}

		upgradeInfo.SetText
		("HULL NAME: " + hull.name + 
		"\nHULL ARMOR: " + hull.armor + 
		"\n--------------" + 
		"\nREQUIRED ORE: " + 
		"\n--------------\n" +
		requiredOreNames +
		"\n\nINVENTORY : " + 
		"\n--------------\n" +
		playerOreNames);

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

			if (Player.me.numInvOres == 0) { 
				upgradeInfo.SetText("inventory is empty!");
				return; 
			}

			for(int i = 0; i < Player.me.inventory.Length - 1; i++) {
				
				Debug.Log(Master.me.getOreName(i));
				float oreValue = Master.me.ores[i].value;
				oreValue *= Player.me.inventory[i];
				totalOreValue += oreValue;
				totalOre += Player.me.inventory[i];
				oreNames += Master.me.getOreName(i) +": " + Player.me.inventory[i] + "\n";
				Player.me.inventory[i] = 0;
			}
			
			//Player.me.inventory.Clear();
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

		displayDrillUpgradeInfo();
		Drill drill = Master.me.drills[0];
		string missingText = "";

			for(int i = 0; i < drill.requiredOre.Length - 1; i++) {
				
				if (Player.me.inventory[i] >= drill.requiredOre[i]) {
					continue;
				} else {
					missingText = "\nmissing 1 " + Master.me.getOreName(i);
					upgradeInfo.SetText(upgradeInfo.text + missingText);
					return;
				}
			}
		
			Debug.Log("cha ching!");
			
			for(int i = 0; i < drill.requiredOre.Length - 1; i++) {
				Player.me.inventory[i] -= drill.requiredOre[i];
			}


		Player.me.digSpeed = drill.speed;
		Master.me.drills.Remove(drill);	
		displayDrillUpgradeInfo();
	}

	public void buyHull() {

		Hull hull = Master.me.hulls[0];

		for(int i = 0; i < hull.requiredOre.Length - 1; i++) {
			
			if (Player.me.inventory[i] >= hull.requiredOre[i]) {
				continue;
			} else {
				string text = upgradeInfo.text + "\n\n\nmissing 1 " + Master.me.getOreName(i);
				upgradeInfo.SetText(text);
				return;
			}
		}
	
		Debug.Log("cha ching!");
		
		for(int i = 0; i < hull.requiredOre.Length - 1; i++) {
			Player.me.inventory[i] -= hull.requiredOre[i];
		}


		Player.me.armor = hull.armor;
		Master.me.hulls.Remove(hull);
		displayHullUpgradeInfo();	
	 }


		

}
