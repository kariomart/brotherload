using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour {

	public static GroundGenerator me;
	public GameObject thePad;
	public AudioSource soundtrack;
	public GameObject wall1;
	public GameObject wall2;

	public GameObject groundTile;

	public MeshRenderer quad;

	BoxCollider2D box;
	Rigidbody2D rb;
	Texture2D tex;

	public int groundWidth;
	public int groundHeight;
	public int mapStartY;

	//public List<GameObject> tiles = new List<GameObject>();
	public int[,] tiles;
	public int[,] newTiles;
	public GameObject[,] tileObjects;
	GameObject currentTile;

	public int clusterSize;
	public float frequency;
	public int caNeighbors;
	public int caSteps;

	public int[] trickleCounters;
	public int trickleCounter;
	public bool trickled;


	// Use this for initialization
	void Start () {

		me = this;
		box = GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();

		tex = new Texture2D(groundWidth, groundHeight);

		tiles = new int[groundWidth, groundHeight];
		newTiles = new int[groundWidth, groundHeight];
		tileObjects = new GameObject[groundWidth, groundHeight];

		trickleCounters = new int[2];

		generateMapData();
		CA();
		shrinePass();
		cleanupTiles();

		if (Master.me.numShrines == 0) {
			spawnShrine(Random.Range(6, groundWidth - 6), Random.Range(6, 20));
		}

		wall1.transform.position = new Vector3(0, transform.position.y, transform.position.z);
		wall2.transform.position = new Vector3(groundWidth, transform.position.y, transform.position.z);
		Player.me.transform.position = new Vector3(thePad.transform.position.x, thePad.transform.position.y  + 10, Player.me.transform.position.z);

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.R)) {
			Application.LoadLevel(0);
		}

		if (Input.GetKeyDown(KeyCode.Alpha9)) {
			soundtrack.mute = !soundtrack.mute;
		}


	}

	void FixedUpdate() {

		 if (!trickled) {
			trickleTiles();
		 }

	}


	void generateMapData() {
	
		float xOffset = Random.Range(0.0f, 100.0f);
		float yOffset = Random.Range(0.0f, 100.0f);
		for (int x = 0; x < groundWidth; x++) {
			for (int y = 0; y < groundHeight; y++) {
			
				transform.hierarchyCapacity = 10000;
				float perlinVal = Mathf.PerlinNoise((float)x * frequency + xOffset, (float)y * frequency + yOffset);
				int index = (int)(perlinVal * 10);
				GameObject newTile = Instantiate(groundTile);
				newTile.transform.position = new Vector2(x, y);
				generateTile(index, newTile, x, y);
				tex.SetPixel(x, y, new Color(perlinVal, perlinVal, perlinVal));

			}
		}

		tex.filterMode = FilterMode.Point;
		tex.Apply();
		quad.material.mainTexture = tex;
		thePad.transform.position = new Vector2(groundWidth/2, groundHeight + 2);
		//Player.me.transform.position = new Vector2(groundWidth / 2, groundHeight);

	}



	void generateTile(int index, GameObject tile, int x, int y) {

		int oreType = 0;


		switch(index){
			
			case 1:
			break;

			case 2:
				
				if(Random.value < 0.5){
					oreType = 3;
				} else{
					oreType = 0;
				}

			break;

			case 3:
				
				if(Random.value < 0.3){
					oreType = 4;
				}else{
					oreType = 0;
				}
			break;

			case 4:
				//gold
				if(Random.value < 0.4){
					oreType = 5;
				}else{
					oreType = 0;
				}
				
			break;

			case 5:
				//tin
				if(Random.value < 0.8){
					oreType = 2;
				}
				
				else{
					oreType = 0;
				}

			break;

			
			case 6:

				if(Random.value < 0.6){
					oreType = 1;
				}else{
					oreType = 0;
				}

			break;

			case 7:
			//platinum
				if(Random.value < 0.8){
					oreType = 6;
				}else{
					oreType = 0;
				}

			break;

			case 8:
			
			break;

			case 9:
				oreType = 7;
			break;

			case 10:
			//insanely rare stuff

			break;

			default:
				oreType = 0;
			break;
		}

		// if(Random.value < Mathf.Pow(((float)y/(float)groundHeight), 2)){
		// 	oreType = 0;
		// }

		//Debug.Log(Master.me.ores[oreType].name + " " + y + " " + Master.me.ores[oreType].minDepth);
		//Debug.Log(y < Master.me.ores[oreType].minDepth);
		

		if (oreType >= 0 && y < groundHeight * Master.me.ores[oreType].minDepth) {
			//Debug.Log("GOOD TILE");
			tile.GetComponentInChildren<SpriteRenderer>().color = Master.me.ores[oreType].color;
			tile.gameObject.name = Master.me.ores[oreType].name;
		} else {
			//Debug.Log("BAD TILE");
			oreType = 0;
			tile.GetComponentInChildren<SpriteRenderer>().color = Master.me.ores[oreType].color;
			tile.gameObject.name = Master.me.ores[oreType].name;
			
		}

		if (Random.value < 0.1f) {
			oreType = -1;
		}

		tiles[x, y] = oreType;
		newTiles[x, y] = oreType;
		tileObjects[x, y] = tile;

	}

	void updateDisplay() {

		for (int x = 0; x < groundWidth; x++) {
			for (int y = 0; y < groundHeight; y++) {
				GameObject tile = tileObjects[x,y];
				int oreType = tiles[x,y];

				tile.SetActive(tiles[x, y] >= 0);
				if (oreType >= 0) {
					tile.GetComponentInChildren<SpriteRenderer>().color = Master.me.ores[oreType].color;
					tile.gameObject.name = Master.me.ores[oreType].name;
				}
			}
		}
	}

	void cleanupTiles() {

		for (int x = 0; x < groundWidth; x++) {
			for (int y = 0; y < groundHeight; y++) {
				if (!tileObjects[x, y].activeInHierarchy) {
					Destroy(tileObjects[x,y]);
				} else {
				//tileObjects[x,y].transform.parent = this.transform;
				}
			}
		}
	}

	void trickleTiles() {


		for(int i = 0; i < groundWidth; i++) {
			GameObject tile = tileObjects[i, trickleCounter];

			if (tile) {
				tile.transform.parent = this.transform;
			}
		}

		if (trickleCounter == groundHeight - 1) {
			trickled = true;
		}

		trickleCounter ++;
	}

	void shrinePass() {

		for (int x = 0; x < groundWidth; x++) {
			for (int y = 0; y < groundHeight; y++) {

				if (Random.value < 0.001f && x >  5 && x < groundWidth - 5 && y < groundHeight*.3f && y < groundHeight - 5) {
					Debug.Log("SHRINE SPAWN");
					spawnShrine(x, y);

				}
			}
		}

		updateDisplay();
	}

	void spawnShrine(int a, int b) {

//		Debug.Log(a + " " + b);
		Master.me.numShrines ++;
		for (int x = 0; x < 5; x++) {
			for (int y = 0; y < 5; y++) {

				//Debug.Log("test");
				tiles[a + x, b + y] = Master.me.shrine[x, y];
			}
		}
	}
		

	void CA() {

		for (int i = 0; i < caSteps; i++) {
			doCAStep();
		}

		updateDisplay();

	}

	void doCAStep() {

		for (int x = 0; x < groundWidth; x++) {
			for (int y = 0; y < groundHeight; y++) {
				newTiles[x, y] = nextState(x, y);
			}
		}

		for (int x = 0; x < groundWidth; x++) {
			for (int y = 0; y < groundHeight; y++) {
				tiles[x, y] = newTiles[x, y];
			}
		}
	}

	int nextState(int x, int y) {

		int livingNeighborCount = 0;
		for (int nX = x-1; nX <= x+1; nX++) {
			for (int nY = y-1; nY <= y+1; nY++) {
				if (nX < 0 || nX >= groundWidth || nY < 0 || nY >= groundHeight) {
					continue;
				}

				int neighborState = tiles[nX, nY];
				if (neighborState >= 0) {
					livingNeighborCount++;
				}
			}
		}

		if (livingNeighborCount >= caNeighbors) {
			return tiles[x, y];
		}

		return -1;
	}


	GameObject getTileObject(int type) {
		if (type == -1) {
			return groundTile;
		} 

		if (type >= 0 && type <= Master.me.ores.Length) {

			GameObject ore = groundTile;
			return ore;
		}
		
		else {
			return groundTile;
		}

	}

}
