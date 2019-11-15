using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    void Start(){
        offset = new Vector3(0.0f, transform.position.y, transform.position.z);
    }
    void LateUpdate() {
       transform.position = player.transform.position + offset;
    }
}
