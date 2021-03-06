using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMaps : MonoBehaviour
{


    public GameObject map;
    [Header("For ship prefabs")]
    public GameObject smallShip;
    public GameObject mediumShip;
    public GameObject largeShip;
    
    
    public int numLayers = 10;
    [Tooltip("The fix station that has update")]
    public int upGap = 3;
    [Tooltip("The fix station that has shop")]
    public int difficultyUpTimes = 2;
    [HideInInspector]
    public int[] difficultyUpIndex;

    //might not be of much use, just record keeping
    private int[] mapsEachLayer;

    [Header("how many maps per layer")]
    public int maxMapPerLayer = 4;
    public int minMapPerLayer = 2;


    [Header("Ship number bounds")]
    public int minLargeShip = 0;
    public int minMediumShip = 1;
    public int minSmallShip = 2;

    
    public int maxLargeShip = 1;
    public int maxMediumShip = 2;
    public int maxSmallShip = 4;

    [Header("Ship reward")]
    public int rewardSmallShip = 100;
    public int rewardMediumShip = 500;
    public int rewardLargeShip = 1000;


    

    // 2D array used to store all map class instances.
    public GameObject[][] layers;
    private int currDifficulty = 0;

    [Header("Chance to turn a map to shop or up station")]
    [Tooltip("Choose to have random number of upstation through out map")]
    public bool randUpStation = true;
    public bool randShop = true;
    [Tooltip("Randomly chance to make a map shop or upgrade station")]
    public float isShopProbability = 0.2f;
    public float isUpProbability = 0.2f; 

    private int lastNumber;
    
    // Start is called before the first frame update
    void Start()
    {
        layers = new GameObject[numLayers][];
        mapsEachLayer = new int[numLayers];
        difficultyUpIndex = new int[difficultyUpTimes];

        for (int i = 0; i < difficultyUpIndex.Length; i++)
        {
            int section = numLayers / difficultyUpTimes;
            difficultyUpIndex[i] = UnityEngine.Random.Range(section * i, section * (i + 1));
            
            //Debug.Log("Difficulty up index is: " + difficultyUpIndex[i]);
        }

        for (int i = 0; i < numLayers; i++)
        {
            int mapInLayer = UnityEngine.Random.Range(minMapPerLayer, maxMapPerLayer);
            
            mapsEachLayer[i] = mapInLayer;
            layers[i] = new GameObject[mapInLayer];
        }

        for (int i = 0; i < numLayers; i++)
        {
            //used to increment difficulty level
            for (int j = 0; j < difficultyUpIndex.Length; j++)
            {
                if (i == difficultyUpIndex[j])
                {
                    currDifficulty += 1;
                }
            }

            //hard coded shop and upgrade location
            if (i % upGap == 0)
            {
                //Debug.Log("rewrite layer: " + i);
                layers[i] = new GameObject[1];
                layers[i][0] = Instantiate(map);
                layers[i][0].GetComponent<Map>().isShop = true;
                layers[i][0].GetComponent<Map>().layerNum = i;
            }
            
            else {
                for (int j = 0; j < layers[i].Length; j++)
                {
                    
                    layers[i][j] = Instantiate(map);
                    if (randShop && Random.value > (1 - isShopProbability))
                    {
                        layers[i][j].GetComponent<Map>().isShop = true;
                        layers[i][j].GetComponent<Map>().layerNum = i;
                    }
                    else if (randUpStation && Random.value > (1 - isUpProbability))
                    {
                        layers[i][j].GetComponent<Map>().isUpgradeStation = true;
                        layers[i][j].GetComponent<Map>().layerNum = i;
                    }
                    else
                    {
                        layers[i][j].GetComponent<Map>().difficultyLevel = currDifficulty;

                        int numSmall = UnityEngine.Random.Range(minSmallShip, maxSmallShip);
                        int numMid = UnityEngine.Random.Range(minMediumShip, maxMediumShip);
                        int numLarge = UnityEngine.Random.Range(minLargeShip, maxLargeShip);
                        layers[i][j].GetComponent<Map>().smallShip = numSmall;
                        layers[i][j].GetComponent<Map>().mediumShip = numMid;
                        layers[i][j].GetComponent<Map>().largeShip = numLarge;

                        layers[i][j].GetComponent<Map>().moneyReward = numSmall * rewardSmallShip + numMid * rewardMediumShip +
                            numLarge * rewardLargeShip;
                        layers[i][j].GetComponent<Map>().healthReward = (int)(layers[i][j].GetComponent<Map>().moneyReward / 10);
                        layers[i][j].GetComponent<Map>().layerNum = i;
                    }
                    
                }
            }
        }
        //Debug.Log("layer 7, map 2's enemy: " + layers[6][1].GetComponent<map>().moneyReward);

        
        
    }
    int GetRandom(int min, int max)
    {
        int rand = Random.Range(min, max);
        while (rand == lastNumber)
            rand = Random.Range(min, max);
        lastNumber = rand;
        return rand;
    }

}
