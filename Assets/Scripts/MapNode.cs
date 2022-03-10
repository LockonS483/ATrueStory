using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapNode : MonoBehaviour
{
    public enum NodeType{Battle, Shop, Upgrade}
    public NodeType nodeType;

    public int mapLayer;
    public int difficulty;
    public int lShips;
    public int mShips;
    public int sShips;
    public int reward;

    private GameObject selectRing;
    private GameObject infoCanvas;

    private Text infoText;

    public bool isClickable;

    public Color oc;
    public Color visitedColor;
    Renderer r;

    public bool visited;

    // Start is called before the first frame update
    void Awake()
    {
        r = gameObject.GetComponent<Renderer>();
        isClickable = false;
        selectRing = gameObject.transform.GetChild(0).gameObject;
        selectRing.SetActive(false);
        infoCanvas = gameObject.transform.GetChild(1).gameObject;
        infoText = infoCanvas.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        infoCanvas.SetActive(false);
        visited = false;
    }

    // Update is called once per frame
    void Update()
    {

        if(visited){
            r.material.SetColor("_Color", visitedColor);
            infoText.text = "";
            isClickable = false;
            selectRing.SetActive(false);
            return;
        }
        if (GlobalVars.currentLayer == mapLayer){
            isClickable = true;
        } else {
            isClickable = false;
        }
        if(!isClickable){
            selectRing.SetActive(false);
            oc = r.material.GetColor("_Color");
            oc.a = 0.45f;
            r.material.SetColor("_Color", oc);
        }else{
            oc = r.material.GetColor("_Color");
            oc.a = 1f;
            r.material.SetColor("_Color", oc);
        }
    }

    void OnMouseDown(){
        if(isClickable){
            Debug.Log("I HAVE BEEN CLICKED");
            GameManager gm = GameObject.FindObjectsOfType<GameManager>()[0].GetComponent<GameManager>();
            print(gm.name);
            if(!gm.inCombat){
                gm.NodeClick(this);
            }
        }
    }

    void OnMouseOver(){
        if(isClickable){
            selectRing.SetActive(true);
            infoCanvas.SetActive(true);
            infoText.text = "Big Ship: " + lShips + "\n" + "Mid Ship: " + mShips + "\n" +
                "Sm Ship: " + sShips;
        }
    }

    void OnMouseExit(){
        if(isClickable){
            selectRing.SetActive(false);
            infoCanvas.SetActive(false);
        }
    }
}
