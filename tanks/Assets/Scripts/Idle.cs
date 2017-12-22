using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : FSMState {


    const int ATTACK_DIST = 35;

    GameObject player;

    public Idle()
    {
        stateID = FSMStateID.Idle;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void Act(Transform player, Transform npc)
    {
        //do nothing
    }

    public override void Reason(Transform player, Transform npc)
    {
        if (IsInCurrentRange(npc, player.transform.position, ATTACK_DIST))
        {
            npc.GetComponent<TurretController>().PerformTransition(Transition.SawPlayer);
            return;
        }
    }
}

