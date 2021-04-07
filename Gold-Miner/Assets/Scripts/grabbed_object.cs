using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabbed_object : MonoBehaviour
{
    enum object_type
    {
        ore,
        stone,
        diamond,
        dynamite,
        money_bag
    }
    [SerializeField] private object_type type;
    [SerializeField] private float weight = 1;
    [SerializeField] private int price = 100;
    [SerializeField] private string description="";

}
