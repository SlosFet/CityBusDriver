using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusDoorsAnim : MonoBehaviour
{
    [SerializeField] private Animation _doors;

    /// <summary>
    /// True for Open , False for Close
    /// </summary>
    /// <param name="state"></param>
    public void OpenCloseDoors(bool state)
    {
        if (state)
            _doors.Play("Open");
        else
            _doors.Play("Close");
    }
}
