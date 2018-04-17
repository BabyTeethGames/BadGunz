using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowmingLauncher : ProjectileGun {

    public GameObject tracer;
    private Transform target;
    delegate void DeleShoot();
    DeleShoot deleShoot;
    // Use this for initialization


    public override void Start()
    {
        deleShoot = TracerFire;
        base.Start();
    }


    public override void Shoot()
    {

        deleShoot();

    }

    private void missileShoot()
    {
        var slowmingMissile = Instantiate(bullet, shotSpawn.position, shotSpawn.rotation);
        SlowmingMissile slowm = slowmingMissile.GetComponent<SlowmingMissile>();
        slowm.playerShot = playerShot;
        slowm.target = target;
        deleShoot = TracerFire;

    }

    private void TracerFire()
    {
        clipAmmo += 1;
        RaycastHit hit;
        if (Physics.Raycast(cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f)), cam.transform.forward, out hit))
        {
            Quaternion targRot = Quaternion.LookRotation(Vector3.forward, hit.normal.normalized); 
            var trace = Instantiate(tracer, hit.point, targRot);
            trace.transform.SetParent(hit.transform);
            target = trace.transform;
           
            deleShoot = missileShoot;

        }
        else
        {
            nextFire = Time.time - fireRate;
        }
    }
}
