using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Circle : MonoBehaviour
{
    // Start is called before the first frame update
    public Renderer circleRenderer;
    public GameObject highlight;
    void Awake()

    {
        circleRenderer = GetComponentInChildren<Renderer>();
        highlight = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        highlight.transform.position = transform.position + new Vector3(0f, 0.5f, 0f);
        highlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
