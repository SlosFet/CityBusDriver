using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusManager : MonoBehaviour
{
    [SerializeField] private Transform _entrancePoint;
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private BusDoorsAnim _busDoorsAnim;
    [SerializeField] private BusDriveController _busDriveController;
    [SerializeField] private BusPathGuidier _busPathGuidier;
    [SerializeField] private TimerBetweenStations _timerBetweenStations;

    private List<Passenger> passengers = new List<Passenger>();
    private List<Passenger> gonnaRemovePassengers = new List<Passenger>();
    private List<Passenger> gonnaAddPassengers = new List<Passenger>();

    private float arrivedTime;

    //Bools to move bus again
    private bool passengersBoarding;
    private bool passengersDisembarking;

    [field: SerializeField] public BusStation TargetBusStation { get; private set; }

    //When arrive bus station it works and busdrivecontroller ýnvoke InstantStop function
    public void SetCarCanMove(bool state) => _busDriveController.CanMove = state;

    //When arrive bus station doors open and when passengers jobs done it closes
    public void SetDoorsState(bool state) => _busDoorsAnim.OpenCloseDoors(state);

    //When arrive bus station timer sets 
    public void SetTimer(float distance) => _timerBetweenStations.SetTimer(distance);

    //Set a destination to next station to guide show player to correct path
    public void SetDestination(BusStation newTargetStation)
    {
        _busPathGuidier.SetDestination(newTargetStation.transform.position);
        TargetBusStation = newTargetStation;
    }

    //When bus move again timer starts
    private void StartTimer() => _timerBetweenStations.StartTimer();

    public void PassengersDisembarking()
    {
        //Hold the arrived time to send passengers
        arrivedTime = _timerBetweenStations.Timer;
        
        passengersDisembarking = false;
        gonnaRemovePassengers.Clear();
        //Adding passengers another list to doesnt make a error
        foreach (Passenger passenger in passengers)
        {
            gonnaRemovePassengers.Add(passenger);
        }

        passengers.Clear();
        StartCoroutine(PassengersDisembarkingCoroutine());
    }

    private IEnumerator PassengersDisembarkingCoroutine()
    {
        yield return new WaitForSeconds(1);

        foreach (Passenger passenger in gonnaRemovePassengers)
        {
            passenger.GetOffTheBus(_exitPoint, arrivedTime);
            yield return new WaitForSeconds(0.5f);
        }
        passengersDisembarking = true;
        gonnaRemovePassengers.Clear();
        CheckPassengersBoarding();
    }

    public void AddPassenger(List<Passenger> GonnaAddPassengers)
    {
        passengersBoarding = false;
        gonnaAddPassengers.Clear();
        //Adding another list because firstly passengers left the bus and clear passengers list then gonnaaddpassengers must be added on passengers list
        foreach (Passenger passenger in GonnaAddPassengers)
        {
            gonnaAddPassengers.Add(passenger);
        }
        StartCoroutine(AddPassengerCoroutines());
    }

    private IEnumerator AddPassengerCoroutines()
    {
        yield return new WaitForSeconds(1);
        foreach (Passenger passenger in gonnaAddPassengers)
        {
            passenger.GetOnTheBus(_entrancePoint);
            this.passengers.Add(passenger);
            yield return new WaitForSeconds(passenger.datas.Speed / 3);
        }

        //If is there any passengers it is waiting for him to board to bus
        if (gonnaAddPassengers.Count > 0)
            yield return new WaitForSeconds(gonnaAddPassengers[0].datas.Speed);

        gonnaAddPassengers.Clear();
        passengersBoarding = true;
        CheckPassengersBoarding();
    }

    private void CheckPassengersBoarding()
    {
        //If passengers add and leave jobs done bus can move again
        if (passengersBoarding && passengersDisembarking)
        {
            SetCarCanMove(true);
            SetDoorsState(false);
            StartTimer();
        }
    }
}
