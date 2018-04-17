using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Weapon : MonoBehaviour {

 
    [HideInInspector] public int ammo;
    [HideInInspector] public int clipAmmo;
    [HideInInspector] public float nextFire;
    [HideInInspector] public int playerShot;
    [HideInInspector] public Inventory inventory;
    [HideInInspector] public FPSController controller;

    public Camera cam;
    public string ammoName;
    public int clipSize;
    public float fireRate;
    public float weight;
    public GameObject gunPickup;
    public VirtualAudioSource sound;
    public Image crosshairImage;
    public WeaponAnimator anim;
    
    RaycastHit crossHit;

    // Use this for initialization
    virtual public void Start()
    {
        //get player inventory for ammo quantity
        inventory = GetComponentInParent<Inventory>();
        anim = GetComponent<WeaponAnimator>();
        crosshairImage = transform.root.Find("PlayerUI/Crosshair").gameObject.GetComponent<Image>();
        cam = GetComponentInParent<Camera>();
        controller = GetComponentInParent<FPSController>();

        if (GetComponent<VirtualAudioSource>())
            sound = GetComponent<VirtualAudioSource>();
        else
            Debug.Log("This gun has no sound attached");

        controller.speed -= weight;
    }

    public virtual void Update()
    {
        if (crosshairImage.isActiveAndEnabled)
        {
            if (Physics.Raycast(cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f)), cam.transform.forward, out crossHit))
            {
                if (crossHit.collider.CompareTag("Player"))
                {
                    crosshairImage.color = Color.red;
                }
                //gotta figure out a better way to do this
                else
                    crosshairImage.color = Color.black;
            }
            else if (crosshairImage.color == Color.red)
                crosshairImage.color = Color.black;
        }

    }

    public virtual void Fire()
    {
        
    }
    public virtual void Reload()
    {

    }

    private void OnDestroy()
    {
        controller.speed += weight;
    }

}
