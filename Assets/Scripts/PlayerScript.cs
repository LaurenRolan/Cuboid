using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = (float) 0.01;
    public int instances = 1;
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
            //else if(Input.GetKey(KeyCode.S)) {
            //    StartCoroutine("splitAnimation");
            //    input = false;
            //}
        }
    }
    IEnumerator splitAnimation() {
        for(int j = 0; j < instances; j++) {
            _currentScaleX += _dx;
            _currentScaleY += _dy;
            if (_currentScaleX > targetScaleWidth)
                _currentScaleX = targetScaleWidth;
            if(_currentScaleY < targetScaleHeight)
                _currentScaleY = targetScaleHeight;
            //player[j].transform.localScale = new Vector3(1,0,0) * _currentScaleX;
            //player[j].transform.localScale = new Vector3(0,1,0) * _currentScaleY;
            yield return new WaitForSeconds(_deltaTime);
        }
        input = true;
    }
    IEnumerator rotateForward(){
        //for(int j = 0; j < instances; j++) {
            for(int i = 0; i < 90 / step; i++) {
                player.transform.RotateAround(up.transform.position, Vector3.right, step);
                yield return new WaitForSeconds(speed);
            }
            center.transform.position = player.transform.position;
        //}
        input = true;
    }
}
