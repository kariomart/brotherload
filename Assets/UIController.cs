using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour {


	public static UIController me;
	public UpgradeUI upgrade;
	public GameObject upgradeUI;
	public GameObject statUI;

	public TextMeshProUGUI moneyUI;
	public TextMeshProUGUI depthUI;
	public TextMeshProUGUI hullUI;
	public TextMeshProUGUI fuelUI;
	public TextMeshProUGUI packageTimeUI;



	// Use this for initialization
	void Start () {

		me = this;
		upgrade = GetComponentInChildren<UpgradeUI>();
		
	}
	
	// Update is called once per frame
	void Update () {

		updateUI();
		
	}


	void updateUI() {

		moneyUI.SetText("MONEY\n" + Player.me.money);
		depthUI.SetText("DEPTH\n" + (int)(Player.me.transform.position.y - (GroundGenerator.me.groundHeight)));
		hullUI.SetText("HULL\n" + Player.me.numInvOres + "/" + Player.me.inventorySize);
		fuelUI.SetText("FUEL\n" + Mathf.Round((Player.me.fuel / Player.me.fueltankSize) * 100) + "%");

		if (Master.me.oreInSpace) {
			packageTimeUI.SetText(Mathf.Round(((float)Master.me.packageTime / (float)Master.me.packageTimer) * 100) + "%");
		} else {
			packageTimeUI.SetText("0%");
		}


	}

	public void toggleUpgradeDisplay() {

		if (upgradeUI.activeInHierarchy) {
			upgradeUI.SetActive(false);
			statUI.SetActive(true);
		} else {
			upgradeUI.SetActive(true);
			statUI.SetActive(false);
		}

	}
}
