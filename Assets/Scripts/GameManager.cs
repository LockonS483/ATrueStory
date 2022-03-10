using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform mapCam;
    public Transform battleCam;
    public Transform menuCam;
    public MapGenerator mapGen;

    public GameObject shopMenu;
    public GameObject upgradeMenu;
    public GameObject mapGuide;

    public bool inCombat;
    public bool inMenu = false;
    private char whichMenu = 'u'; // u = upgrade menu, s = shop menu

    public Ship[] enemyShips; //S, M, L ship prefabs.
    public Ship[] playerShips; //S, M, L ship prefabs.
    Vector3[] visitedNodes;
    Vector3 nodePosOffset = new Vector3(0, 0, 0.05f);

    int cEnemyCount;
    LineRenderer lr;

    public GameObject playerFleetObject;

    public int laserUp;
    public int missileUp;
    public int healthUp;

    [Header("Event stuff")]
    public GameObject[] eventPrefabs;
    public float eventChance;
    public Transform eventLocation;

    bool generateShop = true;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "GameController";
        inCombat = false;
        visitedNodes = new Vector3[10];
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Battle Node
        if(inCombat){
            StopAllCoroutines();
            upgradeMenu.SetActive(false);
            shopMenu.SetActive(false);
            mapGuide.SetActive(false);
            generateShop = true;

            transform.position = Vector3.Lerp(transform.position, battleCam.position, 0.05f);
            transform.rotation = Quaternion.Lerp(transform.rotation, battleCam.rotation, 0.05f);
        
        // Menu Node
        } else if (inMenu){
            StartCoroutine(LerpFirst());
            if ((whichMenu == 's') && generateShop) ActivateShopMenu();
            generateShop = false;
            mapGuide.SetActive(false);

            transform.position = Vector3.Lerp(transform.position, menuCam.position, 0.05f);
            transform.rotation = Quaternion.Lerp(transform.rotation, menuCam.rotation, 0.05f);
        
        // Map view
        }else{
            StopAllCoroutines();
            upgradeMenu.SetActive(false);
            shopMenu.SetActive(false);
            mapGuide.SetActive(true);
            generateShop= true;

            transform.position = Vector3.Lerp(transform.position, mapCam.position, 0.05f);
            transform.rotation = Quaternion.Lerp(transform.rotation, mapCam.rotation, 0.05f);
        }
    }

    public void NodeClick(MapNode node){
        // if battle node
        if (node.nodeType == MapNode.NodeType.Battle){
            inCombat = true;
            GameObject[] pfleetGo = GameObject.FindGameObjectsWithTag("Player");
            for(int i=0; i<pfleetGo.Length; i++){
                Ship s = pfleetGo[i].GetComponent<Ship>();
                s.InitCombat();
            }
            cEnemyCount = 0;
            for(int j=0; j<node.sShips; j++){
                Ship s = Instantiate(enemyShips[0], Vector3.zero, Quaternion.identity);
                s.InitCombat();
                cEnemyCount++;
            }
            for(int j=0; j<node.mShips; j++){
                Ship s = Instantiate(enemyShips[1], Vector3.zero, Quaternion.identity);
                s.InitCombat();
                cEnemyCount++;
            }
            for(int j=0; j<node.lShips; j++){
                Ship s = Instantiate(enemyShips[2], Vector3.zero, Quaternion.identity);
                s.InitCombat();
                cEnemyCount++;
            }

            //apply upgrades
            foreach(ShipLaser s in playerFleetObject.GetComponentsInChildren<ShipLaser>()){
                s.upgradeLevel = laserUp;
            }
            foreach(ShipMissileArray s in playerFleetObject.GetComponentsInChildren<ShipMissileArray>()){
                s.upgradeLevel = missileUp;
            }
            foreach(Ship s in playerFleetObject.GetComponentsInChildren<Ship>()){
                s.hitpoints += healthUp;
                s.cHitpoints = s.hitpoints;
            }
        // if shop node (menu has to be set at runtime)
        } else if (node.nodeType == MapNode.NodeType.Shop){
            inMenu = true;
            whichMenu = 's';
        // if upgrade node (menu is predetermined)
        } else if (node.nodeType == MapNode.NodeType.Upgrade){
            inMenu = true;
            whichMenu = 'u';
        }

        visitedNodes[GlobalVars.currentLayer] = node.transform.position + nodePosOffset;
        lr.positionCount += 1;
        lr.SetPosition(GlobalVars.currentLayer, visitedNodes[GlobalVars.currentLayer]);
        GlobalVars.currentLayer++;
    }

    // So that camera reaches destination before canvas is activated
     IEnumerator LerpFirst(){
        int i = 0;
        while (i<1){
            i++;
            yield return new WaitForSeconds(0.2f);
        }
        if (whichMenu == 'u'){
            upgradeMenu.SetActive(true);
            shopMenu.SetActive(false);
        } else {
            upgradeMenu.SetActive(false);
            shopMenu.SetActive(true);
        }
        yield return null;
    }

    public void EnemyDeath(){
        cEnemyCount--;
        if(cEnemyCount<=0){
            CombatCleanup();
        }
    }

    void CombatCleanup(){
        GameObject[] pfleetGo = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] missiles = GameObject.FindGameObjectsWithTag("Missile");
        foreach(GameObject m in missiles){
            Destroy(m);
        }
        foreach(GameObject p in pfleetGo){
            p.transform.position = Vector3.zero;
            Ship s = p.GetComponent<Ship>();
            s.hitpoints += 10;
            s.cHitpoints = s.hitpoints;
        }

        foreach(Ship s in playerFleetObject.GetComponentsInChildren<Ship>()){
            s.hitpoints -= healthUp;
            s.cHitpoints = s.hitpoints;
        }
        inCombat = false;
        FinishNode();
    }

    public void FinishNode(){
        inCombat = false;
        inMenu = false;

        //event stuff
        float roll = Random.Range(0f, 100f);
        if(roll <= eventChance){
            int eventind = Random.Range(0, eventPrefabs.Length);
            Instantiate(eventPrefabs[eventind], eventLocation.position, eventLocation.rotation);
        }
    }

    //0 for small, 1 for m, anything larger for large
    public void SpawnPlayerShip(int s){
        if(s <= 2){
            Ship g = Instantiate(playerShips[s], Vector3.zero, Quaternion.identity);
            g.transform.parent = playerFleetObject.transform;
        }
    }

    private void ActivateShopMenu(){
        for (int i=0; i<3; i++){
            // get random number between 0 and 2
            //var rnd = new Random();
            Debug.Log("pointing to: " + shopMenu.transform.GetChild(i).name);
            int randomizedIndex = Random.Range(0,3);

            // corresponds to slot 1,2,3 and their randomIndex'th child
            shopMenu.transform.GetChild(i).GetChild(randomizedIndex).gameObject.SetActive(true);
        }
    }
}
