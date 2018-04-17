using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : Gun {

    public GameObject bullet;
    public Transform shotSpawn;
    [HideInInspector] public GameObject projectile;
    // Use this for initialization
  

    public override void Shoot()
    {
        anim.Fire();
        projectile = Instantiate(bullet, shotSpawn.position, Quaternion.LookRotation(ShotTragectory()));
        projectile.GetComponent<Projectile>().playerShot = playerShot;
    }

    public Vector3 ShotTragectory()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f)), cam.transform.forward, out hit))
        {
            Vector3 traj = shotSpawn.position - hit.point;
            traj = -traj.normalized;
            return traj;
        }

        else
            return shotSpawn.forward;
    }

  
}
