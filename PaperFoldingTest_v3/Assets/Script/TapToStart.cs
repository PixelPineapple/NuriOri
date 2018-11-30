using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapToStart : MonoBehaviour {

    // Grow parameters
    public float approachSpeed = 0.02f;
    public float growthBound = 2.0f;
    public float shrinkBound = 0.8f;
    private float currentRatio = 1;


    private Component tapToStart;
    private Vector3 originalFontSize;

    private Coroutine routine;
    private bool isPulsing = true;
    public bool ActiveFromBeginning;

    private void Awake()
    {
        if (gameObject.GetComponent<Image>())
        {
            tapToStart = gameObject.GetComponent<Image>();
        }
        else if (gameObject.GetComponent<SpriteRenderer>())
        {
            tapToStart = gameObject.GetComponent<SpriteRenderer>();
        }
        originalFontSize = GetComponent<Transform>().localScale;

        if (ActiveFromBeginning)
        {
            StartThisCoroutine();
        }
    }
    
    IEnumerator PulsatingText()
    {
        while (isPulsing)
        {
            while (currentRatio != growthBound)
            {
                currentRatio = Mathf.MoveTowards(currentRatio, growthBound, approachSpeed);
                tapToStart.transform.localScale = originalFontSize * currentRatio;

                yield return new WaitForEndOfFrame();
            }

            while (currentRatio != shrinkBound)
            {
                currentRatio = Mathf.MoveTowards(currentRatio, shrinkBound, approachSpeed);
                tapToStart.transform.localScale = originalFontSize * currentRatio;

                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void StartThisCoroutine()
    {
        routine = StartCoroutine(PulsatingText());
    }

    public bool GetIsPulsing()
    {
        return isPulsing;
    }
}
