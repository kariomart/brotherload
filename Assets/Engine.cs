using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine {

	public string name;
	public int[] requiredOre;
	public float speed;
	public float fuelRate;
	public float fuelTank;

	public Engine(string name, int[] requiredOre, float speed, float fuelRate, float fuelTank) {

		this.name = name;
		this.requiredOre = requiredOre;
		this.speed = speed;
		this.fuelRate = fuelRate;
		this.fuelTank = fuelTank;

	}



}
