using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorGun : HitScan {

    
    RenderTexture mirrorTex;
    Material[] mirrorMats;
    public Renderer matRend;
    public Camera mirrorCam;
    
    
    
    public override void Start()
    {
        base.Start();
        mirrorMats = matRend.materials;
        crosshairImage.enabled = false;
        cam = GetComponentInChildren<Camera>();
        //might have to call texture create;
        mirrorTex = new RenderTexture(2048, 2048, 16, RenderTextureFormat.ARGB32);
        mirrorCam.targetTexture = mirrorTex;
        mirrorMats[1].SetTexture("_MainTex", mirrorTex);
        mirrorMats[1].SetTextureOffset("_MainTex", new Vector2(0.275f, 0.305f));
    }


    public override void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1.0f)), cam.transform.forward, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (hit.collider.GetComponent<Player>() != null)
                {
                    hit.collider.GetComponent<Player>().Damaged(damage, playerShot);
                }
                else
                {
                    Debug.Log("Player is lacking 'player' script");
                }
            }

            else if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * shotForce);
            }

            Instantiate(hitParticle, hit.point, Quaternion.identity);
        }

    }

    private void OnDestroy()
    {
        crosshairImage.enabled = true;
        mirrorTex.Release();
    }
}
