using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerLight : MonoBehaviour {


    public GameObject trackLight;
    bool lightOnOff;
    public float timer = 1;

	// Use this for initialization
	void Start () {
        StartCoroutine("offOn");
	}
	

    IEnumerator offOn()
    {
        while (true)
        {
            lightOnOff = !lightOnOff;
            trackLight.SetActive(lightOnOff);
            yield return new WaitForSeconds(timer);
        }
    }

}
