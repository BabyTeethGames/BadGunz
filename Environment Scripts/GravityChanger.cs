using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChanger : MonoBehaviour {

	// Use this for initialization
	void Start () {
            Physics.gravity = Physics.gravity / 2;
    }
	
	
}
