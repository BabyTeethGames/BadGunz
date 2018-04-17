using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSky : MonoBehaviour {

    public float angle = 20;
    private Quaternion randomAngle;
    public GameObject meteor;
    public float dropRate = 1;
    private float nextDrop;

	// Use this for initialization
	void Start () {
        nextDrop = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > nextDrop)
        {
            nextDrop = Time.time + dropRate;
            ShootMeteor();
        }
	}

    void ShootMeteor ()
    {
        randomAngle = Quaternion.Euler(RandomVector(angle));
        Instantiate(meteor, transform.position, transform.rotation * randomAngle);
    }

    private Vector3 RandomVector(float x)
    {
        return new Vector3(Random.Range(-x, x), Random.Range(-x, x), Random.Range(-x, x));
    }
}
