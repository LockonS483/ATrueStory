using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public enum MovementType {Map, Target};
    public int hitpoints;
    public int cHitpoints;
    public enum Team {player, enemy};
    public Team team;
    public MovementType movementType;
    public Vector2 angleSpeedRange; //MAP movement only
    public Vector2 turnSpeed;  //Target movement only
    public float moveSpeed;
    float angleSpeed; //angle/turn speed for movement
    public Vector2 moveRadiusRange;
    public float moveRadius;
    public float targetAngle = 0;
    public enum ShipType {Fighter, MFighter, Cruiser};
    public ShipType whoAmI;

    public Ship target;

    public GameObject[] explosions;
    

    TrailRenderer tr;
    float otime;

    Vector2 yBounds = new Vector2(-20, 20);

    // Start is called before the first frame update
    void Start()
    {
        //InitCombat();
        //tr.time = otime;
    }

    public void InitCombat(){
        tr = GetComponent<TrailRenderer>();
        otime = tr.time;
        tr.time = 0;
        Invoke("ResetTrail", 0.5f);
        moveRadius = Random.Range(moveRadiusRange.x, moveRadiusRange.y);
        if(movementType == MovementType.Map){
            targetAngle = Random.Range(0, Mathf.PI);
            angleSpeed = Random.Range(angleSpeedRange.x, angleSpeedRange.y);
            Vector3 startPos = Vector3.zero;
            startPos.y = Random.Range(-10f, 10f);
            transform.position = startPos;
            transform.position = CalculatePos(targetAngle);
        }else if(movementType == MovementType.Target){
            Vector3 startPos = Random.insideUnitSphere * moveRadius;
            startPos.y = 0f;
            transform.position = startPos;
            transform.rotation = Random.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveUpdate();
        
    }

    void MoveUpdate(){
        if(target == null){
            if(GetNewTarget() != 0){
                return;
            }
        }
        if(movementType == MovementType.Map){
            targetAngle += angleSpeed * Time.deltaTime;
            transform.position = CalculatePos(targetAngle);

            transform.LookAt(Vector3.zero);
            transform.Rotate(0, 90, 0);
        }else if(movementType == MovementType.Target){
            float d = Vector3.Distance(transform.position, target.transform.position);
            if(d >= moveRadius){
                angleSpeed = turnSpeed.y;
            }else{
                angleSpeed = Random.Range(0.1f, turnSpeed.x);
            }

            Vector3 tDir = target.transform.position - transform.position;
            float step = angleSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, tDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);

            transform.Translate(0, 0, moveSpeed * Time.deltaTime);
        }
        
        if(transform.position.y > yBounds.y){
            Vector3 ny = transform.position;
            ny.y = yBounds.y;
            transform.position = ny;
        }else if(transform.position.y < yBounds.x){
            Vector3 ny = transform.position;
            ny.y = yBounds.x;
            transform.position = ny;
        }
    }

    int GetNewTarget(){
        GameObject[] es;
        if(team == Team.player){
            es = GameObject.FindGameObjectsWithTag("Enemy");
        }else{
            es = GameObject.FindGameObjectsWithTag("Player");
        }
        int l = es.Length;
        if(l == 0){
            return 1;
        }
        
        int ind = Random.Range(0, l);
        target = es[ind].GetComponent<Ship>();
        return 0;
    }

    Vector3 CalculatePos(float ang){
        Vector3 center = Vector3.zero;
        if(movementType == MovementType.Target){
            center = target.transform.position;
        }
        Vector3 r = Vector3.zero;
        r.x = Mathf.Cos(ang);
        r.z = Mathf.Sin(ang);
        r *= moveRadius;
        r.y = transform.position.y;
        return r;
    }

    void ResetTrail(){
        tr.time = otime;
    }

    void Die(){
        foreach(GameObject g in explosions){
            Instantiate(g, transform.position, transform.rotation);
        }
        if(gameObject.tag == "Enemy"){
            GameObject.FindObjectsOfType<GameManager>()[0].GetComponent<GameManager>().EnemyDeath();
        }
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void DealDamage(int d){
        cHitpoints -= d;
        if(cHitpoints <= 0){
            Die();
        }
    }

    public Ship GetRandomTarget(){
        GameObject[] es;
        if(team == Team.player){
            es = GameObject.FindGameObjectsWithTag("Enemy");
        }else{
            es = GameObject.FindGameObjectsWithTag("Player");
        }
        int l = es.Length;
        if(l == 0){
            return null;
        }
        
        int ind = Random.Range(0, l);
        Ship t = es[ind].GetComponent<Ship>();
        return t;
    }
}
