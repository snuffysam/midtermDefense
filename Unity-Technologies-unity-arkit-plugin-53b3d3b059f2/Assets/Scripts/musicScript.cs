using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class musicScript : MonoBehaviour
{

    public GameObject audioSource;
    public AudioClip mainMusic;
    public AudioClip finMusic;
    public GameObject victoryPanel;
    public GameObject gameOverPanel;
    bool playing;
    // Start is called before the first frame update
    void Start()
    {
        audioSource.GetComponent<AudioSource>().clip = mainMusic;
        audioSource.GetComponent<AudioSource>().Play();
        playing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (victoryPanel.activeInHierarchy || gameOverPanel.activeInHierarchy)
        {
            audioSource.GetComponent<AudioSource>().Stop();
        } else if (GetComponent<Text>().text == "10" && !playing)
        {
            audioSource.GetComponent<AudioSource>().clip = finMusic;
            audioSource.GetComponent<AudioSource>().Play();
            playing = true;
        }
    }
}
