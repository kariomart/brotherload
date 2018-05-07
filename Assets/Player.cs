using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour {

	public static Player me;
	public GameObject reticle;
	public GameObject thePad;
	public Camera cam;

	public float tuningScale = .001f;

	Rigidbody2D rb;
	BoxCollider2D box;
	GameObject tileBeingDestroyed;

	//MOVEMENT VARS

	bool left;
	bool right;
	bool up;
	bool grounded;
	bool colliding;
	bool jumpFlag;
	bool justJumped;
	bool digging;
	public bool wonGame;

	public Vector2 vel;
	public float moveAccel;
	public float maxMoveSpeed;
	public float drag;
	public float initialThrustSpeed;
	public float thrustSpeed;
	public float gravity;
	public float groundedOffset;

	public Transform digDownPt;
	public Transform digLeftPt;
	public Transform digRightPt;
	public Vector2 padPos;
	public int buttonHeldCounter;
	public int digCounter;
	public float digSpeed;
	public int digPadding;

	//GAMEPLAY VARS

	public float money;

//	public List<int> inventory = new List<int>();
	public int[] inventory;
	public int numInvOres;

	public int inventorySize;
	public float fueltankSize;
	public float fuelIdleRate;
	public float fuelMovingRate;
	public float fuel;

	public float cameraRampUp;

	public bool onPlatform;

	Drill drill;
	Engine engine;
	Hull hull;



	void Start () {

		me = this;
		rb = GetComponent<Rigidbody2D>();
		box = GetComponent<BoxCollider2D>();
		cam = Camera.main.GetComponent<Camera>();
		fuel = fueltankSize;
		drill = new Drill("Drill", new int[] {0, 0, 0, 0, 0, 0, 0, 0}, 30, 2);
		engine = new Engine("Engine", new int[] {0, 0, 0, 0, 0, 0, 0, 0}, maxMoveSpeed, 1, fueltankSize);
		hull = new Hull("Hull", new int[] {0, 0, 0, 0, 0, 0, 0, 0}, inventorySize);
		inventory = new int[9];
		//
		updateStats();

	}
	
	void Update () {

		numInvOres = inventory.Sum();

		if (Vector2.Distance(thePad.transform.position, transform.position) > 10) {
			reticle.transform.position = transform.position + (thePad.transform.position - transform.position).normalized;
		} else {
			reticle.transform.position = transform.position;
		}
		float horizontal = Input.GetAxis("Horizontal");
		
		left = horizontal < 0;
		right = horizontal > 0;
		
		if (Input.GetKey(KeyCode.W)) {
			jumpFlag = true;
			up = true;
		} else {
			up = false;
		}

		if (Input.GetKey(KeyCode.S) && colliding && grounded) {
			buttonHeldCounter ++;

			if (buttonHeldCounter > digPadding) {
				dig("down");
			}
		}

		else if (Input.GetKey(KeyCode.D) && colliding && grounded) {
			buttonHeldCounter ++;

			if (buttonHeldCounter > digPadding) {
				dig("right");
			}
		}

		else if (Input.GetKey(KeyCode.A) && colliding && grounded) {

			buttonHeldCounter ++;
			
			if (buttonHeldCounter > digPadding) {
				dig("left");
			}
		}

		else {
			buttonHeldCounter = 0;
		}

		
	
	}

	void FixedUpdate() {

		if (wonGame) {
			//rb.isKinematic = true;
			vel = Vector2.zero;
			thePad.transform.position += Vector3.up * .05f;
			cam.orthographicSize += .06f + cameraRampUp;
		}

		SetGrounded();

		if (left) {
			vel.x -= moveAccel * tuningScale;
		}

		if (right) {
			vel.x += moveAccel * tuningScale;
		}

		if (!left && !right) {
			vel.x = Mathf.MoveTowards(vel.x, 0, Mathf.Clamp01(drag * tuningScale));
//			Debug.Log("moving towards 0");
		}

		if (!grounded) {
			vel.y -= gravity * tuningScale;
		}

		if (grounded) {
			vel.y = 0;
		}

		if (jumpFlag && justJumped) {
			justJumped = false;
//			Debug.Log("initial thrust");
			vel.y = initialThrustSpeed * tuningScale;
		}

		else if (jumpFlag) {
			vel.y = thrustSpeed * tuningScale;
			jumpFlag = false;
		}



		vel.x = Mathf.Clamp(vel.x, -maxMoveSpeed, maxMoveSpeed);


		if (digging && digCounter < digSpeed) {
			digCounter ++;
			vel = (tileBeingDestroyed.transform.position - transform.position).normalized * 0.1f;
			//vel = Vector2.zero;
		} else if (digging) {
			digging = false;
			digCounter = 0;

			int oreType = Master.me.posToOre(tileBeingDestroyed.transform.position);
			if (oreType > 0) {
				//inventory.Add(oreType);
				inventory[oreType] ++;
				SoundController.me.PlaySound(SoundController.me.ore, 1f);
			}

			if (oreType == 8) {
				SoundController.me.PlaySound(SoundController.me.idol, 1f);
			}

			if (oreType == 0) {
				SoundController.me.PlaySound(SoundController.me.dig, .2f);
			}



			Destroy(tileBeingDestroyed);
			tileBeingDestroyed = null;
		} 


		rb.MovePosition((Vector2)transform.position + vel);

		if (!onPlatform) {
			if (fuel <= 500) {
				fuel -= fuelIdleRate * .5f;
				if (!SoundController.me.checkIfPlaying(SoundController.me.lowFuel)) {
					SoundController.me.PlaySound(SoundController.me.lowFuel);
				}
			} else {
				fuel -= fuelIdleRate;
			}	
		}

		if (fuel <= 0) {
			Application.LoadLevel(0);
		}
		//rb.velocity = vel;

	}

	void dig(string direction) {
		Collider2D coll = null;

		if (direction == "down") {
			coll = Physics2D.OverlapPoint(digDownPt.position);
		}

		if (direction == "left") {
			coll = Physics2D.OverlapPoint(digLeftPt.position);
		}

		if (direction == "right") {
			coll = Physics2D.OverlapPoint(digRightPt.position);
		}

		if (coll != null && coll.gameObject.tag == "ground") {
			tileBeingDestroyed = coll.gameObject;
			int oreType = Master.me.posToOre(tileBeingDestroyed.transform.position);

			if (drill.mineableOre >= oreType && (numInvOres < inventorySize || oreType == 0)) {
				digging = true;
			}

			if (numInvOres >= inventorySize || drill.mineableOre < oreType) {
				SoundController.me.PlaySound(SoundController.me.miningError, .5f);
			} 
		}
	}

	public void updateStats() {

		digSpeed = drill.speed;
		fueltankSize = engine.fuelTank;
		inventorySize = hull.size;

	}


	void OnCollisionStay2D(Collision2D coll) {
		//Debug.Log(coll.gameObject.name);
		colliding = true;
		if (!grounded) {
			//stopGoingThisWay((coll.gameObject.transform.position - transform.position).normalized);
		}
		justJumped = false;
		
	}

	void OnCollisionEnter2D(Collision2D coll) {
		

	}


	void OnCollisionExit2D(Collision2D coll) {
		colliding = false;
		justJumped = true;

	}

	void OnTriggerEnter2D(Collider2D coll) {

		if (coll.gameObject.tag == "platform") {
			onPlatform = true;
			UIController.me.toggleUpgradeDisplay();
			UpgradeUI.me.displayInventory();
			SoundController.me.PlaySound(SoundController.me.upgradeShop, 1f);
			//offsetCamera();
		}
	}

	void OnTriggerExit2D(Collider2D coll) {

		if (coll.gameObject.tag == "platform") {
			onPlatform = false;
			UIController.me.toggleUpgradeDisplay();
			SoundController.me.PlaySound(SoundController.me.upgradeShopLeft);
			//offsetCamera();
		}
	}

	public void stopGoingThisWay(Vector2 a) {
        vel -= a.normalized * Vector2.Dot(vel, a.normalized);
    }

	void SetGrounded() {
		Vector2 pt1 = transform.TransformPoint(box.offset + new Vector2(box.size.x / 2, -box.size.y / 2) + new Vector2(-.01f, groundedOffset));//(box.size / 2));
        Vector2 pt2 = transform.TransformPoint(box.offset - (box.size / 2) + new Vector2(.01f, groundedOffset));
	
		bool prevGrounded = grounded;
		grounded = Physics2D.OverlapArea(pt1, pt2, LayerMask.GetMask("Dirt"));
		//Debug.Log(Physics2D.OverlapArea(pt1, pt2, LayerMask.GetMask("Dirt")));
		Debug.DrawLine(pt1, pt2, Color.cyan);
	}

	void offsetCamera() {

		Vector3 pos = Camera.main.transform.localPosition;
		Camera.main.transform.position = new Vector3(pos.x - 1.5f, pos.y, pos.z);

	}

	void resetCamera() {

		Vector3 pos = Camera.main.transform.localPosition;
		Camera.main.transform.position = new Vector3(0, 0, pos.z);

	}

	public void godMode() {

		drill = Master.me.drills[Master.me.drills.Count - 1];
		engine = Master.me.engines[Master.me.engines.Count - 1];
		hull = Master.me.hulls[Master.me.hulls.Count - 1];
		money = 100000f;
		inventory[inventory.Length - 1] = 5;

		updateStats();
		fuel = fueltankSize;
	}

}
