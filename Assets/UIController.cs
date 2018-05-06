using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour {


	public static UIController me;
	public GameObject upgradeUI;
	public GameObject statUI;

	public TextMeshProUGUI moneyUI;
	public TextMeshProUGUI depthUI;
	public TextMeshProUGUI hullUI;
	public TextMeshProUGUI fuelUI;



	// Use this for initialization
	void Start () {

		me = this;
		
	}
	
	// Update is called once per frame
	void Update () {

		updateUI();
		
	}

	void updateUI() {

		moneyUI.SetText("MONEY\n" + Player.me.money);
		depthUI.SetText("DEPTH\n" + (int)(Player.me.transform.position.y - (GroundGenerator.me.groundHeight)));
		hullUI.SetText("HULL\n" + Player.me.armor);
		fuelUI.SetText("FUEL\n" + Mathf.Round((Player.me.fuel / Player.me.fueltankSize) * 100) + "%");

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
