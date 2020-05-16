using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfoObject : MonoBehaviour
{

    [SerializeField]
    private LevelInfo levelInfo;

    private void OnEnable()
    {
        Debug.Log(levelInfo.GetLevelName);
        Debug.Log(levelInfo.GetLevelDesc);
    }
}
