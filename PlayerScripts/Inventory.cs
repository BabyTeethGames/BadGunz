using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
   
    // look into using structs instead of classes

   
    public Dictionary<string, int> gunzNAmmo;

    //Player Variables
    private Player player;
    public int playerNum;
    public Camera cam;

    //Weapon Variables
    public GameObject weapon;
    public GameObject oldWeaponPickup;
    public GameObject grenade;
    public bool armed = false;
    public float rayDist = 10;
    public Weapon currentWeapon;
    private Ammo ammo;
    public Transform hand;
    public Transform grenadeSpawn;
    private List<GameObject> gunz;
    private int grenadeAmmo;  
   

    // UI variables
    public Text gunPickupText;
    public string m_AmmoCountText;
    public Text grenadeCount;
    public GameObject pickupPanel;
    public GunPickup weapSpecs;
    private string pickup;
    public string currentAmmo = null;

    public void Init(string pu)
    {
        gunzNAmmo = new Dictionary<string, int>();
        player = GetComponent<Player>();
        playerNum = player.m_PlayerNum;
        pickup = pu;
    }

    public void Pickup()
    {
        RaycastHit swap;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out swap, rayDist))
        {

            if (swap.collider.tag == "Gun")
            {
                GunPickup(swap);
            }
         
            else
                pickupPanel.SetActive(false);
        }

        if (!Physics.Raycast(ray, rayDist))
            pickupPanel.SetActive(false);

        if (currentAmmo != null && currentWeapon != null)
            m_AmmoCountText = currentWeapon.clipAmmo.ToString() + "/" + gunzNAmmo[currentAmmo].ToString();
        else
            m_AmmoCountText = "- -";
        player.m_UI.AmmoUpdate(m_AmmoCountText);
    }

    private void GunPickup(RaycastHit swap)
    {

        gunPickupText.text = "Pickup " + swap.collider.name;
        pickupPanel.SetActive(true);
        if (Input.GetButtonDown(pickup))
        {          
            DropGun();
            weapSpecs = swap.collider.GetComponent<GunPickup>();
            weapon = Instantiate(weapSpecs.gun, hand.transform.position, hand.transform.rotation, hand.transform);
            currentWeapon = weapon.GetComponent<Weapon>();
            currentWeapon.clipAmmo = weapSpecs.clipAmmo;
            currentWeapon.playerShot = playerNum;
            Destroy(swap.collider.gameObject);
            armed = true;
            UpdateDictionary(currentWeapon.ammoName, 0);
            currentAmmo = currentWeapon.ammoName;
        }
    }

    public void DropGun()
    {
        if (armed != false)
        {
            GameObject oldWeaponPickup = Instantiate(currentWeapon.gunPickup, hand.transform.position, hand.transform.rotation) as GameObject;
            oldWeaponPickup.name = weapSpecs.gun.name;
            oldWeaponPickup.GetComponent<GunPickup>().clipAmmo = currentWeapon.clipAmmo;

            if (oldWeaponPickup.GetComponent<Rigidbody>())
            {
                Rigidbody rb = oldWeaponPickup.GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 500);
            }
            currentWeapon = null;
            currentAmmo = null;
            armed = false;
            weapSpecs = null;
            Destroy(weapon.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Ammo"))
        {
            ammo = other.GetComponent<Ammo>(); 
            UpdateDictionary(ammo.ammo, ammo.ammoCount);
            Destroy(other.gameObject);
            
        }

        if (other.CompareTag("Grenade"))
        {
            grenadeAmmo = 1;
            grenade = other.GetComponent<GrenadeAmmo>().grenade;
            grenadeCount.text = grenadeAmmo.ToString();
        }


    }

    public void Fire()
    {
        if (currentWeapon != null)
            currentWeapon.Fire();
    }

    public void Reload()
    {
        
         if (currentWeapon != null)
         currentWeapon.Reload();
    }

    public void Grenade()
    {
        if (grenadeAmmo > 0)
        {
            if (grenade != null)
            {
                Instantiate(grenade, grenadeSpawn.position, grenadeSpawn.rotation);
                grenadeAmmo = 0;
                grenadeCount.text = grenadeAmmo.ToString();
            }
        }
    }

    void UpdateDictionary(string entry, int ammo)
    {
        if (gunzNAmmo.ContainsKey(entry))
        {
            gunzNAmmo[entry] += ammo;
            if(currentWeapon !=null)
            {
                currentWeapon.ammo = gunzNAmmo[entry];
            }
        }
        else
        {
            gunzNAmmo.Add(entry, ammo);
        }
    }

   
}
