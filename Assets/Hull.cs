using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hull {

	public string name;
	public int[] requiredOre;
	public int size;

	public Hull(string name, int[] requiredOre, int size) {

		this.name = name;
		this.requiredOre = requiredOre;
		this.size = size;

	}



}
