using UnityEngine;
using TMPro;
public class StatsHud : MonoBehaviour
{
    [SerializeField]
    TMP_Text _score;
    [SerializeField]
    TMP_Text _goals;
    private void Awake()
    {
        //Clear Stats;
        Data.Points = 0;
        Data.Goals = new();
    }
    // Update is called once per frame
    void Update()
    {
        _score.text = "Score: " + Data.Points.ToString();
        _goals.text = "Goals Reached: ";
        foreach (var p in Data.Goals)
        {
            _goals.text += p.ToString();
        }
    }
}
