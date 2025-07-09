using UnityEngine;
using Grid = UfoPuzzle.Grid;

public class UfoTrio : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        var slots = gameObject.GetComponentsInChildren<Grid>();
        int i = 0;
        foreach(Grid slot in slots)
        {
            slot.InitializeAsSlot(i);
            i++;
        }
    }
}
