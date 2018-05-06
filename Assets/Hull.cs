using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hull {

	public string name;
	public int[] requiredOre;
	public float armor;

	public Hull(string name, int[] requiredOre, float armor) {

		this.name = name;
		this.requiredOre = requiredOre;
		this.armor = armor;

	}



}
