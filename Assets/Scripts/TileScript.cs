using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    private float fallDelay = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            TileManager.Instance.newLine();
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            StartCoroutine(FallDown());
        }
    }

    IEnumerator FallDown() {
        yield return new WaitForSeconds(fallDelay);
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
