using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : HitScan {


    Camera[] cams;

    public override void Start()
    {
        cams = GetComponentsInParent<Camera>();
        foreach (Camera camera in cams)
        {
            camera.fieldOfView = 18;
        }
        base.Start();
    }


    private void OnDestroy()
    {
        foreach (Camera camera in cams)
        {
            camera.fieldOfView = 60;
        }
    }
}
