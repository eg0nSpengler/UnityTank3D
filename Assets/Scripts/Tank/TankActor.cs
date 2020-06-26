using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This just serves as a class to hold references to other components on our TankActor
/// </summary>
public class TankActor : MonoBehaviour
{
    [Header("Audio References")]
    public AudioClip hitSound;
    public AudioClip deathSound;

    public HealthComponent healthComp { get; private set; }

    public int playerScore { get; private set; }
    public int levelNum { get; private set; }

    public int numPickupsCollected { get; private set; }

    public int numPickupsLost { get; private set; }

    private AudioSource _audioSource;

    private  int _numPickupsCollected;

    private void Awake()
    {
        healthComp = GetComponent<HealthComponent>();
        _audioSource = GetComponent<AudioSource>();

        _numPickupsCollected = numPickupsCollected;

        if (!healthComp)
        {
            Debug.LogError("Failed to get Health Component on " + gameObject.name.ToString() + ", creating one now");
            gameObject.AddComponent<HealthComponent>();
        }

        if (!_audioSource)
        {
            Debug.LogError("Failed to get Audio Source on " + gameObject.name.ToString() + ", creating one now");
            gameObject.AddComponent<AudioSource>();
        }

        if (!hitSound)
        {
            Debug.LogWarning("No Hit Sound provided for TankActor!");
        }

        if (!deathSound)
        {
            Debug.LogWarning("No Death Sound provided for TankActor!");
        }

        LevelPortal.OnLevelPortalEnabled += SaveTankData;
        healthComp.OnHealthModified += PlayHitSound;
        healthComp.OnHealthZero += PlayDeathSound;
    }


    private void OnDisable()
    {
        LevelPortal.OnLevelPortalEnabled -= SaveTankData;
        healthComp.OnHealthModified -= PlayHitSound;
        healthComp.OnHealthZero -= PlayDeathSound;
    }

    public void SaveTankData()
    {
        GameDataSerializer.SaveGameData(this);
    }

    public int GetTankPickups()
    {
        return _numPickupsCollected;
    }

    void PlayHitSound()
    {
        _audioSource.clip = hitSound;
        _audioSource.Play();
    }

    void PlayDeathSound()
    {
        _audioSource.clip = deathSound;
        _audioSource.Play();
    }
}
