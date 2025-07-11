using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    // Start is called before the first frame update
    public Renderer ufoRenderer;
    public Vector3 originalPos;
    void Awake()

    {
        ufoRenderer = GetComponentInChildren<Renderer>();
    }

    public bool IsSamePosition()
    {
        return true;
    }

    public void ResetPosition()
    {
        transform.position = originalPos;
    }
}
