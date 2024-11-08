using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transport : MonoBehaviour
{
    public Transform targetPosition;

    public void OnTriggerStay(Collider hit)
    {
        if(hit.CompareTag("Player"))
        {
            hit.transform.position = targetPosition.position;
        }
                
                
    
    }
}
