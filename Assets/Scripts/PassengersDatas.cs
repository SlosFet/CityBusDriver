using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PassengersDatas : ScriptableObject
{
    [field: SerializeField] public float Speed { get; private set; }
    [Tooltip("When player arrive to bus station late , Players money decrease this much")]
    [field: SerializeField] [field: Range(-500,0)] public int Cost { get; private set; }
    [Tooltip("When player arrive to bus station early or exact time , Players money increase this much")]
    [field: SerializeField] [field: Range(0,500)] public int Prize { get; private set; }
}
