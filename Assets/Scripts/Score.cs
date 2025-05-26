
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Score : MonoBehaviour
{
    [SerializeField]
    TMP_Text _scoreText;
    [SerializeField]
    TMP_Text _goalsText;

    [SerializeField]
    List<string> _allGoals;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Display());
    }

    IEnumerator Display()
    {
        yield return new WaitForSeconds(0.75f);

        float scoreDelay = 0.5f;
        for (int i = 0; i <= Data.Points; i++)
        {
            _scoreText.text = "Coins Found: " + i;
            //scoreDelay *= 0.9f;
            if (i > 0)
            {

                yield return new WaitForSeconds(scoreDelay * Mathf.Pow(i / (float)Data.Points, 10));
            }
        }

        yield return new WaitForSeconds(2.3f);

        float goalDelay = 1.4f;
        _goalsText.text = "Checkpoints: ";
        if (_allGoals.Count > 0)
        {
            for (int i = 0; i < _allGoals.Count; i++)
            {
                _goalsText.text += '\n';
                if (Data.Goals.Contains(_allGoals[i]))
                {
                    _goalsText.text += "<color=green> " + _allGoals[i];
                }
                else
                {
                    _goalsText.text += "<color=red> " + _allGoals[i];
                }
                //goalDelay *= 0.65f;
                yield return new WaitForSeconds(goalDelay);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
