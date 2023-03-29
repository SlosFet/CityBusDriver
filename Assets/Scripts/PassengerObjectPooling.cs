using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerObjectPooling : MonoBehaviour
{
    //I use a singleton and object pooling pattern in a same script because it will be only one object in game and also it has a one job
    public static PassengerObjectPooling Instance;

    [SerializeField] List<Passenger> _passengers;
    private Passenger currentPassenger;
    private int index;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public Passenger GetPassenger()
    {
        currentPassenger = _passengers[index];
        currentPassenger.gameObject.SetActive(true);

        index++;
        if (index >= _passengers.Count)
            index = 0;

        //If wanted passenger spawned/not available it returns null 
        //I spawned a lot of passengers on list so it wont be a problem but it is defence mechanism
        if (!currentPassenger.IsAvailable)
            return null;

        return currentPassenger;
    }
}
