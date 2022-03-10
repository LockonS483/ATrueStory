using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileUpgradeEvent : EventParent
{
    public override void ApplyEvent()
    {
        print("triggered event");
        GameManager gm = GameObject.FindObjectsOfType<GameManager>()[0].GetComponent<GameManager>();
        gm.missileUp += 1;
    }
}
