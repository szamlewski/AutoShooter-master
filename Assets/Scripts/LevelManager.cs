using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    //ilosc punktów
    int points = 0;

    //licznik punktów na ekranie
    public GameObject pointsCounter;

    //licznik czasu na ekranie
    public GameObject timeCounter;

    //ekran koñca gry
    public GameObject gameOverScreen;

    //czas do koñca poziomu
    public float levelTime = 60f;

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

        //dodaj do czasu poziomu czas od ostatniej klatki
        
        if(levelTime < 0)
        {
            GameOver();
        } 
        else
        {
            levelTime -= Time.deltaTime;
            UpdateUI();
        }
        
    }
    public void AddPoints(int amount)
    {
        points += amount;
    }
    //funkcja która odpowiada za aktualizacje interfejsu
    private void UpdateUI()
    {
        pointsCounter.GetComponent<TextMeshProUGUI>().text = "Punkty: " + points.ToString();
        timeCounter.GetComponent<TextMeshProUGUI>().text = Mathf.Floor(levelTime).ToString();
    }
    //ta funkcja uruchamia siê jeœli gracz zginie lub jeœli czas siê skoñczy
    public void GameOver()
    {
        //wy³¹cz sterowanie gracza
        player.GetComponent<PlayerController>().enabled = false;
        player.transform.Find("MainTurret").GetComponent<WeaponController>().enabled = false;

        //wylacz bashery
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject basher in enemyList)
        {
            basher.GetComponent<BasherController>().enabled = false;
        }

        //wyswietl poprawnie wynik na ekranie koñcowym
        gameOverScreen.transform.Find("FinalScoreText").GetComponent<TextMeshProUGUI>().text = "Wynik koñcowy: " + points.ToString();

        //poka¿ ekran koñca gry
        gameOverScreen.SetActive(true);
        
    }
}
