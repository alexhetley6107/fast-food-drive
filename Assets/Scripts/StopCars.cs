using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCars : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {       
        if (other.transform.CompareTag("Car") && CarControl.isLose == true)
        {            
            other.gameObject.GetComponent<Rigidbody>().constraints =
                RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;            
        }
        
    }
}
