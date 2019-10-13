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
    public bool decay;
    private int nPlatforms;
    public GameObject[] currentPlatforms;
    public GameObject[] nextPlatforms;
    private static TileManager instance;

    private Stack<GameObject> top4 = new Stack<GameObject>();
    private Stack<GameObject> top2 = new Stack<GameObject>();
    private Stack<GameObject> top1 = new Stack<GameObject>();
    private Stack<GameObject> right4 = new Stack<GameObject>();
    private Stack<GameObject> right2 = new Stack<GameObject>();
    private Stack<GameObject> right1 = new Stack<GameObject>();
    private Stack<GameObject> left2 = new Stack<GameObject>();
    private Stack<GameObject> left1 = new Stack<GameObject>();

    // Start is called before the first frame update

    public static TileManager Instance {
        get {
            if(instance == null)
                instance = GameObject.FindObjectOfType<TileManager>();
            return instance;
            }
    }

    void Start()
    {
        state = 0;
        decay = false;
        nextPlatforms = (GameObject[]) currentPlatforms.Clone();
        nPlatforms = 1;
        SpawnPlatform(topPlatforms[0], currentPlatforms[0].transform.GetChild(0).transform.GetChild(0).position, 0);
        currentPlatforms = (GameObject[]) nextPlatforms.Clone();
        SpawnPlatform(topPlatforms[0], currentPlatforms[0].transform.GetChild(0).transform.GetChild(0).position, 0);
        currentPlatforms = (GameObject[]) nextPlatforms.Clone();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateTiles(int amount) {
        for(int i = 0; i < amount; i++) {
            top4.Push(Instantiate(topPlatforms[0]));
            top4.Peek().SetActive(false);
            top2.Push(Instantiate(topPlatforms[1]));
            top2.Peek().SetActive(false);
            top1.Push(Instantiate(topPlatforms[2]));
            top1.Peek().SetActive(false);

            right4.Push(Instantiate(rightPlatforms[0]));
            right4.Peek().SetActive(false);
            right2.Push(Instantiate(rightPlatforms[1]));
            right2.Peek().SetActive(false);
            right1.Push(Instantiate(rightPlatforms[2]));
            right1.Peek().SetActive(false);

            left2.Push(Instantiate(leftPlatforms[0]));
            left2.Peek().SetActive(false);
            left1.Push(Instantiate(leftPlatforms[1]));
            left1.Peek().SetActive(false);
        }
    }

    Vector3 getPosition(GameObject current, string attach) {
        if(attach.Equals("top")) return current.transform.GetChild(0).transform.GetChild(0).position;
        if(attach.Equals("left")) return current.transform.GetChild(0).transform.GetChild(1).position;
        if(attach.Equals("right")) return current.transform.GetChild(0).transform.GetChild(2).position;
        return Vector3.zero;
    }

    GameObject getPlatform(string attach, int index) {
        if(attach.Equals("top")) {
            if(index == 0) return top4.Pop();
            if(index == 1) return top2.Pop();
            if(index == 2) return top1.Pop();
        }
        if(attach.Equals("left")) {
            if(index == 0) return left2.Pop();
            if(index == 1) return left1.Pop();
        } 
        if(attach.Equals("right")) {
            if(index == 0) return right4.Pop();
            if(index == 1) return right2.Pop();
            if(index == 2) return right1.Pop();
        }
        return null;
    }

    public void newLine() {
        StateMachine();
        Debug.Log("State " + state + "   Decay " + decay + "     NPlatforms " + nPlatforms);
        int loopMax = nPlatforms;
        for(int i = 0; i < loopMax; i++) {
            if((!decay && state % 2 == 0) || state == 0) {
                InvokePlatform(state/2, "top", i);
                Debug.Log("Top of " + i);
            }
            else if(state % 2 == 1) {
                if(decay && i % 2 == 0) {
                    InvokePlatform((state-1)/2, "right", i);
                    nPlatforms--;
                } else if(!decay) {
                    InvokePlatform((state+1)/2, "right", i);
                    InvokePlatform((state-1)/2, "left", i);
                    nPlatforms++;
                }
            }
        }
        currentPlatforms = (GameObject[]) nextPlatforms.Clone();
    }

    public void InvokePlatform(int index, string position, int i) {
        if(top4.Count == 0   || top2.Count == 0   || top1.Count == 0   ||
           right4.Count == 0 || right2.Count == 0 || right1.Count == 0 ||
           left2.Count == 0  || left1.Count == 0 ) {
            CreateTiles(10);
        }
        GameObject platform = getPlatform(position, index);
        platform.SetActive(true);
        Vector3 attach = getPosition(currentPlatforms[i], position);
        platform.transform.position = attach;
        if(position.Equals("left")) nextPlatforms[i + 1] = platform;
        else nextPlatforms[i] = platform;
    }

    public void SpawnPlatform(GameObject newPlatform, Vector3 attachPoint, int i) {
        nextPlatforms[i] = Instantiate(newPlatform, attachPoint, Quaternion.identity);
    }

    private void StateMachine(){
        int nextState = 0;
        switch(state){
            case 0: //State where there is only a top4 
                nextState = Random.Range(0,2);
                if(nextState == 1) state++;
                decay = false;
                break;
            case 2: // State where there are 2 platforms type 2
                nextState = Random.Range(0,3);
                if(nextState == 1) {state++; decay = false;}
                else if(nextState == 2) {state--; decay = true;}
                break;            
            case 1: //State of transition
            case 3: //State of transition
                if(decay) state--;
                else {state++; decay = false;}
                break;
            case 4: //State where there are 4 platforms type4
                nextState = Random.Range(0,2);
                if(nextState == 1) {state--; decay = true;}
                else decay = false;
                break;
        }
    }
}
