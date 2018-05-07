using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour {

	public static Master me;
	public Ore[] ores;
	public List<Drill> drills = new List<Drill>();
	public List<Hull> hulls = new List<Hull>();
	public List<Engine> engines = new List<Engine>();
	public GameObject planet;

	public int minPlanets;
	public int maxPlanets;
	public float minDistance;
	public float maxDistance;

	public int[,] shrine;
	public int numShrines;
	public int idolsSent;

	public float fuelRate;
	public float orePackageValue;
	public bool oreInSpace;
	public int packageTimer;
	public int packageTime;

	bool gameover;

	// Use this for initialization

	void Awake() {
		me = this;
		addOres();		
		addDrills();
		addEngines();
		addHulls();
		makeShrine();

	}

	void Start () {


	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Space)) {
			//spawnPlanets();
		}

		if (Input.GetKeyDown(KeyCode.Alpha8)){
			Player.me.godMode();
		}
		if (packageTime <= packageTimer && oreInSpace) {
			packageTime ++;
		} 
		
		else if (packageTime > packageTimer && oreInSpace) {
			oreInSpace = false;
			Player.me.money += orePackageValue;
			packageTime = 0;

			if (idolsSent >= numShrines) {
			gameover = true;
		}
		}

		if (gameover) {
			GroundGenerator.me.setPlayerPos();
			UIController.me.gameObject.SetActive(false);
			Player.me.wonGame = true;
			spawnPlanets();
			gameover = false;
		}
		
	}

	void spawnPlanets() {

		int numPlanets = Random.Range(minPlanets, maxPlanets);
		Vector2 basePos = GroundGenerator.me.transform.position;

		for (int i = 0; i < numPlanets; i++) {
			float rand1 = Random.Range(minDistance, maxDistance);
			if (Random.value > 0.5f) { rand1 *= -1;}
			float rand2 = Random.Range(minDistance, maxDistance);
			if (Random.value > 0.5f) { rand1 *= -1;}

			Vector2 randomPos = new Vector2(rand1, rand2); 

			GameObject tempPlanet = Instantiate(planet, new Vector3(basePos.x + randomPos.x, basePos.x + randomPos.y, 0), Quaternion.identity);
			PlanetGenerator tempGen = tempPlanet.GetComponent<PlanetGenerator>();
			tempGen.groundHeight = Random.Range(15, 30);
			tempGen.groundWidth = Random.Range(15, 30);
			//tempGen.frequency = Random.Range(0.7f, 1f);
			tempGen.spawnPlanet();

		}

	}

	public void sendOresHome(float value, int totalOre) {

		if (!oreInSpace) {
			packageTime = 0;
			oreInSpace = true;
			orePackageValue = value;
			packageTimer = Random.Range(50, 150) * totalOre;
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

	void addEngines() {

		engines.Add(new Engine("Scrap Engine", new int[] {0, 3, 3, 3, 0, 0, 0, 0}, 6, .8f, 3000f));
		engines.Add(new Engine("24Karat Vault", new int[] {0, 0, 0, 0, 0, 30, 0, 0}, 7, .6f, 5000f));
		engines.Add(new Engine("Silvium Engine", new int[] {0, 0, 0, 0, 20, 0, 10, 0}, 8, .3f, 8000f));
		engines.Add(new Engine("Diamond Engine", new int[] {0, 0, 0, 0, 0, 0, 0, 50}, 8, .3f, 15000f));

	}

	void addHulls() {

		hulls.Add(new Hull("hull1", new int[] {0, 3, 0, 0, 0, 0, 0, 0}, 20));
		hulls.Add(new Hull("hull2", new int[] {0, 0, 3, 0, 0, 0, 0, 0}, 50));
		hulls.Add(new Hull("hull3", new int[] {0, 0, 0, 3, 0, 0, 0, 0}, 100));

	}

	void addDrills() {

		drills.Add(new Drill("Copper Drill", new int[] {0, 10, 0, 0, 0, 0, 0, 0}, 25, 3));
		drills.Add(new Drill("Tiron Drill", new int[] {0, 0, 8, 8, 0, 0, 0, 0}, 22, 5));
		drills.Add(new Drill("Silver Drill", new int[] {0, 0, 0, 0, 10, 0, 0, 0}, 20, 5));
		drills.Add(new Drill("Gold Drill", new int[] {0, 0, 0, 0, 0, 10, 0, 0}, 15, 6));
		drills.Add(new Drill("Platinum Drill", new int[] {0, 0, 0, 0, 0, 0, 15, 0}, 12, 7));
		drills.Add(new Drill("Diamond Drill", new int[] {0, 0, 0, 0, 0, 0, 0, 30}, 5, 8));

	}

	void addOres() {


		ores = new Ore[9];
		ores[0] = new Ore("Dirt", 0, 0, 1f, rgbToFloat(51f, 51f, 51f));
		ores[1] = new Ore("Copper", .9f, 50f, .99f, rgbToFloat(221f, 136f, 85f));
		ores[2] = new Ore("Tin", .8f, 100f, .97f, rgbToFloat(0f, 0, 170f));
		ores[3] = new Ore("Iron", .7f, 150f, .94f, rgbToFloat(255f, 100f, 100f));
		ores[4] = new Ore("Silver", .6f, 300f, .8f, rgbToFloat(170f, 255f, 102f));
		ores[5] = new Ore("Gold", .5f, 350f, .65f, rgbToFloat(238f, 238f, 119f));
		ores[6] = new Ore("Platinum", .3f, 400f, .5f, rgbToFloat(204f, 68f, 204f));
		ores[7] = new Ore("Diamond", .1f, 450f, .3f, rgbToFloat(170f, 255f, 238f));
		ores[8] = new Ore("Idol", .1f, 1000f, .3f, rgbToFloat(255f, 0f, 0f));

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
//		Debug.Log(ores[oreType].name);
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
