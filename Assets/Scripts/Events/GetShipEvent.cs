using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetShipEvent : EventParent
{
    public override void ApplyEvent()
    {
        print("triggered event");
        GameManager gm = GameObject.FindObjectsOfType<GameManager>()[0].GetComponent<GameManager>();
        int randShip = Random.Range(0, 3);
        gm.SpawnPlayerShip(randShip);
    }
}
