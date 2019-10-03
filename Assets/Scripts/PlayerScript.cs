using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = (float) 0.01;
    public int instances = 1;
    public int step = 5;
    [SerializeField] GameObject up;
    [SerializeField] GameObject center;
    [SerializeField] GameObject player;
    bool input = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(input == true) {
            if(Input.GetKey(KeyCode.UpArrow)) {
               StartCoroutine("rotateForward");
                input = false;
            }
        }
    }
    IEnumerator rotateForward(){
         for(int i = 0; i < 90 / step; i++) {
                player.transform.RotateAround(up.transform.position, Vector3.right, step);
                yield return new WaitForSeconds(speed);
            }
        center.transform.position = player.transform.position;
        input = true;
    }
}
