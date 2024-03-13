using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //pozycja gracza
    Transform player;

    //prefab przeciwnika
    public GameObject basherPrefab;

    //czas miêdzy respawnem kolejnego bashera
    public float spawnInterval = 1;

    //czas od ostatniego respawnu
    float timeSinceSpawn;

    //bezpieczna odleg³oœæ spawnu
    float spawnDistance = 30;

    // Start is called before the first frame update
    void Start()
    {
        //zlinkuj aktualna pozycje gracza do zmiennej transform
        player = GameObject.FindWithTag("Player").transform;

        //zerujemy licznik
        timeSinceSpawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //dodaj do czasu od ostatniego spawnu czas od ostatniej klatki (ostatni update())
        timeSinceSpawn += Time.deltaTime;

        //je¿eli d³u¿ej ni¿ jedna sekunda
        if(timeSinceSpawn > spawnInterval)
        {
            //wygeneruj losow¹ pozycje
            //Vector3 randomPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));

            //wygeneruj randomow¹ pozycjê na kole o promieniu 1
            Vector2 random = Random.insideUnitCircle.normalized;

            //skonwertuj x,y na x,z i zerow¹ wysokoœæ
            Vector3 randomPosition = new Vector3(random.x, 0, random.y);

            //zwielokrotnij odleg³osæ od gracza tak, ¿eby spawn nastêpowa³ poza kamer¹
            randomPosition *= spawnDistance;

            //dodaj do niej pozycje gracza tak, aby nowe wspó³rzêdne by³y pozycj¹ wzglêdem gracza
            randomPosition += player.position;

            //sprawdz czy danej miejsce jest wolne
            if(!Physics.CheckSphere(new Vector3(randomPosition.x, 1, randomPosition.z), 0.5f))
            {
                //stworz nowego przeciwnika z istniej¹cego prefaba, na pozycji randomPosition z rotacj¹ domyœln¹
                Instantiate(basherPrefab, randomPosition, Quaternion.identity);

                //wyzeruj licznik
                timeSinceSpawn = 0;
            }
            //jeœli miejsce bêdzie zajête to program podejmie kolejn¹ próbê w nastêpnej klatce
            
        }

        //TODO: opracowaæ sposób na przyspieszanie spawnu w nieskoñczonoœæ wraz z d³ugoœcia trwania etapu

        
    }
}
