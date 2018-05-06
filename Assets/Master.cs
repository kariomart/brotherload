using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour {

	public static Master me;
	public Ore[] ores;
	public List<Drill> drills = new List<Drill>();
	public List<Hull> hulls = new List<Hull>();

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

	void addHulls() {

		hulls.Add(new Hull("hull1", new int[] {1, 1, 1}, 18));
		hulls.Add(new Hull("hull2", new int[] {2, 2, 2}, 16));
		hulls.Add(new Hull("hull3", new int[] {2, 2, 2}, 14));

	}

	void addDrills() {

		drills.Add(new Drill("drill1", new int[] {1, 1, 1}, 18));
		drills.Add(new Drill("drill2", new int[] {2, 2, 2}, 16));
		drills.Add(new Drill("drill3", new int[] {2, 2, 2}, 14));

	}

	void addOres() {

		ores = new Ore[8];
		ores[0] = new Ore("Dirt", 0, 0, new Color(0.4f, 0.2f, 0.1f));
		ores[1] = new Ore("Copper", .9f, 50f, new Color(1f, 0.5f, .3f));
		ores[2] = new Ore("Tin", .8f, 100f, new Color(.2f, .3f, .8f));
		ores[3] = new Ore("Iron", .7f, 150f, Color.white);
		ores[4] = new Ore("Silver", .6f, 300f, Color.grey);
		ores[5] = new Ore("Gold", .5f, 350f, new Color(1f, .9f, 0f));
		ores[6] = new Ore("Platinum", .3f, 400f, new Color(.9f, .9f, .88f));
		ores[7] = new Ore("Diamond", .1f, 450f, new Color(.72f, .94f, 1f));

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

	public string getOreName(int index) {

		return ores[index].name;

	}
}
