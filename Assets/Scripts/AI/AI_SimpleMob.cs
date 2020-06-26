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
            Debug.LogError("Failed to get Audio Source on " + gameObject.name.ToString() + ", creating one now...");
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
        StartCoroutine(MonsterDeath());
    }

    IEnumerator MonsterDeath()
    {
        _audioSource.Play();
        yield return new WaitForSeconds(_audioSource.clip.length); // Just to make sure the clip plays completely before disabling the gameobject
        gameObject.SetActive(false);
    }


}
