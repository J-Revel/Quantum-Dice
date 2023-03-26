using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource defaultMusic;
    public AudioSource reflexionMusic;
    public float crossfadeDuration = 1;

    void Start()
    {
        defaultMusic.volume = 0;
        reflexionMusic.volume = 1;
        defaultMusic.PlayScheduled(AudioSettings.dspTime);
        reflexionMusic.PlayScheduled(AudioSettings.dspTime);
    }

    public void StartCrossFade()
    {
        StartCoroutine(MusicTransitionCoroutine());
    }

    IEnumerator MusicTransitionCoroutine()
    {
        for(float time = 0; time < crossfadeDuration; time += Time.deltaTime)
        {
            defaultMusic.volume = time / crossfadeDuration;
            reflexionMusic.volume = 1 - time / crossfadeDuration;
            yield return null;
        }
    }
}
