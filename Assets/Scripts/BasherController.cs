using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasherController : MonoBehaviour
{
    //gracz
    GameObject player;
    //prędkość podążania za graczem
    public float walkSpeed = 1f;
    //odwolanie do levelManager
    GameObject levelManager;
    //flaga, która mówi czy został już trafiony i został za niego policzony punkt
    bool hasBeenHit = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        levelManager = GameObject.Find("LevelManager");
    }

    // Update is called once per frame
    void Update()
    {
        //patrz się na gracza
        transform.LookAt(player.transform.position);
        //idz do przodu
        transform.position += transform.forward * Time.deltaTime * walkSpeed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //jeśli został już trafion to nic nie rób
        if(hasBeenHit) { return; }
        //Debug.Log("Trafiony");
        //obiekt z którym mamy kolizje
        GameObject projectile = collision.gameObject;

        //tylko jeśli trafił nas gracz
        if(projectile.CompareTag("PlayerProjectile"))
        {
    

            //ustaw flage
            hasBeenHit = true;
            
            //dolicz punkty
            levelManager.GetComponent<LevelManager>().AddPoints(1);
            
            //zniknij pocisk
            Destroy(projectile);

            //zniknij przeciwnika
            Destroy(transform.gameObject);

        }
       /* if (collision.gameObject.CompareTag("Player"))
        {
            //weszlismy w gracza - poinformuj go o tym
            collision.gameObject.GetComponent<PlayerController>().Hit(gameObject);
        }*/
    }
}
