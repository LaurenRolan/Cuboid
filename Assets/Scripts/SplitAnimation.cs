using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitAnimation : MonoBehaviour
{
    public bool input = true;
    public int instances;
    private const float targetScaleWidth = 1.5f;
    private const float targetScaleHeight = 0.5f;
    private const float initialScale = 1f;
    private float _currentScaleX = initialScale;
    private float _currentScaleY = initialScale;
    private const int FramesCount = 100;
    private const float AnimationTimeSeconds = 2;
    private float _deltaTime = AnimationTimeSeconds / FramesCount;
    private float _dx = (targetScaleWidth - initialScale) / FramesCount;
    private float _dy = (targetScaleHeight - initialScale) / FramesCount;
    [SerializeField] GameObject[] player;
    // Start is called before the first frame update
    void Start()
    {
        instances = 1;
    }

    void Update()
    {
        if(input == true) {
            if(Input.GetKey(KeyCode.S)) {
                StartCoroutine("splitAnimation");
                input = false;
            }
        }
    }

    void duplicateCube() {
        for(int i = 0; i < instances; i++) {
            player[i * 2].transform.localScale = new Vector3(1/(instances+1), 1/(instances+1), 1/(instances+1));
        }
        for(int i = 1; i < instances * 2; i+=2) {
            player[i] = Instantiate(player[i-1], new Vector3(1/(instances+1), 1/(instances+1), 1/(instances+1)), Quaternion.identity); //change vector
        }
        instances *= 2;
    }
    IEnumerator splitAnimation() {
        bool done = false;
        Debug.Log("dx " + _dx);
        Debug.Log("dy " + _dy); 
        while(!done) {
            Debug.Log("In while");
            for(int j = 0; j < instances; j++) {
                _currentScaleX += _dx;
                _currentScaleY += _dy;
                if (_currentScaleX > targetScaleWidth) {
                    _currentScaleX = targetScaleWidth;
                    done = true;
                    Debug.Log("Got to x target scale");
                }
                if (_currentScaleY < targetScaleHeight) {
                    _currentScaleY = targetScaleHeight;
                    done = done && true;
                    Debug.Log("Got to y target scale");
                }
                    
                player[j].transform.localScale = new Vector3(_currentScaleX,_currentScaleY,_currentScaleY);
                yield return new WaitForSeconds(_deltaTime);
            }
        }
        duplicateCube();
        input = true;
    }
}
