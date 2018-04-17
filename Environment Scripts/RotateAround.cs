using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour {


    public float speed = 5;
    public Transform rotateTarget;

    void Update () {
        transform.RotateAround(rotateTarget.position, Vector3.up, speed * Time.deltaTime);
    }
}
