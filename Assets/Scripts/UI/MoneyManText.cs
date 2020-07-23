using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManText : MonoBehaviour
{
    [Header("Text References")]
    public TextMeshProUGUI SaviorText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI SectorText;

    private void Awake()
    {

        if (!SaviorText | !TimeText | !SectorText)
        {
            Debug.LogError("MoneyManText is missing a Text Reference!");
        }

        SaviorText.text = " ";
        TimeText.text = " ";
        SectorText.text = " ";
        SaviorText.color = Color.white;
        TimeText.color = Color.white;
        SectorText.color = Color.white;

    }

    private void OnEnable()
    {
        PickupDisplayBox.OnPickupScoreGood += DisplayPickupText;
        LevelTimerBox.OnLevelTimerScoreBegin += DisplayTimeText;
        LevelTimerBox.OnLevelTimerScoreEnd += DisplaySectorText;
    
    }

    private void OnDisable()
    {
        PickupDisplayBox.OnPickupScoreGood -= DisplayPickupText;
        LevelTimerBox.OnLevelTimerScoreBegin -= DisplayTimeText;
        LevelTimerBox.OnLevelTimerScoreEnd -= DisplaySectorText;
    }


    private void DisplayPickupText()
    {
        SaviorText.text = "Savior reward...";
    }

    private void DisplayTimeText()
    {
        TimeText.text = "Time bonus...";
    }

    private void DisplaySectorText()
    {
        SectorText.text = "GO TO NEXT SECTOR";
    }
    


}
