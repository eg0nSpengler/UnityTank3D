using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfoObject : MonoBehaviour
{

    [SerializeField]
    private LevelInfo _levelInfo;

    private void OnEnable()
    {
        Debug.Log(_levelInfo.GetLevelName);
        Debug.Log(_levelInfo.GetLevelDesc);
    }
}
