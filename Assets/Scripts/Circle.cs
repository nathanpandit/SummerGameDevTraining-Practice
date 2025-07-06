using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    // Start is called before the first frame update
    public Renderer circleRenderer;
    void Awake()

    {
        circleRenderer = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
