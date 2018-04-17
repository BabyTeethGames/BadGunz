using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPlatform : MovingPlatform {


    public override void Update()
    {
        RotationUpdate();
        VelocityUpdate();
        MovePlayer(true);
    
    }


}
