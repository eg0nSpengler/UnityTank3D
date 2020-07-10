using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManText : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();

        if (!_text)
        {
            Debug.LogError("Failed to get Text Mesh Pro Text on " + gameObject.name.ToString() + " , creating one now");
            _text = gameObject.AddComponent<TextMeshProUGUI>();
        }


        _text.text = " ";
        
        PickupDisplayBox.OnPickupScoreGood += DisplayGoodText;
        PickupDisplayBox.OnPickupScoreBad += DisplayBadText;

    }
    private void OnDisable()
    {
        PickupDisplayBox.OnPickupScoreGood -= DisplayGoodText;
        PickupDisplayBox.OnPickupScoreBad -= DisplayBadText;        
    }

    private void Start()
    {
        
    }

    void DisplayGoodText()
    {
        StartCoroutine(GoodTextCoroutine());
    }

    void DisplayBadText()
    {
        StartCoroutine(BadTextCoroutine());
    }

    IEnumerator GoodTextCoroutine()
    {
        string[] nameList = { "champ", "buster", "pal", "stud", "dude" };

        var goodText = "Nice goin' " + nameList[Random.Range(0, nameList.Length)].ToString();

        while (_text.text.Length < goodText.Length)
        {
            foreach (var c in goodText)
            {
                _text.text += c;
                yield return new WaitForSeconds(0.1f);
            }
        }

        _text.color = Color.green;
    }
    IEnumerator BadTextCoroutine()
    {
        string[] awardList = { "being bad", "being dumb", "being incompetent", "being stupid" };

        var badText = "I should give you an award right now for " + awardList[Random.Range(0, awardList.Length)].ToString();

        while (_text.text.Length < badText.Length)
        {
            foreach(var c in badText)
            {
                _text.text += c;
                yield return new WaitForSeconds(0.1f);
            }
        }

        _text.color = Color.red;
    }


}
