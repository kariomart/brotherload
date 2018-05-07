using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore {

	public string name;
	public float depthPref;
	public float value;
	public Color color;
	public float minDepth;

	public Ore(string name, float depthPref, float value, float minDepth, Color color) {

		this.name = name;
		this.depthPref = depthPref;
		this.value = value;
		this.color = color;
		this.minDepth = minDepth;


	}


}
