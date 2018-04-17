using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlatform : MovingPlatform {

    public float speed;

    public override void Update()
    {       
        if (moving)
        {
            Move(target, speed);
            VelocityUpdate();
            MovePlayer(false);
        }
    }

    public void GetMove(Vector3 newTarget, float platformSpeed)
    {
        target = newTarget;
        speed = platformSpeed;
        moving = true;      
    }

    private void Move(Vector3 target, float speed)
    {
        if (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else
            moving = false;
    }
}
