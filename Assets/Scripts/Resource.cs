using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] static private string name;
    [SerializeField] static private float count;
    [SerializeField] static private float cost;
    [SerializeField] static private Sprite icon;

    static void SellResource(int countSell)
    {
        count -= countSell;
    }
}
