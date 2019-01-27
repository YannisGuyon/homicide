using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGenerator : MonoBehaviour
{
    public AudioSource audiosource;
    public AudioClip[] japaneseSounds;
    public AudioClip[] buildingSounds;
    public AudioClip scream;
    private int lastBuildingSoundPlayedMs = 0;
    private int delayBetweenBuildingSoundMs = 1000;

    public void GenerateJapaneseSound()
    {
        audiosource.clip = japaneseSounds[Mathf.FloorToInt(Random.Range(0, 1 * japaneseSounds.Length))];
        audiosource.Play();
    }

    public void GenerateBuildingSound(float elapsedTime)
    {
        lastBuildingSoundPlayedMs -= (int)(elapsedTime * 1000);
        if (lastBuildingSoundPlayedMs <= 0)
        {
            audiosource.clip = buildingSounds[Mathf.FloorToInt(Random.Range(0, 1 * buildingSounds.Length))];
            audiosource.Play();
            lastBuildingSoundPlayedMs = (int)(delayBetweenBuildingSoundMs * (0.8 + Random.Range(0, 0.4f)));
        }
    }

    public void GenerateScream()
    {
        audiosource.clip = scream;
        audiosource.Play();
    }
}
