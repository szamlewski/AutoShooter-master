using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //zasiêg broni
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

    //moc wystrza³u (prêdkoœc pocz¹tkowa)
    public float projectileForce = 20;

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
            //Debug.Log("Celuje do: " + target.gameObject.name);
            transform.LookAt(target.position + Vector3.up);

            //wystrzel pocisk
            //jeœli minê³o wiêcej od ostatniego strza³u ni¿ wskazuje na to prêdkoœæ strzelania
            if (timeSinceLastFire > rateOfFire)
            {
                //stworz pocisk
                GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);

                
                //znajdz rrigidbody dla pocisku
                Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
                //"popchnij" pocisk do przodu
                //sila dziala w kierunku przodu dzia³a (pojectilespawn.z) * si³a wystrza³u
                projectileRB.AddForce(projectileSpawn.transform.forward * projectileForce, ForceMode.VelocityChange);

                //je¿eli strzelisz to wyzeruj czas 
                timeSinceLastFire = 0;

                //zniszcz pocisk po 5 sekundach
                Destroy(projectile, 5);
            }
            else
            {
                timeSinceLastFire += Time.deltaTime;
            }
        }

    }
    Transform TagTargeter(string tag)
    {
        //tablica wszystkich obiektów pasuj¹cych do taga podanego jako agument
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);

        //szukamy najbli¿szego
        Transform closestTarget = transform;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            //wektor przesuniêcia wzglêdem gracza
            Vector3 difference = target.transform.position - player.position;
            //odleg³oœæ od gracza
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

        //do celów testowych 
        //Debug.Log("Iloœæ colliderów w zasiêgu broni: " +  collidersInRange.Length);

        //szukamy najbli¿szego przeciwnika

        Transform target = transform;
        float targetDistance = Mathf.Infinity;

        foreach (Collider collider in collidersInRange)
        {
            //wyci¹gnij transforma od tego coldiera

            //najpierw znajdz kapsu³e/model (w³aœciciela colidera)
            GameObject model = collider.gameObject;

            if (model.transform.parent != null)
            {
                //znajdz rodzica modelu czyli przeciwnika
                GameObject enemy = model.transform.parent.gameObject;

                //sprawdz czy to co znalaz³eœ jest przeciwnikiem
                if (enemy.CompareTag("Enemy"))
                {
                    //jeœli to przeciwnik to okreœl wektor przesuniêcia
                    Vector3 diference = player.position - enemy.transform.position;
                    //policz d³ugoœæ wektora (odleg³oœæ)
                    float distance = diference.magnitude;
                    if (distance < targetDistance)
                    {
                        //znaleziono nowy cel bli¿ej
                        target = enemy.transform;
                        targetDistance = distance;
                    }
                }
            }


        }

        //do celów testowych
        //Debug.Log("Celuje do: " + target.gameObject.name);

        return target;
    }
}
