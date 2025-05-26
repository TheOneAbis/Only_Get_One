using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextControl : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private List<string> storyString;

    [SerializeField]
    private GameObject UICanvas;

    [SerializeField]
    private float fadeTime = 1.5f;

    [SerializeField]
    private float holdTime = 2f;

    private void OnTriggerEnter(Collider other)
    {
        StartStory();
    }

    public void StartStory()
    {
        UICanvas.SetActive(true);
        StartCoroutine(RunStory());
    }

    IEnumerator RunStory()
    {
        foreach (string s in storyString)
        {
            text.text = s;
            yield return StartCoroutine(FadeIn(text));
            yield return new WaitForSeconds(holdTime);
            yield return StartCoroutine(FadeOut(text));
        }

        this.gameObject.SetActive(false);
    }

    IEnumerator FadeIn(TextMeshProUGUI text)
    {
        Color color = text.color;
        color.a = 0;
        text.color = color;

        while (color.a < 1.0f)
        {
            color.a += Time.deltaTime / fadeTime;
            text.color = color;
            yield return null;
        }

        color.a = 1f;
        text.color = color;
    }

    IEnumerator FadeOut(TextMeshProUGUI text)
    {
        Color color = text.color;
        color.a = 1;
        text.color = color;

        while (color.a > 0.0f)
        {
            color.a -= Time.deltaTime / fadeTime;
            text.color = color;
            yield return null;
        }

        color.a = 0f;
        text.color = color;
    }


}
