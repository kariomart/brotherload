using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill {

	public string name;
	public int[] requiredOre;
	public float speed;

	public Drill(string name, int[] requiredOre, float speed) {

		this.name = name;
		this.requiredOre = requiredOre;
		this.speed = speed;

	}



}
