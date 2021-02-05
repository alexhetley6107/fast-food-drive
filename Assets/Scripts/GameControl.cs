using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameControl : MonoBehaviour
{
    public GameObject[] maps;
    public bool isMainScene;
    public GameObject[] cars;
    public GameObject canvasLosePanel;
    public float timeToSpawnFrom = 3f, timeToSpawnTo = 5f;
    private int countCars;
    private Coroutine bottomCars, leftCars, rightCars, upCars;
    private bool isLoseOnce;// isPlaying;
    public Text nowScore, topScore, coinsCount;
    public GameObject horn;
    public AudioSource turnSignal;
    public int instCars;
    int x = 1;
    [NonSerialized] public static int countLoses;
    private static bool isAdd;
    public GameObject adsManager;
    public GameObject pauseCanvas;


    private void Start()
    {
        if (!isAdd && PlayerPrefs.GetString("NoAds") != "yes")
        {
            Instantiate(adsManager, Vector3.zero, Quaternion.identity);
            isAdd = true;
        }      

        if (PlayerPrefs.GetInt("NowMap") == 2)
        {
            maps[0].SetActive(false);
            maps[1].SetActive(true);
            maps[2].SetActive(false);
        }
        else if (PlayerPrefs.GetInt("NowMap") == 3)
        {
            maps[0].SetActive(false);
            maps[1].SetActive(false);
            maps[2].SetActive(true);
        }
        else  {
            maps[0].SetActive(true);
            maps[1].SetActive(false);
            maps[2].SetActive(false);
        }

        CarControl.isLose = false;
        CarControl.countCars = 0;

        if (isMainScene)
        {
            timeToSpawnFrom = 4f; 
            timeToSpawnTo = 6f;

        }
        bottomCars = StartCoroutine(BottomCars());
        leftCars = StartCoroutine(LeftCars());
        rightCars = StartCoroutine(RightCars());
        upCars = StartCoroutine(UpCars());

        StartCoroutine(CreateHorn());
   }
    public void Update()
    {
        if (CarControl.isLose && !isLoseOnce)
        {
            countLoses++;
            StopCoroutine(bottomCars);
            StopCoroutine(leftCars);
            StopCoroutine(rightCars);
            StopCoroutine(upCars);
            pauseCanvas.SetActive(false);
            canvasLosePanel.SetActive(true);

            isLoseOnce = true;

            nowScore.text = "<color=#F65757>Score: </color>" + CarControl.countCars.ToString();
            if (PlayerPrefs.GetInt("Score") < CarControl.countCars)
                PlayerPrefs.SetInt("Score", CarControl.countCars);
            topScore.text = "<color=#F65757>Top Score:</color>" + PlayerPrefs.GetInt("Score").ToString();
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + CarControl.countCars);
            coinsCount.text = PlayerPrefs.GetInt("Coins").ToString();

            
            instCars = 0;
            foreach (GameObject k in cars)
            {
                k.GetComponent<CarControl>().speed = 7f;
            }
            x = 1;
        }        
        if (instCars >= 10*x  ) {
            foreach (GameObject k in cars)            
                k.GetComponent<CarControl>().speed += 2f;               
            x++;                     
        }       
    }
   IEnumerator BottomCars() {
        while (true) {
            SpawnCar(new Vector3(-1.9f, -0.045f, -50f), 180);
            float timeToSpawn = Random.Range(timeToSpawnFrom, timeToSpawnTo);
            yield return new WaitForSeconds(timeToSpawn);
        }
    }
    IEnumerator LeftCars() {
        while (true) {
            SpawnCar(new Vector3(-76.9f, -0.045f, 4.6f), 270);
            float timeToSpawn = Random.Range(timeToSpawnFrom, timeToSpawnTo);
            yield return new WaitForSeconds(timeToSpawn);
        }
    }
    IEnumerator RightCars() {
        while (true) {
            SpawnCar(new Vector3(53f, -0.045f, 9.6f), 90);
            float timeToSpawn = Random.Range(timeToSpawnFrom, timeToSpawnTo);
            yield return new WaitForSeconds(timeToSpawn);
        }
    }
    IEnumerator UpCars() {
        while (true) {
            SpawnCar(new Vector3(-8.4f, -0.045f, 62.1f), 0, true);
            float timeToSpawn = Random.Range(timeToSpawnFrom, timeToSpawnTo);
            yield return new WaitForSeconds(timeToSpawn);
        }
    }
    void SpawnCar(Vector3 pos, float rotationY, bool isMoveFromUp = false)
    {
        GameObject newObj = Instantiate(cars[Random.Range(0,cars.Length)], pos, Quaternion.Euler(0, rotationY, 0)) as GameObject;
        newObj.name = "Car -"+ ++countCars;
        instCars++;


        int random = isMainScene ? 1 : Random.Range(1, 7);
        if (isMainScene)
            newObj.GetComponent<CarControl>().speed = 7f;
            switch (random)
            {
                case 1:
                case 2:
                    newObj.GetComponent<CarControl>().rightTurn = true;
                if (PlayerPrefs.GetString("music") != "No" && !turnSignal.isPlaying)
                {
                    turnSignal.Play();
                    Invoke("StopSound", 4f);
                }
                    break;
                case 3:
                case 4:
                    newObj.GetComponent<CarControl>().leftTurn = true;
                if (PlayerPrefs.GetString("music") != "No" && !turnSignal.isPlaying)
                {
                    turnSignal.Play();
                    Invoke("StopSound", 4f);
                }
                if (isMoveFromUp)
                           newObj.GetComponent<CarControl>().moveFromUp = true;
                break;
                case 5:
                    break;
            }       
    }
    void StopSound()
    {
        turnSignal.Stop();
    }
    IEnumerator CreateHorn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4, 8));
            if (PlayerPrefs.GetString("music") != "No")
                Instantiate(horn, Vector3.zero, Quaternion.identity);
        }
    }
}
