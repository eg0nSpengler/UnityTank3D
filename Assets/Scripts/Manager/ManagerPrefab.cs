using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPrefab : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

}
