using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parenter : MonoBehaviour {

	public int height;
	public float widthPercentage;

	// Use this for initialization
	void Start () {

		int width = (int)((float)GroundGenerator.me.groundWidth * widthPercentage);
		transform.localScale = new Vector3(width, height, transform.localScale.z);
		
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = Player.me.transform.position;
		
	}

	void OnTriggerEnter2D(Collider2D coll) {

		if (coll.gameObject.tag == "ground") {
			if (coll.transform.parent != GroundGenerator.me.transform) {
				coll.transform.parent = GroundGenerator.me.transform;
			}
		}

	}
}
