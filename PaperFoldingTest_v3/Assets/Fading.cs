using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fading : MonoBehaviour
{
    public bool fadeInWhenStart = true;
    public Image img;

     // Use this for initialization
    void Start()
    {
        if (fadeInWhenStart) StartCoroutine(FadeIn());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FadeIn()
    {
        for (float i = 1; i >= 0; i -= 0.7f * Time.deltaTime)
        {
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }

    public IEnumerator FadeOutAndChangeScene(int sceneIndex)
    {
        // fade from opaque to transparent
        for (float i = 0; i <= 1; i += 0.7f * Time.deltaTime)
        {
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
        SceneManager.LoadScene(sceneIndex);
    }
    public IEnumerator FadeOutAndChangeScene(string sceneName)
    {
        // fade from opaque to transparent
        for (float i = 0; i <= 1; i += 0.7f * Time.deltaTime)
        {
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
}
