using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserUpgradeEvent : EventParent
{
    public override void ApplyEvent()
    {
        print("triggered event");
        GameManager gm = GameObject.FindObjectsOfType<GameManager>()[0].GetComponent<GameManager>();
        gm.laserUp += 1;
    }
}
