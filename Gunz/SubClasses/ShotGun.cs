using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : ProjectileGun {

    public Transform shotSpawnDos;
    public float randX;
    public float randY;
    public float randZ;
    Quaternion randomAngle;
    public int buckShot;

    public override void Shoot()
    {
        for (int i = 0; i < buckShot; ++i)
        {
            
            randomAngle = Quaternion.Euler(RandomVector(randX,randY,randZ));
            var projectileDos = Instantiate(bullet, shotSpawnDos.position, shotSpawnDos.rotation * randomAngle);
            randomAngle = Quaternion.Euler(RandomVector(randX, randY, randZ));
            projectileDos.GetComponent<Projectile>().playerShot = playerShot;
            var projectile = Instantiate(bullet, shotSpawn.position, shotSpawn.rotation*randomAngle);
            projectile.GetComponent<Projectile>().playerShot = playerShot;
            sound.Play();

        }
    }

    private Vector3 RandomVector(float x, float y, float z)
    {
        return new Vector3(Random.Range(-x, x), Random.Range(-z, z), Random.Range(-z, z));
    }
}
