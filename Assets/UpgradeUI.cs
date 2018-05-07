using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeUI : MonoBehaviour {

	public static UpgradeUI me;
	public TextMeshProUGUI upgradeInfo;
	public string selectedUpgrade;

	// Use this for initialization

	void Awake() {

		me = this;

	}
	
	// Update is called once per frame
	void Update () {

		if (selectedUpgrade == "fuel") {
			displayFuelInfo();
		}
		
	}

	public void displayInventory() {

		string playerOreNames = "";

		for(int i = 0; i < Player.me.inventory.Length; i++) {
			playerOreNames += Master.me.getOreName(i) + ": " + Player.me.inventory[i] + "\n";
		}

		upgradeInfo.SetText("INVENTORY : " + 
		"\n--------------\n" +
		playerOreNames + "\nMONEY: " + Player.me.money);


	}


	public void displayFuelInfo() {
		
		if (!SoundController.me.checkIfPlaying(SoundController.me.shopButtons)) {
			SoundController.me.PlaySound(SoundController.me.shopButtons);
		}
		float fuel = Mathf.Round((Player.me.fuel / Player.me.fueltankSize) * 100);
		float price = (Player.me.fueltankSize - Player.me.fuel) * Master.me.fuelRate;
		upgradeInfo.SetText("FUEL PERCENTAGE: " + fuel + " %\nFUEL COST: " + price + "\n\nMONEY: " + Player.me.money);
		selectedUpgrade = "fuel";

	}

	public void displayDrillUpgradeInfo() {

		SoundController.me.PlaySound(SoundController.me.shopButtons);
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

		public void displayEngineUpgradeInfo() {

		SoundController.me.PlaySound(SoundController.me.shopButtons);
		Engine engine = Master.me.engines[0];
		string requiredOreNames = "";
		string playerOreNames = "";

		for(int i = 0; i < engine.requiredOre.Length; i++) {
			requiredOreNames += Master.me.getOreName(i) + ": " + engine.requiredOre[i] + "\n";
		}

		for(int i = 0; i < Player.me.inventory.Length; i++) {
			playerOreNames += Master.me.getOreName(i) + ": " + Player.me.inventory[i] + "\n";
		}

		upgradeInfo.SetText
		("ENGINE NAME: " + engine.name + 
		"\nENGINE SPEED: " + engine.speed + 
		"\nENGINE FUEL RATE: " + engine.fuelRate + 
		"\n--------------" + 
		"\nREQUIRED ORE: " + 
		"\n--------------\n" +
		requiredOreNames +
		"\n\nINVENTORY : " + 
		"\n--------------\n" +
		playerOreNames);

		selectedUpgrade = "engine";

	}

	public void displayHullUpgradeInfo() {
		
		SoundController.me.PlaySound(SoundController.me.shopButtons);
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
		"\nHULL SIZE: " + hull.size + 
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

		if (selectedUpgrade == "engine") {

			buyEngine();	
			
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
				SoundController.me.PlaySound(SoundController.me.shopError);
				return; 
			}

			if (Master.me.oreInSpace) { 
				upgradeInfo.SetText("there are already ores in space!");
				SoundController.me.PlaySound(SoundController.me.shopError);
				return; 
			}

			for(int i = 0; i < Player.me.inventory.Length; i++) {

				if (i == 8 && Player.me.inventory[i] > 0) {
					Master.me.idolsSent = Player.me.inventory[i];
				}
				
//				Debug.Log(Master.me.getOreName(i));
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
			SoundController.me.PlaySound(SoundController.me.sentOres);
		}

	}

	public void buyFuel() {

		float price = (Player.me.fueltankSize - Player.me.fuel) * Master.me.fuelRate;

		if (Player.me.money > price) {
			Player.me.money -= price;
			Player.me.fuel = Player.me.fueltankSize;
			SoundController.me.PlaySound(SoundController.me.boughtItem);
		} else {
			SoundController.me.PlaySound(SoundController.me.shopError);
		}

	}

	public void buyEngine() {

		displayEngineUpgradeInfo();
		Engine engine = Master.me.engines[0];
		string missingText = "";

			for(int i = 0; i < engine.requiredOre.Length - 1; i++) {
				
				if (Player.me.inventory[i] >= engine.requiredOre[i]) {
					continue;
				} else {
					missingText = "\nmissing 1 " + Master.me.getOreName(i);
					upgradeInfo.SetText(upgradeInfo.text + missingText);
					SoundController.me.PlaySound(SoundController.me.shopError);
					return;
				}
			}
		
			//Debug.Log("cha ching!");
			SoundController.me.PlaySound(SoundController.me.boughtItem);
			for(int i = 0; i < engine.requiredOre.Length - 1; i++) {
				Player.me.inventory[i] -= engine.requiredOre[i];
			}


		Player.me.updateStats();
		Master.me.engines.Remove(engine);	
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
					SoundController.me.PlaySound(SoundController.me.shopError);
					return;
				}
			}
		
			Debug.Log("cha ching!");
			SoundController.me.PlaySound(SoundController.me.boughtItem);
			for(int i = 0; i < drill.requiredOre.Length - 1; i++) {
				Player.me.inventory[i] -= drill.requiredOre[i];
			}


		Player.me.updateStats();
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
				SoundController.me.PlaySound(SoundController.me.shopError);
				return;
			}
		}
	
		Debug.Log("cha ching!");
		SoundController.me.PlaySound(SoundController.me.boughtItem);
		for(int i = 0; i < hull.requiredOre.Length - 1; i++) {
			Player.me.inventory[i] -= hull.requiredOre[i];
		}


		Player.me.updateStats();
		Master.me.hulls.Remove(hull);
		displayHullUpgradeInfo();	
	 }


		

}
