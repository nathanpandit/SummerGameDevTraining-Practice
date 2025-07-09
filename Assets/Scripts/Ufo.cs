using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    // Start is called before the first frame update
    public Renderer ufoRenderer;
    void Awake()

    {
        ufoRenderer = GetComponentInChildren<Renderer>();
    }
}
