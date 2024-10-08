using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : Actuator
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActivateMechanism();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        DeactivateMechanism();
    }
}
