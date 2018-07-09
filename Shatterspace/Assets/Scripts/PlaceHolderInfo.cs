using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolderInfo : MonoBehaviour {
private bool empty = true;

    public bool Empty
    {
        get
        {
            return empty;
        }

        set
        {
            empty = value;
        }
    }
}
