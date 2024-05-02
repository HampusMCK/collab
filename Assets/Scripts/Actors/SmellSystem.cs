using System;
using UnityEngine;

public class SmellSystem : MonoBehaviour
{
    //Odours are measured via the human nose and are ranked on a scale of 0-6, with 0 being no odour and 6 being intolerable.
    [Range(0,6)]
    public int odour = 0;
}
