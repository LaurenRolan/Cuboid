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
    private const int FramesCount = 50;
    private const float AnimationTimeSeconds = 2;
    private float _deltaTime = AnimationTimeSeconds/FramesCount;
    private float _dx = (targetScaleWidth - initialScale)/FramesCount;
    private float _dy = (targetScaleHeight - initialScale)/FramesCount;
    [SerializeField] List<GameObject> up = new List<GameObject>();
    [SerializeField] List<GameObject> center = new List<GameObject>();
    [SerializeField] List<GameObject> player = new List<GameObject>();
    bool input = true;
    bool spliting = false;

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
                if(!spliting){
                    spliting = true;
                    StartCoroutine("splitAnimation");
                }
            }
        }
    }

    void duplicateObject(List<GameObject> gameObject) {
        /* For each present cube : 
         *        create a new cube
         *        move the new cube to the right
         *        move the old cube to the left
         */
        for(int i = 0; i < instances; i++) {
            gameObject[i].transform.localScale = new Vector3(1.0f/(instances*2), 
                                                             1.0f/(instances*2), 
                                                             1.0f/(instances*2));
            gameObject[i].transform.Translate(gameObject[i].transform.position.x - 1.0f/(instances*2), 
                                              gameObject[i].transform.position.y/2,
                                              gameObject[i].transform.position.z - 1.0f/(instances*2));
            Debug.Log("Cube " + i + " has (x, y,z) = (" + gameObject[i].transform.position.x + ", " + gameObject[i].transform.position.y
                        + ", " + gameObject[i].transform.position.z + ")");
            gameObject.Add(Instantiate(gameObject[i], new Vector3(gameObject[i].transform.position.x + 2.0f/(instances*2),
                                                                  gameObject[i].transform.position.y,
                                                                  gameObject[i].transform.position.z),
                                                                  Quaternion.identity));
        }
    }


    void duplicateCube() {
        duplicateObject(player);
        duplicateObject(up);
        duplicateObject(center);

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
                
                yield return new WaitForSeconds(_deltaTime);
            }
        }
        if(instances < 4)
            duplicateCube();
        spliting = false;
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
