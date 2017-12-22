using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretController : AdvancedFSM {


    public Text StateText;
    //public GameObject Bullet;
    public float health;
    bool debugDraw;

    private string GetStateString()
    {
        string state = "NONE";
        if (currentState != null)
        {
            if (currentState.ID == FSMStateID.Idle)
            {
                state = "IDLE";
            }

            if (currentState.ID == FSMStateID.Attack)
            {
                state = "ATTACK";
            }

            if (currentState.ID == FSMStateID.Dead)
            {
                state = "CHASE";
            }

        }

        return state;
    }

    //Initialize the FSM for the NPC tank
    protected override void Initialize()
    {
        health = 100;
        elapsedTime = 0.0f;
        shootRate = 2.0f;
        debugDraw = true;
        //Find the Player
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        //get the turret transform (or tank gun)
        turret = gameObject.transform.GetChild(0).transform;

        //get the bullet spawnpoint, should be at the end of tank gun
        //bulletSpawnPoint = turret.GetChild(0).transform;

        //create the FSM for the tank
        ConstructFSM();


    }

    //Update each frame
    protected override void FSMUpdate()
    {
        //Check for health
        elapsedTime += Time.deltaTime;
    }

    protected override void FSMFixedUpdate()
    {
        if (CurrentState != null)
        {
            CurrentState.Reason(playerTransform, transform);
            CurrentState.Act(playerTransform, transform);
        }
        StateText.text = "Tank STATE IS: " + GetStateString();
        if (debugDraw)
        {
            UsefullFunctions.DebugRay(transform.position, transform.forward * 5.0f, Color.red);
        }

    }


    private void ConstructFSM()
    {
        //pointList = GameObject.FindGameObjectsWithTag("WayPoint");

        //Creating a waypoint transform array for each state
        //Transform[] waypoints = new Transform[pointList.Length];
       // int i = 0;
        //foreach (GameObject obj in pointList)
        //{
            //waypoints[i] = obj.transform;
        //    i++;
        //}
        //
        //Create States
        //
        Idle idle = new Idle();
        Attack attack = new Attack();


        //
        //Create Transitions for states
        ///
        idle.AddTransition(Transition.SawPlayer, FSMStateID.Attack);

        attack.AddTransition(Transition.LostPlayer, FSMStateID.Idle);

        AddFSMState(attack);
        AddFSMState(idle);

        currentState = idle;
    }




    /// <summary>
    /// Shoot the bullet from the turret
    /// </summary>
    public void ShootBullet()
    {
        if (elapsedTime >= shootRate)
        {
            //Instantiate(Bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            elapsedTime = 0.0f;
        }
    }





    /// <summary>
    /// Check the collision with the bullet
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        //Reduce health
        if (collision.gameObject.tag == "Bullet")
        {
            health -= 50;

            if (health <= 0)
            {
                Debug.Log("Switch to Dead State");
                PerformTransition(Transition.NoHealth);
                Explode();
            }
        }
    }


    protected void Explode()
    {
        float rndX = Random.Range(10.0f, 30.0f);
        float rndZ = Random.Range(10.0f, 30.0f);
        for (int i = 0; i < 3; i++)
        {
            GetComponent<Rigidbody>().AddExplosionForce(10000.0f, transform.position - new Vector3(rndX, 10.0f, rndZ), 40.0f, 10.0f);
            GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(rndX, 20.0f, rndZ));
        }

        Destroy(gameObject, 1.5f);
    }
}
