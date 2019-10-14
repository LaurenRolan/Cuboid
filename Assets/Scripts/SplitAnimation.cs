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
            player[i * 2].transform.localScale = new Vector3(1.0f/(instances+1), 1.0f/(instances+1), 1.0f/(instances+1));
            player[i * 2].transform.Translate(- 1.0f/(instances*2), 
                                              0,
                                              0);
        }
        for(int i = 1; i < instances * 2; i+=2) {
            player[i] = Instantiate(player[i-1], new Vector3(1.0f/(instances*2), -1.0f/(instances*2+2), player[i-1].transform.position.z), Quaternion.identity); //change vector
        }
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
                    
                player[j].transform.localScale = new Vector3(_currentScaleX,_currentScaleY,_currentScaleY);
                if(!done)
                    player[j].transform.Translate( new Vector3(0, _dy/2, 0) );
                //player[j].transform.Translate(player[j].transform.position.x, player[j].transform.position.y, player[j].transform.position.z + _dy/2);
                yield return new WaitForSeconds(_deltaTime);
            }
        }
        input = true;
        duplicateCube();
    }
}
