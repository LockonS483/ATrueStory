using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventButton : MonoBehaviour
{
    public EventParent eparent;
    // Start is called before the first frame update
    void OnMouseDown(){
        eparent.Confirm();
    }
}
