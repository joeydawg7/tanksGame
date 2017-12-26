using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : FSMState {

    const int ATTACK_DIST = 35;
    const int LOST_DIST = 40;

    float timer;

    GameObject player;

    public Attack()
    {
        stateID = FSMStateID.Attack;
        curRotSpeed = 2.0f;
        timer = 0;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void Reason(Transform player, Transform npc)
    {
        //Transition to idle on death
        if (npc.GetComponent<TurretController>().health <= 0)
        {
            npc.GetComponent<TurretController>().PerformTransition(Transition.NoHealth);
            return;
        }
        //transtion to idle when player leaves range
        else if (!IsInCurrentRange(npc, player.transform.position, LOST_DIST))
        {
            npc.GetComponent<TurretController>().PerformTransition(Transition.LostPlayer);
            return;
        }


    }

    //Rotate towards target 
    public override void Act(Transform player, Transform npc)
    {
        //rotate to look at player
        Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);
        npc.Rotate(Vector3.forward * Time.deltaTime * curSpeed);

        timer += Time.deltaTime;

        if (timer > 1.0f)
        {
            GameObject projectile = UnityEngine.Object.Instantiate(npc.GetComponent<TurretController>().shoot, npc.GetChild(1).transform.position, Quaternion.identity) as GameObject;
            projectile.GetComponent<Rigidbody>().AddForce(npc.transform.forward *5, ForceMode.Impulse);
            timer = 0;
        }

        

    }
}
