using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_SimpleMob : MonoBehaviour
{
    private AudioSource _audioSource;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (!_audioSource)

        {
            Debug.LogError("Failed to get Audio Source on " + gameObject.name.ToString() + ", creating one now");
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        gameObject.GetComponent<HealthComponent>().OnHealthZero += DoMonsterDeath;
    }

    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    void DoMonsterDeath()
    {
        gameObject.SetActive(false);
        AudioSource.PlayClipAtPoint(_audioSource.clip, gameObject.transform.position);
    }

}
