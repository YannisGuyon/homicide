using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public AudioSource audiosource;
    public AudioClip gameoversound;

    void Start()
    {
        audiosource.clip = gameoversound;
        audiosource.loop = true;
        audiosource.Play();
    }

    public void NextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
