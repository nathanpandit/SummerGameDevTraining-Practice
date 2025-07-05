using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public static int currentLevel;
    
    void Start()
    {
        currentLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
