using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.iOS;

public class homeBaseScript : MonoBehaviour
{
    public float timePerSpawn;
    public int health;
    public int money;
    public GameObject planePlacer;
    public GameObject ufo;
    public AudioClip ufoSpawn;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public GameObject placeText;
    public GameObject moneyText;
    public GameObject healthText;
    public GameObject levelText;
    public GameObject soldierPrefab;
    private UnityARCameraManager arUtility;
    public AudioClip victory;
    float timer;
    bool exists;
    GameObject placingObject;
    bool gameOn;
    List<GameObject> currentUFOs;
    int[] waves;
    int level;
    float initSpawnTime;
    // Start is called before the first frame update
    void Start()
    {
        //gameOverPanel.SetActive(true);
        exists = false;
        placingObject = null;
        level = -1;
        waves = new int[] { 20, 30, 45, 68, 101, 152, 228, 342, 513, 769 };
        gameOn = false;
        currentUFOs = new List<GameObject>();
        initSpawnTime = timePerSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        if (level > -1)
        {
            timePerSpawn = initSpawnTime / Mathf.Sqrt(level + 1);
        }

        if (arUtility == null)
        {
            arUtility = FindObjectOfType<UnityARCameraManager>();
        }
        else
        {
            arUtility.planeDetection = UnityARPlaneDetection.Horizontal;
            arUtility.getPointCloud = false;
            arUtility.enableLightEstimation = false;
        }
        exists = exists || GetComponent<UnityARHitTestExample>().DoesExist();
        if (exists)
        {
            if (GetComponent<UnityARHitTestExample>().enabled)
            {
                planePlacer.SetActive(false);
                arUtility.planeDetection = UnityARPlaneDetection.None;
            }
            GetComponent<UnityARHitTestExample>().enabled = false;
            GetComponent<MeshRenderer>().enabled = true;
            timer += Time.deltaTime;
            if (level > -1 && gameOn && currentUFOs.Count < waves[level] && timer > timePerSpawn)
            {
                timer = 0f;
                GameObject myUfo = Instantiate(ufo);
                Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f));
                myUfo.transform.position = transform.position + dir.normalized * 3;
                currentUFOs.Add(myUfo);
                GetComponent<AudioSource>().PlayOneShot(ufoSpawn, 0.3f);
            }
            if (gameOn && level > -1 && currentUFOs.Count >= waves[level])
            {
                bool found = false;
                foreach (GameObject u in currentUFOs)
                {
                    if (u != null)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    gameOn = false;
                    GetComponent<AudioSource>().PlayOneShot(victory);
                }
                if (gameOn == false && level == waves.Length - 1)
                {
                    victoryPanel.SetActive(true);
                }
            }
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        GetComponent<MeshRenderer>().materials[0].color = Color.HSVToRGB(0.492f * health / 100f, 0.37f, 1f);
        if (health <= 0)
        {
            gameOverPanel.SetActive(true);
        }
        if (placingObject != null && placingObject.GetComponentInChildren<UnityARHitTestExample>().DoesExist())
        {
            placingObject = null;
            placeText.SetActive(false);
        }
        moneyText.GetComponent<Text>().text = money + " D";
        healthText.GetComponent<Text>().text = "" + health;
        levelText.GetComponent<Text>().text = "" + (level + 1);
        if (gameOn)
        {
            levelText.GetComponent<Text>().color = Color.green;
        }
        else
        {
            levelText.GetComponent<Text>().color = Color.white;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ufoScript>() != null)
        {
            health -= collision.gameObject.GetComponent<ufoScript>().health;
            Destroy(collision.gameObject);
            GetComponent<AudioSource>().PlayOneShot(ufoSpawn, 1f);
        }
    }

    public void buySoldier()
    {
        if (exists && placingObject == null && money >= 200)
        {
            planePlacer.SetActive(true);
            arUtility.planeDetection = UnityARPlaneDetection.Horizontal;
            placeText.SetActive(true);
            placingObject = Instantiate(soldierPrefab);
            money -= 200;
        }
    }

    public UnityARCameraManager GetARUtility()
    {
        return arUtility;
    }

    public void startWave()
    {
        if (exists && !gameOn && level < waves.Length - 1)
        {
            level++;
            gameOn = true;
            currentUFOs = new List<GameObject>();
        }
    }

    public bool IsGameRunning()
    {
        return gameOn;
    }

    public List<GameObject> ActiveUFOs()
    {
        return currentUFOs;
    }
}
