using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    [Space]
    public AudioSource fixedPitchSource;
    public AudioSource variablePitchSource;
    public AudioSource[] worldSources;

    [Header("Audio Clips")]
    [Space]

    [Space]
    [Header("External References")]
    [Space]

    [Header("Attributes")]
    [Space]
    public float randomPitchFactor = 0.01f;
    public float minCamSize = 3f;
    public float maxCamSize = 8f;

    [Header("Privates")]
    [Space]
    private WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    private bool canPlaySprintStep = true;
    private bool canPlaySprintStepRelease = true;

    public void UpdateAudioSourcesVolume(float camSize)
    {
        if (camSize <= minCamSize)
            for (int i = 0; i < worldSources.Length; i++)
                worldSources[i].volume = 1f;
        else if (camSize >= maxCamSize)
            for (int i = 0; i < worldSources.Length; i++)
                worldSources[i].volume = 3f / maxCamSize;
        else
            for (int i = 0; i < worldSources.Length; i++)
                worldSources[i].volume = 3f / camSize;
    }

    public void PlaySprintStep()
    {
        //if (!canPlaySprintStep)
        //    return;

        //canPlaySprintStep = false;

        //PitchRandomizer(stepsSource, randomPitchFactor);

        //stepsSource.PlayOneShot(sprintStepClips[Random.Range(0, sprintStepClips.Length)]);

        //StartCoroutine(WaitToPlaySprintStepAgain());
    }

    public void PlayHandStepRelease()
    {
        //PitchRandomizer(handStepReleaseSource, randomPitchFactor);

        //handStepReleaseSource.PlayOneShot(handStepReleaseClips[Random.Range(0, handStepReleaseClips.Length)]);
    }

    private IEnumerator WaitToPlaySprintStepAgain()
    {
        yield return new WaitForSeconds(0.1f);

        canPlaySprintStep = true;
    }

    private void PitchRandomizer(AudioSource source, float delta)
    {
        float rand = Random.Range(1 - delta, 1 + delta);
        source.pitch = rand;
    }
}