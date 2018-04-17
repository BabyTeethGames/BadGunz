using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigGun : ProjectileGun {


    public float biteTimerMin = 20;
    public float biteTimerMax = 30;
    public float biteChance = 50;
    public int pigBiteDamage;
    private float bitetimer;
    Player play;

    // Update is called once per frame
    public override void Start()
    {
        bitetimer = Random.Range(biteTimerMin, biteTimerMax) + Time.time;
        play = GetComponentInParent<Player>();
        base.Start();
    }

    public override void Update()
    {
        if (bitetimer < Time.time)
        {
            Debug.Log("bite?");
            Bite();
        }
        base.Update();
    }

    void Bite()
    {
        if (Random.Range(0, 100) < biteChance)
        {
            Debug.Log("BITE");
            play.Damaged(pigBiteDamage, 0);
            if (play.m_Health > 0)
            {
                inventory.DropGun();
            }
        }
        else
        {
            bitetimer = Random.Range(biteTimerMin, biteTimerMax) + Time.time;
        }
    }
}
