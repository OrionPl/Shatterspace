using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolderInfo : MonoBehaviour {

    private GameObject placeholderObject;
    public GameObject PlaceholderObject
    {
        get
        {
            return placeholderObject;
        }

        set
        {
            placeholderObject = value;
        }
    }

    private bool empty;
    public bool Empty
    {
        get
        {
            return empty;
        }

        set
        {
            empty = value;
            if (value)
                placeholderObject = null;
        }
    }

    private void Start()
    {
        Empty = true;
    }
}
