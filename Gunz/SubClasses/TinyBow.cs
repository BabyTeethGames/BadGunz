using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyBow : ProjectileGun {

    bool nocked = false;
    float arrowForce = 1;
    public float pullForce = 5;
    public float maxForce = 15;
    public Transform handReturn;
    string fire;
    GameObject arrow;

    public override void Start()
    {       
        fire = GetComponentInParent<FPSController>().Inputs[5];
        base.Start();
        //crosshairImage.enabled = false;
    }

    public override void Update()
    {

        if (inventory.gunzNAmmo.TryGetValue(ammoName, out ammo) && !reloading)
        {
            if (Input.GetButton(fire))
            {
                if (clipAmmo <= 0 && reloading == false)
                {
                    Reload();
                }
                if (clipAmmo > 0 && Time.time > nextFire)
                {
                    if (nocked == false)
                    {
                        arrow = Instantiate(bullet, shotSpawn.position, shotSpawn.rotation);
                        arrow.transform.parent = shotSpawn;
                        nocked = true;
                    }

                    if (nocked == true && arrowForce < maxForce)
                    {
                        anim.m_IKRH.transform.Translate(Vector3.forward * 0.1f * -Time.deltaTime);
                        arrowForce += pullForce * Time.deltaTime;
                        arrow.transform.Translate(new Vector3(0, 0, -0.1f) * Time.deltaTime);

                    }
                }  
            }

            if (nocked == true && !Input.GetButton(fire))
            {
                anim.m_IKRH.position = handReturn.position;
                Shoot();
            }
        }

        base.Update();
        
    }
    
    public override void Fire()
    {
    
    }

    public override void Shoot()
    {
        float arrowFwdVel = controller.transform.InverseTransformDirection(controller.velocity).z;
        Rigidbody arrowBod = arrow.GetComponent<Rigidbody>();
        arrow.GetComponent<Arrow>().enabled = true;
        nocked = false;
        arrow.transform.parent = null;
        arrowBod.isKinematic = false;
        if (controller.transform.InverseTransformDirection(controller.velocity).z > 0)
            arrowBod.velocity = ShotTragectory() * arrowFwdVel;
        arrowBod.AddForce(ShotTragectory() * arrowForce, ForceMode.Impulse);
        arrowForce = 1;
        clipAmmo -= 1;
        nextFire = Time.time + fireRate;
    }

    private void OnDestroy()
    {
        crosshairImage.enabled = true;
    }
}
