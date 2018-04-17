using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : Projectile {

    public AnimationCurve trajectory;
    public ParticleSystem explosion;
    
    public float thrust;
    public float acceleration;
    public float straightTime;
    public float explosionForce;
    public float multiplier;
    private float time;
    private float curvePosition;
    private float angleY;
    private float angleX;
    private float lifeTime;
    private float turnTime;
    private float randomAngleX;
    private float randomAngleY;
    


    // Use this for initialization
    void Start () {
        turnTime = Random.Range(0.5f, 1);
        angleX = transform.eulerAngles.x;
        angleY = transform.eulerAngles.y;
        lifeTime = Random.Range(100, 350);
    }
	
	// Update is called once per frame
	void Update () {
        if (straightTime > turnTime)
            Turn();
        
        transform.Translate(Vector3.forward * Time.deltaTime * thrust);
        thrust = thrust + Time.deltaTime * acceleration;

        if (straightTime > lifeTime)
            StartCoroutine("Explode");
    }

    private void FixedUpdate()
    {
        straightTime += 1;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Damage(other);
        if(!other.CompareTag("Ammo") && !other.CompareTag("Space"))
            StartCoroutine("Explode");
    }


    void Turn()
    {
        angleY = angleY + trajectory.Evaluate(curvePosition) * randomAngleY;
        angleX = angleX + trajectory.Evaluate(curvePosition) * 0.5f * randomAngleX;
        Vector3 turn = new Vector3(angleX, angleY, 0);
        curvePosition += Time.deltaTime;
        transform.eulerAngles = turn;

        if (curvePosition > time)
        {
            CreateCurve();
            curvePosition = curvePosition - time;
        }
    }

    void CreateCurve()
    {
        float curveAngle = Random.Range(-0.75f, 0.75f);
        float curveTime = Random.Range(0.1f, 0.25f);
        float curveAdditive = Random.Range(-1f, 1f);
        float curveTimeAdd = Random.Range(0.1f, 0.25f);
        randomAngleX = Random.Range(-1f, 1f);
        randomAngleY = Random.Range(-1f, 1f);
        trajectory = new AnimationCurve(new Keyframe(curveTime, curveAngle), new Keyframe(curveTime + curveTimeAdd, curveAngle + curveAdditive));
        time = trajectory[trajectory.length - 1].time;
    }

    private IEnumerator Explode()
    {
        
        float r = 10 * multiplier;
        var cols = Physics.OverlapSphere(transform.position, r);
        var rigidbodies = new List<Rigidbody>();
        foreach (var col in cols)
        {
            if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody))
            {
                rigidbodies.Add(col.attachedRigidbody);
            }
            if (col.CompareTag("Player"))
            {
                //wait for one frame to prevent double scoring on kills;
                yield return null;
                Damage(col);
            }

        }
        foreach (var rb in rigidbodies)
        {
            rb.AddExplosionForce(explosionForce * multiplier, transform.position, r, 1 * multiplier, ForceMode.Impulse);
        }

        Instantiate(explosion, transform.position, transform.rotation);

        Destroy(this.gameObject);
       
          
      
    }


}
