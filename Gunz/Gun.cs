using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    [HideInInspector] public bool reloading = false;
    public float reloadTime = 1;
    private float gunTurnTime = 0.5f;


    public override void Start()
    {
        
        base.Start();
    }


    public override void Fire()
    {
        //instantiate bullets, update inventory dictionary's ammo

        if (inventory.gunzNAmmo.TryGetValue(ammoName, out ammo) && !reloading)
        {
            if (clipAmmo <= 0 && reloading == false)
            {
                Reload();
            }
            if (clipAmmo > 0 && Time.time > nextFire)
            {
                Shoot();
                clipAmmo -= 1;
                nextFire = Time.time + fireRate;
            }
            
        }
    }

    public virtual void Shoot()
    {

    }


    public override void Reload()
    {
        if (reloading == false && clipAmmo != clipSize && ammo > 0)
        {            
            StartCoroutine("ReloadAnim");
        }
    }




    IEnumerator ReloadAnim()
    {
        reloading = true;
        float myTime = 0;
        while (myTime < gunTurnTime)
        {
            transform.Rotate(100*Time.deltaTime, 0, 0);
            myTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(reloadTime);
        myTime = 0;
        ClipFill();
        while (myTime < gunTurnTime)
        {
            transform.Rotate(-100 * Time.deltaTime, 0, 0);
            myTime += Time.deltaTime;
            yield return null;
        }
        yield return null;       
        reloading = false;
    }

    private void ClipFill()
    {
        int clip = 0;

        if (ammo > 0 && clipSize > clipAmmo)
        {
            if (ammo >= clipSize - clipAmmo)
            {
                clip = clipSize - clipAmmo;
                clipAmmo += clip;
            }
            else if (ammo < clipSize - clipAmmo)
            {
                clip = ammo;
                clipAmmo += ammo;
            }
        }
        inventory.gunzNAmmo[ammoName] = inventory.gunzNAmmo[ammoName] - clip;
        ammo = inventory.gunzNAmmo[ammoName];
    }
}
