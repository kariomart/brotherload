using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {


	public static UIController me;
	public GameObject upgradeUI;



	// Use this for initialization
	void Start () {

		me = this;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void toggleUpgradeDisplay() {

		if (upgradeUI.activeInHierarchy) {
			upgradeUI.SetActive(false);
		} else {
			upgradeUI.SetActive(true);
		}

	}
}
