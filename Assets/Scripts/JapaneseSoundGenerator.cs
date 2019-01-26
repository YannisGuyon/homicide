using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JapaneseSoundGenerator : MonoBehaviour
{
    public AudioSource audiosource;
    public AudioClip[] sounds;

    public void GenerateSound()
    {
        audiosource.clip = sounds[Mathf.FloorToInt(Random.Range(0, 1 * sounds.Length))];
        audiosource.Play();
    }
}
