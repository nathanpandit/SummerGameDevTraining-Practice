using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    // Start is called before the first frame update
    public Renderer ufoRenderer;
    public bool isPlaced;
    public Vector2 maxPos, minPos;
    void Awake()

    {
        ufoRenderer = GetComponentInChildren<Renderer>();
    }

    public bool IsSamePosition()
    {
        return true;
    }
}
