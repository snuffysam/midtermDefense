using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetingScript : MonoBehaviour
{

    public GameObject shootObject;
    public float range;
    public float fireRate;
    public AudioClip shootSound;
    private float timer;
    private bool exists;
    private homeBaseScript homeBase;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        exists = false;
        homeBase = FindObjectOfType<homeBaseScript>();
    }

    // Update is called once per frame
    void Update()
    {
        exists = exists || GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().DoesExist();

        if (exists)
        {
            if (GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled)
            {
                homeBase.planePlacer.SetActive(false);
                homeBase.GetARUtility().planeDetection = UnityEngine.XR.iOS.UnityARPlaneDetection.None;
            }
            GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = false;
        }

        timer += Time.deltaTime;
        if (homeBase.IsGameRunning() && timer > 1f / fireRate)
        {
            timer = 0f;
            List<GameObject> ufos = homeBase.ActiveUFOs();
            float min = range;
            ufoScript ufo = null;
            foreach (GameObject u in ufos)
            {
                if (u != null)
                {
                    float n = (transform.position - u.transform.position).magnitude;
                    if (n <= min)
                    {
                        min = n;
                        ufo = u.GetComponent<ufoScript>();
                    }
                }
            }
            if (ufo != null)
            {
                GameObject go = Instantiate<GameObject>(shootObject);
                go.transform.position = transform.position;
                go.GetComponent<projectileScript>().target = ufo.gameObject;
                GetComponent<Animator>().CrossFade("Shoot", 0.1f);
                GetComponent<AudioSource>().PlayOneShot(shootSound, 0.1f);

                Vector3 target = new Vector3(ufo.transform.position.x, transform.position.y, ufo.transform.position.z);

                Vector3 targetDir = target - transform.position;

                // The step size is equal to speed times frame time.
                float step = 100000 * Time.deltaTime;

                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);

                // Move our position a step closer to the target.
                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
    }


}
