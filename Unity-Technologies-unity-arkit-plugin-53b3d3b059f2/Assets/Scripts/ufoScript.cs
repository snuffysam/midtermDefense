using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ufoScript : MonoBehaviour
{
    public int health;
    public AudioClip hitSound;
    public GameObject audioSource;
    private GameObject homeBase;
    // Start is called before the first frame update
    void Start()
    {
        homeBase = FindObjectOfType<homeBaseScript>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vec = homeBase.transform.position - transform.position;
        GetComponent<Rigidbody>().velocity = vec.normalized * health * 0.3f;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
        if (audioSource == null)
        {
            audioSource = Camera.main.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        health--;
        FindObjectOfType<homeBaseScript>().money++;
        Destroy(other.gameObject);
        audioSource.GetComponent<AudioSource>().PlayOneShot(hitSound, 0.2f);
    }
}
