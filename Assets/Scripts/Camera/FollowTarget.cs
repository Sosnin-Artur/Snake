using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;    
    [SerializeField] private Vector3 offset;   

    void LateUpdate()
    {
        transform.position = new Vector3
            (transform.position.x + offset.x, transform.position.y + offset.y, target.position.z + offset.z);    
    }
}
