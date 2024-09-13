using UnityEngine;
using UnityEngine.Audio;

public class WindAudioController : MonoBehaviour
{
    public WindForLayer windController;
    public AudioMixerGroup calmMusicMixerGroup;
    public AudioMixerGroup calmSoundMixerGroup;
    public AudioMixerGroup stormMusicMixerGroup;
    public AudioMixerGroup stormSoundMixerGroup;
    public AudioMixerGroup mixer;
    public AudioSource calmMusicSource;
    public AudioSource[] calmSoundSources;
    public AudioSource stormMusicSource;
    public AudioSource[] stormSoundSources;
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
        targetVolumeCalm = currentWindForce < calmThreshold ? 0f : 1f;
        targetVolumeStorm = currentWindForce >= stormThreshold ? 0f : 1f;

        // Lerp towards target volumes
        volumeCalmLerpFactor = Mathf.Lerp(volumeCalmLerpFactor, targetVolumeCalm, transitionDuration * Time.deltaTime);
        volumeStormLerpFactor = Mathf.Lerp(volumeStormLerpFactor, targetVolumeStorm, transitionDuration * Time.deltaTime);

        // Apply volumes to audio mixer groups
        mixer.audioMixer.SetFloat("CalmVolume", volumeCalmLerpFactor);
        calmMusicMixerGroup.audioMixer.SetFloat("CalmVolume", volumeCalmLerpFactor*-1000);
        calmSoundMixerGroup.audioMixer.SetFloat("CalmVolume2", volumeCalmLerpFactor*-1000);
        mixer.audioMixer.SetFloat("StormVolume", volumeStormLerpFactor);
        stormMusicMixerGroup.audioMixer.SetFloat("StormVolume", volumeStormLerpFactor*-1000);
        stormSoundMixerGroup.audioMixer.SetFloat("StormVolume2", volumeStormLerpFactor*-1000);
    }
}
