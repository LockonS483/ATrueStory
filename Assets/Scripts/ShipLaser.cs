using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipLaser : MonoBehaviour
{
    public Ship ship;
    public float fireCooldown = 1f;
    float cFireCooldown;
    public Laser laser;
    public Transform[] firePoints;
    public Color color;
    public int damage;
    public int upgradeLevel;

    // Start is called before the first frame update
    void Start()
    {
        cFireCooldown = Random.Range(0f, (fireCooldown));
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("laser upgrade: " + upgradeLevel.ToString());
        if(cFireCooldown <= 0){
            Fire();
            return;
        }
        cFireCooldown -= Time.deltaTime;
    }

    public void Fire(){
        if(ship.target == null) return;

        foreach(Transform f in firePoints){
            Laser t = Instantiate(laser).GetComponent<Laser>();
            t.Initialize(f.position, ship.target.transform.position, color);
        }

        ship.target.DealDamage(damage + (upgradeLevel*5));

        cFireCooldown = fireCooldown;
    }
}
