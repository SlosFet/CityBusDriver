using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightDetector : MonoBehaviour
{
    //Using it if bus trigger with a traffic light when it coming back.So it is not actually going through to light
    //Firstly bus trigger with it which is coming first from traffic lights and say to traffic light "hey there is a bus"
    public GameObject Bus { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        Bus = other.attachedRigidbody.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (Bus != null)
            Bus = null;
    }
}
