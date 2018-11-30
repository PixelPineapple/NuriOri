using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkTutorial : MonoBehaviour {

    //public GameObject[] components;
    public GameObject[] tutorials;
    public int index;
    private bool isActivated;
    private bool isBlur;
    public GameObject doubleTapPrefabs;
    private PlaySystem playSystem;

    // Use this for initialization
    void Start () {
        index = 0;
        isActivated = false;
        isBlur = true;
        playSystem = GameObject.Find("GameManager").GetComponent<PlaySystem>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag != "Hero") return;
        playSystem.SetHeroStopFlag(true);
        playSystem.TranslucentPanelControl(true);

        StartCoroutine(DisplayTutorial());
    }

    IEnumerator DisplayTutorial()
    {
        while (!isActivated)
        {
            Debug.Log("Start of Iteration");
            switch (index)
            {
                case 0:
                    Debug.Log("entering case");
                    for (int i = 0; i < tutorials.Length; i++)
                    {
                        Debug.Log("entering for" + i);
                        if (i == index) { tutorials[index].SetActive(true); Debug.Log("Activated"); }
                        else tutorials[i].SetActive(false);
                    }

                    yield return StartCoroutine(WaitForKeyDown());
                    break;

                case 1:
                    for (int i = 0; i < tutorials.Length; i++)
                    {
                        if (i == index) tutorials[index].SetActive(true);
                        else tutorials[i].SetActive(false);
                    }

                    yield return StartCoroutine(WaitForKeyDown());
                    break;

                case 2:
                    for (int i = 0; i < tutorials.Length; i++)
                    {
                        if (i == index) tutorials[index].SetActive(true);
                        else tutorials[i].SetActive(false);
                    }

                    yield return StartCoroutine(WaitForKeyDown());
                    break;
            }

            if (index <= 2) { Debug.Log("End of iteration"); yield return new WaitForEndOfFrame(); }
            else
            {
                playSystem.TranslucentPanelControl(false);
                isActivated = true;
                Debug.Log("play System " + playSystem.GetHeroStopFlag());
                Debug.Log("pause Flag " + playSystem.GetMenuPauseFlag());
                Debug.Log("Game Progress " + playSystem.GetProgress());
                
                foreach(GameObject x in tutorials)
                {
                    x.SetActive(false);
                }

                //Destroy(gameObject);
            }
        }
    }


    IEnumerator WaitForKeyDown()
    {
        while (!Input.GetMouseButtonUp(0))
        {
            yield return null;
        }
        index++;
        Debug.Log("Index" + index);
    }
}
