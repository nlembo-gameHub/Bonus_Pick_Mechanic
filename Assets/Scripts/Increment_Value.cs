using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Increment Settings")]
public class Increment_Value : ScriptableObject
{
    [SerializeField] public float value;
}
