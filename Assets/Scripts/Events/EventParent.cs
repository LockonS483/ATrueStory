using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventParent : MonoBehaviour
{
    public void Confirm(){
        ApplyEvent();
        Destroy(gameObject);
    }

    public abstract void ApplyEvent();
}
