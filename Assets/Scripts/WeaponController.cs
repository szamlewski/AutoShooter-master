using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //zasi�g broni
    public float range = 10f;

    //transform gracza
    Transform player;

    //prefab pocisku
    public GameObject projectilePrefab;

    //spawn pocisku
    Transform projectileSpawn;

    //czestotliwosc strzalu (/sek)
    public float rateOfFire = 1;
    //czas od ostatniego wystrzalu
    float timeSinceLastFire = 0;

    // Start is called before the first frame update
    void Start()
    {
        // pozycja gracza
        player = GameObject.FindWithTag("Player").transform;

        //znajdz w hierarchii obieku miejsce z ktorego staruje pocisk
        projectileSpawn = transform.Find("ProjectileSpawn").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Transform target = TagTargeter("Enemy");
        if (target != transform)
        {
            Debug.Log("Celuje do: " + target.gameObject.name);
            transform.LookAt(target.position + Vector3.up);

            //wystrzel pocisk
            //je�li min�o wi�cej od ostatniego strza�u ni� wskazuje na to pr�dko�� strzelania
            if (timeSinceLastFire > rateOfFire)
            {
                Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
                //je�eli strzelisz to wyzeruj czas 
                timeSinceLastFire = 0;
            }
            else
            {
                timeSinceLastFire += Time.deltaTime;
            }
        }

    }
    Transform TagTargeter(string tag)
    {
        //tablica wszystkich obiekt�w pasuj�cych do taga podanego jako agument
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);

        //szukamy najbli�szego
        Transform closestTarget = transform;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            //wektor przesuni�cia wzgl�dem gracza
            Vector3 difference = target.transform.position - player.position;
            //odleg�o�� od gracza
            float distance = difference.magnitude;

            if (distance < closestDistance && distance < range) 
            {
                closestTarget = target.transform;
                closestDistance = distance;
            }
        }
        return closestTarget;
    }

    Transform LegeacyTargeter()
    {
        //znajdz wszystkie colidery w promieniu = range i zapisz je do tablicy collidersInRange
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, range);

        //do cel�w testowych 
        //Debug.Log("Ilo�� collider�w w zasi�gu broni: " +  collidersInRange.Length);

        //szukamy najbli�szego przeciwnika

        Transform target = transform;
        float targetDistance = Mathf.Infinity;

        foreach (Collider collider in collidersInRange)
        {
            //wyci�gnij transforma od tego coldiera

            //najpierw znajdz kapsu�e/model (w�a�ciciela colidera)
            GameObject model = collider.gameObject;

            if (model.transform.parent != null)
            {
                //znajdz rodzica modelu czyli przeciwnika
                GameObject enemy = model.transform.parent.gameObject;

                //sprawdz czy to co znalaz�e� jest przeciwnikiem
                if (enemy.CompareTag("Enemy"))
                {
                    //je�li to przeciwnik to okre�l wektor przesuni�cia
                    Vector3 diference = player.position - enemy.transform.position;
                    //policz d�ugo�� wektora (odleg�o��)
                    float distance = diference.magnitude;
                    if (distance < targetDistance)
                    {
                        //znaleziono nowy cel bli�ej
                        target = enemy.transform;
                        targetDistance = distance;
                    }
                }
            }


        }

        //do cel�w testowych
        Debug.Log("Celuje do: " + target.gameObject.name);

        return target;
    }
}
