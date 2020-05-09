using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public Transform mainCamera;
    public float killDepth = -10f;
	
	void Update ()
    {
        Vector3 newPosition = mainCamera.transform.position;
        newPosition.y = killDepth;
        this.transform.position = newPosition;
	}
}
