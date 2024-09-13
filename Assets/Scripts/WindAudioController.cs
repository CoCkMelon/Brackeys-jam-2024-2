using UnityEngine;
using UnityEngine.Audio;

public class WindAudioController : MonoBehaviour
{
    public WindForLayer windController;
    public AudioMixerGroup calmMixerGroup;
    public AudioMixerGroup stormMixerGroup;
    public AudioSource calmMusicSource;
    public AudioSource stormMusicSource;
    public float calmThreshold = 1f;
    public float stormThreshold = 5f;
    public float transitionDuration = 2f;

    private float currentWindForce;
    private float targetVolumeCalm;
    private float targetVolumeStorm;
    private float volumeCalmLerpFactor;
    private float volumeStormLerpFactor;

    void Start()
    {
        currentWindForce = windController.currentPulseForce;
        UpdateAudioVolumes();
    }

    void FixedUpdate()
    {
        currentWindForce = windController.currentPulseForce;
        UpdateAudioVolumes();
    }

    void UpdateAudioVolumes()
    {
        // Determine target volumes based on current wind force
        targetVolumeCalm = currentWindForce < calmThreshold ? 1f : 0f;
        targetVolumeStorm = currentWindForce >= stormThreshold ? 1f : 0f;

        // Lerp towards target volumes
        volumeCalmLerpFactor = Mathf.Lerp(volumeCalmLerpFactor, targetVolumeCalm, transitionDuration * Time.deltaTime);
        volumeStormLerpFactor = Mathf.Lerp(volumeStormLerpFactor, targetVolumeStorm, transitionDuration * Time.deltaTime);

        // Apply volumes to audio mixer groups
        calmMixerGroup.audioMixer.SetFloat("CalmVolume", volumeCalmLerpFactor);
        stormMixerGroup.audioMixer.SetFloat("StormVolume", volumeStormLerpFactor);
    }
}
