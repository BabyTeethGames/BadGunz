using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kube : ProjectileGun {

    public override void Shoot()
    {
        base.Shoot();
        projectile.GetComponent<CubeShoot>().initialVelocity = (controller.transform.InverseTransformVector(controller.velocity));
    }
}
