using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMissileArray : MonoBehaviour
{
    public float fireCooldown = 1f;
    public float swarmDelay = 0.07f;
    public int missileCount;
    public Missile missile;
    public Transform[] firePoints;
    float cFireCooldown;
    Ship ship;
    public int upgradeLevel;

    // Start is called before the first frame update
    void Start()
    {
        ship = GetComponent<Ship>();
        cFireCooldown = Random.Range(0f, (fireCooldown));
    }

    // Update is called once per frame
    void Update()
    {
        if(ship.target == null){
            return;
        }
        if(cFireCooldown <= 0){
            Fire();
            return;
        }
        cFireCooldown -= Time.deltaTime;
    }

    public void Fire(){
        for(int i = 0; i < (missileCount+upgradeLevel); i++){
            foreach(Transform fp in firePoints){
                StartCoroutine(ShootMissile(i * swarmDelay, fp));
            }
        }
        cFireCooldown = fireCooldown;
    }

    IEnumerator ShootMissile(float delay, Transform fp){
        yield return new WaitForSeconds(delay);
        Vector3 pos = fp.position;
        Vector3 rotOffset = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
        Missile m = Instantiate(missile, fp.position, fp.rotation);
        m.transform.Rotate(rotOffset);
        m.InitializeMissile(ship.GetRandomTarget());
    }
}
