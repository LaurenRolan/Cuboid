using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = (float) 0.01;
    public int instances;
    public int step = 5;
    private const float targetScaleWidth = 1.5f;
    private const float targetScaleHeight = 0.5f;
    private const float initialScale = 1f;
    private float _currentScaleX = initialScale;
    private float _currentScaleY = initialScale;
    private const int FramesCount = 100;
    private const float AnimationTimeSeconds = 2;
    private float _deltaTime = AnimationTimeSeconds/FramesCount;
    private float _dx = (targetScaleWidth - initialScale)/FramesCount;
    private float _dy = (targetScaleHeight - initialScale)/FramesCount;
    [SerializeField] GameObject[] up;
    [SerializeField] GameObject[] center;
    [SerializeField] GameObject[] player;
    bool input = true;

    // Start is called before the first frame update
    void Start()
    {
        instances = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(input == true) {
            if(Input.GetKey(KeyCode.UpArrow)) {
               StartCoroutine("rotateForward");
                input = false;
            }
            else if(Input.GetKey(KeyCode.S)) {
                StartCoroutine("splitAnimation");
                input = false;
            }
        }
    }
    void duplicateCube() {
        for(int i = 0; i < instances; i++) {
            player[i * 2].transform.localScale = new Vector3(1.0f/(instances+1), 1.0f/(instances+1), 1.0f/(instances+1));
            player[i * 2].transform.Translate(player[i*2].transform.position.x - 1.0f/((instances+1)*2), 
                                              player[i*2].transform.position.y,
                                              player[i*2].transform.position.z - 1.0f/((instances+1)*2));
        }
        //for(int i = 1; i < instances * 2; i+=2) {
        //    player[i] = Instantiate(player[i-1], new Vector3(1/(instances+1), 1/(instances+1), 1/(instances+1)), Quaternion.identity); //change vector
        //}
        instances *= 2;
    }
    IEnumerator splitAnimation() {
        bool done = false;
        while(!done) {
            for(int j = 0; j < instances; j++) {
                _currentScaleX += _dx;
                _currentScaleY += _dy;
                if (_currentScaleX > targetScaleWidth) {
                    _currentScaleX = targetScaleWidth;
                    done = true;
                }
                if (_currentScaleY < targetScaleHeight) {
                    _currentScaleY = targetScaleHeight;
                    done = done && true;
                }
                    
                player[j].transform.localScale = new Vector3(_currentScaleX, _currentScaleY, _currentScaleY);
                Vector3 player_pos = player[j].transform.position;
                //if(!done)
                //    player[j].transform.Translate( new Vector3(0, _dy, 0) );
                yield return new WaitForSeconds(_deltaTime);
            }
        }
        duplicateCube();
        input = true;
    }
    IEnumerator rotateForward(){
        Debug.Log("Entered with instances " + instances);
        for(int j = 0; j < instances; j++) {
            Debug.Log("j = " + j);
            for(int i = 0; i < 90 / step; i++) {
                player[j].transform.RotateAround(up[j].transform.position, Vector3.right, step);
                yield return new WaitForSeconds(speed);
            }
            center[j].transform.position = player[j].transform.position;
        }
        input = true;
    }
}
