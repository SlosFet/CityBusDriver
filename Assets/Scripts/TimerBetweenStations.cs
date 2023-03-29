using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimerBetweenStations : MonoBehaviour
{
    public float Timer { get; private set; }
    [SerializeField] private float _distanceMultiplier;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Animation _timerHurryUpAnim;
    private Coroutine timerCoroutine;
    private void Start()
    {
        Timer = 0;
        UpdateTimerDisplay(Timer);

    }
    /// <summary>
    /// Function will convert distance to time
    /// </summary>
    /// <param name="distance"></param>
    public void SetTimer(float distance)
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        StopAnimation();

        //Distance usually too much for time so it is decreasing or increasing by a exact value
        Timer = distance * _distanceMultiplier;
        //Timer shouldnt be negative
        Timer = Mathf.Abs(Timer);
        UpdateTimerDisplay(Timer);
    }

    public void StartTimer()
    {
        StopAnimation();
        timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    private void StopAnimation()
    {
         if (_timerHurryUpAnim.isPlaying)
        {
            _timerHurryUpAnim.Stop();
            _timerText.transform.localScale = new Vector3(1, 1.1931f, 1);
}
    }

    private IEnumerator TimerCoroutine()
    {
        //Basic timer
        while (Timer >= 0f)
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer <= 0)
            {
                StopAnimation();
                UpdateTimerDisplay(0);
                yield break;
            }

            if (Timer <= 10 && !_timerHurryUpAnim.isPlaying)
                _timerHurryUpAnim.Play("Timer");

            UpdateTimerDisplay(Timer);
            yield return new WaitForFixedUpdate();

        }
    }

    private void UpdateTimerDisplay(float time)
    {
        //Setting time to minutes and seconds format
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        string currentTimer = string.Format("{00:00}{1:00}", minutes, seconds);
        _timerText.text = currentTimer[0].ToString() + currentTimer[1].ToString() + ":" + currentTimer[2].ToString() + currentTimer[3].ToString();

    }



}
