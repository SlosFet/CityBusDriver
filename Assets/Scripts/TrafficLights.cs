using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TrafficLights : MonoBehaviour
{
    public TrafficLightState TrafficLight;
    //Should be a negative amount to decrease players money
    [Range(-500,0)] public int PenaltyAmount;

    public float StayOnRedTime;
    public float StayOnGreenTime;
    public Image LightImage;
    public TextMeshProUGUI LightStateText;

    public TrafficLightDetector detector;
    private void Start()
    {
        StartCoroutine(ChangeLightState());
    }
    private void OnTriggerEnter(Collider other)
    {
        //If it is green or trafficLightDetector hasnt triggered with a bus
        if (TrafficLight == TrafficLightState.Green || detector.Bus == null)
            return;

        PlayerData.MoneyChangeEvent.Invoke(PenaltyAmount);
    }

    //Basic light changer
    IEnumerator ChangeLightState()
    {
        while(true)
        {
            TrafficLight = TrafficLightState.Green;
            LightImage.color = Color.green;
            LightStateText.text = "YOU CAN GO";

            yield return new WaitForSeconds(StayOnGreenTime);

            TrafficLight = TrafficLightState.Red;
            LightImage.color = Color.red;
            LightStateText.text = "YOU MUST STOP";

            yield return new WaitForSeconds(StayOnRedTime);
        }
    }
}

public enum TrafficLightState
{
    Red,
    Green
}
