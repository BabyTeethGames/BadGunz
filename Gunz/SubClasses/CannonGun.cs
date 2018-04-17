using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonGun : ProjectileGun {

    bool firing = false;
    public float fuseMin;
    public float fuseMax;
    public ParticleSystem fuseSpark;
    public VirtualAudioSource fuseSound;

    public override void Fire()
    {

        if (firing == false)
        {
            base.Fire();
        }

    }

    public override void Shoot()
    {
        StartCoroutine("Fuse");
    }

    IEnumerator Fuse()
    {
        fuseSound.Play();
        reloading = true;
        firing = true;
        float fuse = Random.Range(fuseMin, fuseMax);
        fuseSpark.Play();
        yield return new WaitForSeconds(fuse);
        fuseSpark.Stop();
        base.Shoot();
        fuseSound.Stop();
        sound.Play();
        reloading = false;
        firing = false;        
    }
}
