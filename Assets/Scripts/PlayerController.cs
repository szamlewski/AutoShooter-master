using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* 
         * legacy kode
         * //pobierz stan kontrolera (poziom)
         float x = Input.GetAxisRaw("Horizontal");
         //wylicz docelowy ruch poziomo (lewo/prawo po osi x) mnożąc wychylenie kontrolera przez "1"
         Vector3 movement = Vector3.right * x;

         //pobierz stan kontrolera (pion)
         float y = Input.GetAxisRaw("Vertical");
         movement += Vector3.forward * y;

         //normalizuj ruch
         movement = movement.normalized;

         //przelicz przez czas od ostatniej klatki
         movement *= Time.deltaTime;

         //pomnóż ruch przez prędkość
         movement *= moveSpeed;

         //nałóż zmianę położenia na obiekt gracza
         transform.position += movement;*/
        //pobierz kierunek ruchu x/y
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        //stworz wektor kierunku poruszania się
        Vector3 targetDirection = new Vector3 (x, 0, y);
        Vector3 targetPosition = transform.position + targetDirection;
        if(targetDirection.magnitude > Mathf.Epsilon)
        {
            transform.LookAt (targetPosition);
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
        
    }
    public void Hit(GameObject other)
    {
        //zarejestrowano kolizje z other
        Debug.Log("Gracz trafiony");
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.ToString());
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Gracz trafiony");
            GameObject.Find("LevelManager").GetComponent<LevelManager>().GameOver();
        }
    }
}
