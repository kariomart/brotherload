using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore {

	public string name;
	public float depthPref;
	public float value;
	public Color color;

	public Ore(string name, float depthPref, float value, Color color) {

		this.name = name;
		this.depthPref = depthPref;
		this.value = value;
		this.color = color;

	}


}
