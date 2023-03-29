using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class BusStation : MonoBehaviour
{
    [SerializeField] private List<Transform> _passengersWaitPoints;
    [SerializeField] private BusStation _nextStation;
    [SerializeField] private PathCreator _path;
    //I made this to make some bus stations has different amount passengers
    [SerializeField] [Range(1,3)] private int _desiredPassengerCount = 1;
    [SerializeField] private float _respawnTime;
    [SerializeField] private bool _isLastStation;

    private List<Passenger> passengers = new List<Passenger>();
    private int waitPointIndex;

    public float PathDistance;

    //This bool is temp.Using for tests
    public bool CanSpawnPassengers;
    private void Start()
    {
        //I used path creator to get bus stations distance on path.With this way can calculate the exact distance between stations on the bus route/path
        PathDistance = _path.path.GetClosestDistanceAlongPath(transform.position);

        if (!CanSpawnPassengers)
            return;

        SpawnPassenger();
    }
    private void SpawnPassenger()
    {
        for (int i = 0; i < _desiredPassengerCount; i++)
        {
            Passenger passenger = PassengerObjectPooling.Instance.GetPassenger();

            if (passenger == null)
                return;

            passenger.Spawn(_passengersWaitPoints[waitPointIndex]);
            passengers.Add(passenger);
            waitPointIndex++;
        }
        waitPointIndex = 0;
    }

    private float CalculateTimer()
    {
        //There is the calculation function
        if (_isLastStation)
        {
           float shouldBeValue = _path.path.length - PathDistance;
            return _nextStation.PathDistance + shouldBeValue;
        }
        return _nextStation.PathDistance - PathDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.TryGetComponent(out BusManager bus))
        {
            //This is a defence mechanism if bus trigger to same station or other stations
            if (bus.TargetBusStation != this)
                return;

            bus.PassengersDisembarking();
            bus.SetDestination(_nextStation);
            bus.SetCarCanMove(false);
            bus.SetDoorsState(true);
            bus.AddPassenger(passengers);

            if (CanSpawnPassengers)
                bus.SetTimer(CalculateTimer());

            passengers.Clear();

            //Spawn passengers again to make game endless
            Invoke(nameof(SpawnPassenger), _respawnTime);
        }
    }
}
