using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public MapNode mapNodePrefab;
    [Header("Map Generation Settings")]
    public int numLayers;
    public int upGap = 4;
    public int minNodesInLayer;
    public int maxNodesInLayer;
    public MapLayer[] mapLayers;

    [Header("Probability Settings")]
    public float shopWeight;
    public float battleWeight;
    public float upgradeWeight;

    //colors
    [Header("Colors")]
    public Color battleColor;
    public Color upgradeColor;
    public Color shopColor;

    public float layerGap;

    [Header("Battle Node Config")]
    public int difficultyGap;
    int sScore = 3;
    int mScore = 4;
    int lScore = 7;
    public float smallShipWeight;
    public float mediumShipWeight;
    public float largeShipWeight;

    //map generating variables
    int cDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    void GenerateMap(){
        mapLayers = new MapLayer[numLayers];
        cDifficulty = 1;
        for(int i=0; i<numLayers; i++){
            GenerateLayer(i);
        }

        //adjust positions of stars
        for(int j=0; j<numLayers; j++){
            Vector3 layerPos = Vector3.zero;
            float startingx = j*layerGap;
            float startingy = 0;
            startingy += 3.75f * (mapLayers[j].mapNodes.Length - 1) + (Random.Range(-0.5f, 0.5f));
            layerPos.y = startingy;
            mapLayers[j].transform.localPosition = Vector3.zero;
            cDifficulty=1;
            for(int k=0; k<mapLayers[j].mapNodes.Length; k++){
                layerPos.x = startingx + Random.Range(-layerGap/5, layerGap/5);
                layerPos.y -= Random.Range(5f, 10f);
                mapLayers[j].mapNodes[k].transform.localPosition = layerPos;
            }
        }
    }

    void GenerateLayer(int l){
        //special overrides
        string lName = "MapLayer" + (l.ToString());
        mapLayers[l] = (new GameObject(lName, typeof(MapLayer))).GetComponent<MapLayer>();
        mapLayers[l].transform.parent = transform;
        if(l % difficultyGap == 0 && l!=0){
            cDifficulty ++;
        }
        if(l % upGap == 0 && l!=0){
            //is a single upgrade station
            mapLayers[l].mapNodes = new MapNode[1];
            GenerateUpgradeNode(mapLayers[l], 0, l);
        }else if(l == (numLayers-1)){
            //is a single final battle node
            mapLayers[l].mapNodes = new MapNode[1];
            GenerateBattleNode(mapLayers[l], 0, l);
        }else{
            float gentotal = battleWeight + shopWeight + upgradeWeight;
            //normal layer
            int numStars = Random.Range(minNodesInLayer, maxNodesInLayer+1);
            mapLayers[l].mapNodes = new MapNode[numStars];
            for(int i=0; i<numStars; i++){
                float typeGen = Random.Range(0f, gentotal);
                if(typeGen <= battleWeight){
                    //battlenode
                    GenerateBattleNode(mapLayers[l], i, l);
                }else if(typeGen < battleWeight+shopWeight){
                    //shopnnode
                    GenerateShopNode(mapLayers[l], i, l);
                }else{
                    //upgradenode
                    GenerateUpgradeNode(mapLayers[l], i, l);
                }
            }
        }
    }

    void GenerateUpgradeNode(MapLayer parentLayer, int layerIndex, int thisLayer){
        parentLayer.mapNodes[layerIndex] = Instantiate(mapNodePrefab);
        parentLayer.mapNodes[layerIndex].transform.parent = parentLayer.transform;
        parentLayer.mapNodes[layerIndex].nodeType = MapNode.NodeType.Upgrade;
        parentLayer.mapNodes[layerIndex].GetComponent<Renderer>().material.color = upgradeColor;
        parentLayer.mapNodes[layerIndex].GetComponent<MapNode>().mapLayer = thisLayer;
    }
    void GenerateBattleNode(MapLayer parentLayer, int layerIndex, int thisLayer){
        parentLayer.mapNodes[layerIndex] = Instantiate(mapNodePrefab);
        parentLayer.mapNodes[layerIndex].transform.parent = parentLayer.transform;
        parentLayer.mapNodes[layerIndex].nodeType = MapNode.NodeType.Battle;
        parentLayer.mapNodes[layerIndex].GetComponent<Renderer>().material.color = battleColor;
        parentLayer.mapNodes[layerIndex].GetComponent<MapNode>().mapLayer = thisLayer;
        
        int battleScore = 0;
        int targetScore = Mathf.FloorToInt(cDifficulty * 8f);
        print("generating " + cDifficulty.ToString());
        float gentotal = smallShipWeight + mediumShipWeight + largeShipWeight;
        while(battleScore < targetScore){
            float typeGen = Random.Range(0f, gentotal);
            if(typeGen < smallShipWeight){
                parentLayer.mapNodes[layerIndex].sShips++;
                battleScore += sScore;
            }else if(typeGen < smallShipWeight + mediumShipWeight){
                parentLayer.mapNodes[layerIndex].mShips++;
                battleScore += mScore;
            }else{
                parentLayer.mapNodes[layerIndex].lShips++;
                battleScore += lScore;
            }
        }
    }
    void GenerateShopNode(MapLayer parentLayer, int layerIndex, int thisLayer){
        parentLayer.mapNodes[layerIndex] = Instantiate(mapNodePrefab);
        parentLayer.mapNodes[layerIndex].transform.parent = parentLayer.transform;
        parentLayer.mapNodes[layerIndex].nodeType = MapNode.NodeType.Shop;
        parentLayer.mapNodes[layerIndex].GetComponent<Renderer>().material.color = shopColor;
        parentLayer.mapNodes[layerIndex].GetComponent<MapNode>().mapLayer = thisLayer;
    }
}
