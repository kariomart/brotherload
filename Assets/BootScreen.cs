using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BootScreen : MonoBehaviour {

	public bool ran;

	public Image image;
	public TMP_InputField input;
	public TextMeshProUGUI bootText;
	public GameObject startMenu;

	public int nextLineTime;
	public int lineTimer;
	public int lineNum;
	public string[] lines;

	int scrollCount;
	public int textScrollTime;


	// Use this for initialization
	void Start () {

		image = GetComponent<Image>();
		input.Select();
		lines = new string[6];
		setLines();
		startMenu = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {

		if (ran) {
			runGame();
		}




	}

	public void endEdit() {

		Debug.Log(input.text);
		string text = input.text.ToLower();
		if (text.Contains("run motherload")) {
			Debug.Log("run the game!");
			ran = true;
		} else {
			input.text = "";
			input.Select();
			input.ActivateInputField();

		}

	}

	public void runGame() {
		lineTimer ++;
		scrollCount ++;

		if (lineTimer >= nextLineTime) {
			bootText.SetText(bootText.text + "\n" + lines[lineNum%lines.Length]);
			lineNum ++;
			lineTimer = 0;
		} 

		if (scrollCount >= textScrollTime) {
			ran = false;
			GroundGenerator.me.setPlayerPos();
			startMenu.SetActive(false);
		}




	}

	public void setLines() {

		lines[0] = "LOADING MOTHERLOAD...";
		lines[1] = "MARTIN NAYERI PROC GEN 2018";
		lines[2] = "DDIHQD@$!@#J!@L23123123";
		lines[3] = "TY TO AP/MILAN/ROWAN FOR HELP";
		lines[4] = "23DJDO!@3123123JODJDJDOQJD";
		lines[5] = "HELPD!@#12321ME!@#!E!E";

	}

	
}
