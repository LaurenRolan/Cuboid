using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    #region Prefabs
    public GameObject[] topPlatforms;
    public GameObject[] rightPlatforms;
    public GameObject[] leftPlatforms;
    #endregion
    
    private int state;
    private bool decay;
    private bool keep;
    private int nPlatforms;
    public GameObject[] currentPlatforms;
    public GameObject[] nextPlatforms;
    // Start is called before the first frame update
    void Start()
    {
        state = 0;
        decay = false;
        keep = true;
        nextPlatforms = currentPlatforms;
        nPlatforms = 1;
        SpawnPlatform(topPlatforms[0], currentPlatforms[0].transform.GetChild(0).transform.GetChild(0).position, 0);
        currentPlatforms = nextPlatforms;
        SpawnPlatform(topPlatforms[0], currentPlatforms[0].transform.GetChild(0).transform.GetChild(0).position, 0);
        currentPlatforms = nextPlatforms;
        InvokeRepeating("newLine", 2.0f, 4.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newLine() {
        StateMachine();
        Debug.Log("State " + state);
        int loopMax = nPlatforms;
        for(int i = 0; i < loopMax; i++) {
            GameObject platform = currentPlatforms[i];
            Vector3 topAttach = platform.transform.GetChild(0).transform.GetChild(0).position;
            Vector3 leftTopAttach = platform.transform.GetChild(0).transform.GetChild(1).position;
            Vector3 rightTopAttach = platform.transform.GetChild(0).transform.GetChild(2).position;
            Vector3 leftBottomAttach = platform.transform.GetChild(0).transform.GetChild(3).position;
            Vector3 rightBottomAttach = platform.transform.GetChild(0).transform.GetChild(4).position;
            if(!decay && state % 2 == 0) {
                Debug.Log("State " + state + "     Loop " + i + "     TopPlatform " + state/2);
                SpawnPlatform(topPlatforms[state/2], topAttach, i);
            }
            else {
                if(decay && i % 2 == 0) {
                    Debug.Log("State " + state + "     Loop " + i + "     Decay to " + (state-1)/2);
                    SpawnPlatform(rightPlatforms[(state-1)/2], rightTopAttach, i / 2);
                    nPlatforms--;
                } else if(!decay) {
                    Debug.Log("State " + state + "     Loop " + i + "     Increase to " + (state-1)/2);
                    SpawnPlatform(rightPlatforms[(state+1)/2], rightTopAttach, i * 2 + 1);
                    SpawnPlatform(leftPlatforms[(state-1)/2], leftTopAttach, i * 2);
                    nPlatforms++;
                }
            }
        }
        currentPlatforms = nextPlatforms;
    }

    public void SpawnPlatform(GameObject newPlatform, Vector3 attachPoint, int i) {
        nextPlatforms[i] = Instantiate(newPlatform, attachPoint, Quaternion.identity);
    }

    private void StateMachine(){
        int nextState = 0;
        switch(state){
            case 0:
                nextState = Random.Range(0,2);
                if(nextState == 1) state++;
                decay = false;
                break;
            case 2:
                nextState = Random.Range(0,3);
                if(nextState == 1) {state++; decay = false;}
                else if(nextState == 2) {state--; decay = true;}
                break;            
            case 1:
            case 3:
                if(decay) state--;
                else {state++; decay = false;}
                break;
            case 4:
                nextState = Random.Range(0,2);
                if(nextState == 1) {state--; decay = true;}
                else decay = false;
                break;
        }
    }
}
