using UnityEngine;
using System.Collections;

public class AudioCollisionBehaviour : MonoBehaviour
{
    private const float VelToVolFactor = 0.2f;

    public AudioClip SoundSoftCrash;
    public AudioClip SoundHardCrash;

    public float VelocityClipSplit = 10f;

    public bool UseRandomPitch = true;
    public float LowPitchRange = 0.75f;
    public float HighPitchRange = 1.5f;

    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.spatialize = true;
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
            _audioSource.spatialBlend = 1.0f;
            _audioSource.dopplerLevel = 0.2f;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (UseRandomPitch)
        {
            _audioSource.pitch = Random.Range(LowPitchRange, HighPitchRange);
        }

        var velMagnitude = coll.relativeVelocity.magnitude;
        var hitVol = velMagnitude * VelToVolFactor;
        var audioClip = velMagnitude < VelocityClipSplit ? SoundSoftCrash : SoundHardCrash;
        if (audioClip == null)
        {
            audioClip = SoundSoftCrash == null ? SoundHardCrash : SoundSoftCrash;
        }
        _audioSource.PlayOneShot(audioClip, hitVol);
    }
}
