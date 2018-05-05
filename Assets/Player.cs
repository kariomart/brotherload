using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public static Player me;

	public float tuningScale = .001f;
	public float fuelCapacity;

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
	public int digCounter;
	public int digSpeed;

	//GAMEPLAY VARS

	public List<int> inventory = new List<int>();

	public int inventorySize;
	public float fueltankSize;
	public float fuelLossRate;



	void Start () {

		me = this;
		rb = GetComponent<Rigidbody2D>();
		box = GetComponent<BoxCollider2D>();
		
	}
	
	void Update () {

		float horizontal = Input.GetAxis("Horizontal");
		
		left = horizontal < 0;
		right = horizontal > 0;
		
		if (Input.GetKey(KeyCode.W)) {
			jumpFlag = true;
			up = true;
		} else {
			up = false;
		}

		if (Input.GetKey(KeyCode.S) && colliding && !up) {
			dig("down");
		}

		if (Input.GetKey(KeyCode.D) && colliding && !up) {
			dig("right");
		}

		if (Input.GetKey(KeyCode.A) && colliding && !up) {
			dig("left");
		}
	
	}

	void FixedUpdate() {

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
				inventory.Add(oreType);
			}

			Destroy(tileBeingDestroyed);
			tileBeingDestroyed = null;
		}

		rb.MovePosition((Vector2)transform.position + vel);
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

		if (coll != null) {
			digging = true;
			tileBeingDestroyed = coll.gameObject;
		}
	}


	void OnCollisionStay2D(Collision2D coll) {
//		Debug.Log(coll.gameObject.name);
		colliding = true;
		justJumped = false;
		
	}
	void OnCollisionExit2D(Collision2D coll) {
		colliding = false;
		justJumped = true;
	}

	void SetGrounded() {
		Vector2 pt1 = transform.TransformPoint(box.offset + new Vector2(box.size.x / 2, -box.size.y / 2) + new Vector2(-.01f, groundedOffset));//(box.size / 2));
        Vector2 pt2 = transform.TransformPoint(box.offset - (box.size / 2) + new Vector2(.01f, groundedOffset));
	
		bool prevGrounded = grounded;
		grounded = Physics2D.OverlapArea(pt1, pt2, LayerMask.GetMask("Dirt"));
		//Debug.Log(Physics2D.OverlapArea(pt1, pt2, LayerMask.GetMask("Dirt")));
		Debug.DrawLine(pt1, pt2, Color.cyan);
	}
}
