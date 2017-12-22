using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : FSMState {

    const int ATTACK_DIST = 35;
    const int LOST_DIST = 40;

    GameObject player;

    public Attack()
    {
        stateID = FSMStateID.Attack;
        curRotSpeed = 2.0f;

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
        Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);
        npc.Rotate(Vector3.forward * Time.deltaTime * curSpeed);
    }
}
