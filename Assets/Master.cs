﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour {

	public static Master me;
	public Ore[] ores;
	public List<Drill> drills = new List<Drill>();
	public List<Hull> hulls = new List<Hull>();

	public int[,] shrine;
	public int numShrines;
	public int idolsSent;

	public float fuelRate;
	public float orePackageValue;
	public bool oreInSpace;
	public int packageTimer;
	public int packageTime;

	// Use this for initialization

	void Awake() {
		me = this;
	}

	void Start () {

		addOres();		
		addDrills();
		addHulls();
		makeShrine();

	}
	
	// Update is called once per frame
	void Update () {


		if (packageTime <= packageTimer && oreInSpace) {
			packageTime ++;
		} 
		
		else if (packageTime > packageTimer && oreInSpace) {
			oreInSpace = false;
			Player.me.money += orePackageValue;
			packageTime = 0;
		}
		
	}

	public void sendOresHome(float value, int totalOre) {

		if (!oreInSpace) {
			packageTime = 0;
			oreInSpace = true;
			orePackageValue = value;
			packageTimer = Random.Range(30, 50) * totalOre;
		}
	}

	void makeShrine() {

		shrine = new int[5,5] 
		{ {7, 7, 7, 7, 7},
		  {7, -1, -1, -1, 7},
		  {7, -1, 8, -1, 7},
		  {7, -1, -1, -1, 7},
		  {7, 7, 7, 7, 7}, 
		};	
	}

	void addHulls() {

		hulls.Add(new Hull("hull1", new int[] {0, 3, 0, 0, 0, 0, 0, 0}, 5));
		hulls.Add(new Hull("hull2", new int[] {0, 0, 3, 0, 0, 0, 0, 0}, 10));
		hulls.Add(new Hull("hull3", new int[] {0, 0, 0, 3, 0, 0, 0, 0}, 15));

	}

	void addDrills() {

		drills.Add(new Drill("drill1", new int[] {0, 3, 0, 0, 0, 0, 0, 0}, 18));
		drills.Add(new Drill("drill2", new int[] {0, 0, 3, 0, 0, 0, 0, 0}, 16));
		drills.Add(new Drill("drill3", new int[] {0, 0, 0, 3, 0, 0, 0, 0}, 14));

	}

	void addOres() {

		ores = new Ore[9];
		ores[0] = new Ore("Dirt", 0, 0, rgbToFloat(51f, 51f, 51f));
		ores[1] = new Ore("Copper", .9f, 50f, rgbToFloat(221f, 136f, 85f));
		ores[2] = new Ore("Tin", .8f, 100f, rgbToFloat(0f, 0, 170f));
		ores[3] = new Ore("Iron", .7f, 150f, rgbToFloat(255f, 119f, 119f));
		ores[4] = new Ore("Silver", .6f, 300f, rgbToFloat(170f, 255f, 102f));
		ores[5] = new Ore("Gold", .5f, 350f, rgbToFloat(238f, 238f, 119f));
		ores[6] = new Ore("Platinum", .3f, 400f, rgbToFloat(204f, 68f, 204f));
		ores[7] = new Ore("Diamond", .1f, 450f, rgbToFloat(170f, 255f, 238f));
		ores[8] = new Ore("Idol", .1f, 1000f, rgbToFloat(255f, 0f, 0f));

	}

	public int randomOreIndex(float val) {
		int oreType = (int)(val * (ores.Length-1));
		
 		int randomIndex = Random.Range(0, ores.Length);
		return randomIndex;
	}

	public int posToOre(Vector2 pos) {

		int x = (int)pos.x;
		int y = (int)pos.y;

		int oreType = GroundGenerator.me.tiles[x, y];
		Debug.Log(ores[oreType].name);
		return oreType;

	}

	public void listOres() {

		

	}

	public string getOreName(int index) {

		return ores[index].name;

	}

	public Color rgbToFloat(float rVal, float gVal, float bVal) {

		float r = rVal / 255f;
		float g = gVal / 255f;
		float b = bVal / 255f;
		
		return new Color(r, g, b);
	}
}
