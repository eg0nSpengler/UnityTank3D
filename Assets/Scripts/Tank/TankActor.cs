using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This just serves as a class to hold references to other components on our TankActor
/// </summary>
/// 
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(TankMovement))]
[RequireComponent(typeof(TankActor))]
[RequireComponent(typeof(HealthComponent))]
public class TankActor : MonoBehaviour
{
    [Header("Audio References")]
    public AudioClip hitSound;
    public AudioClip deathSound;

    public class TankStats
    {
        public int PlayerScore { get; set; }
        public int LevelNum { get; set; }

        public int NumPickupsCollected { get; set; }

        public int NumPickupsLost { get; set; }

        public int NumPickupsTotal { get; set; }

        public int FinalLevelTime { get; set; }

        public List<bool> PickupBool;
    }

    public HealthComponent HealthComp { get; private set; }


    private AudioSource _audioSource;

    private TankStats _tankStats;


    private void Awake()
    {
        HealthComp = GetComponent<HealthComponent>();
        _audioSource = GetComponent<AudioSource>();
        _tankStats = new TankStats();
        _tankStats.PickupBool = new List<bool>();

        if (!HealthComp)
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

    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.PageUp))
        {
            TeleportToPortal();
        }
    }

    private void OnEnable()
    {
        FindObjectOfType<LevelPortal>().OnLevelPortalEnabled += SaveTankData;
        //LevelTimerBox.OnLevelTimerScoreEnd += SaveTankData;
        HealthComp.OnHealthModified += PlayHitSound;
        HealthComp.OnHealthZero += PlayDeathSound;
        GameManager.OnGameStateGameOver += PlayDeathSound;
    }

    private void OnDisable()
    {
        FindObjectOfType<LevelPortal>().OnLevelPortalEnabled -= SaveTankData;
        HealthComp.OnHealthModified -= PlayHitSound;
        HealthComp.OnHealthZero -= PlayDeathSound;
        GameManager.OnGameStateGameOver -= PlayDeathSound;
    }

    public void SaveTankData()
    {
        Debug.Log("Saving Tank Data! at " + Time.time.ToString());

        _tankStats.PlayerScore = PickupManager.PlayerScore;
        _tankStats.LevelNum = LevelManager.CurrentLevelStats.CurrentLevelNum;
        _tankStats.LevelNum++;
        _tankStats.NumPickupsCollected = PickupManager.NumPickupsCollected;
        _tankStats.NumPickupsLost = PickupManager.NumPickupsLost;
        _tankStats.FinalLevelTime = LevelTimerBox.FinalLevelTime;


        foreach (var pk in PickupManager.GetPickupBoolList())
        {
            _tankStats.PickupBool.Add(pk);
        }

        GameDataSerializer.SaveGameData(_tankStats);
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

    /// <summary>
    /// A cheat method to place the player at the position of the LevelPortal
    /// </summary>
    void TeleportToPortal()
    {
        var portalPos = FindObjectOfType<LevelPortal>().gameObject.transform.position;

        gameObject.transform.position = portalPos;
    }
}
