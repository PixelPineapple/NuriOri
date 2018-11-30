using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralButton : MonoBehaviour
{
    private int current;
    private GameObject manager;

    // Use this for initialization
    void Start()
    {
        current = SceneManager.GetActiveScene().buildIndex;
        manager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReturnTitle()
    {
        Sound();
        SceneManager.LoadScene("Title");
    }

    public void ReturnToStageSelect()
    {
        Sound();
        SceneManager.LoadScene("ChapterSelect");
    }

    public void Next()
    {
        Sound();
        if (current == SceneManager.sceneCount) ReturnTitle();
        SceneManager.LoadScene(current + 1);
    }

    public void ReloadCurrent()
    {
        Sound();
        SceneManager.LoadScene(current);
    }

    public void Exit()
    {
        Application.Quit();
    }

    void Sound()
    {
        bool soundDebug = manager.GetComponent<PlaySystem>().GetSoundDebug();
        if (soundDebug) return;
        SoundManager.Instance.Stop();
        SoundManager.Instance.Play_TapSE();
    }

    #region beta
    bool pressB = false;
    public void PressB()
    {
        pressB = !pressB;
        Debug.Log("B key is" + pressB);
    }
    public bool GetPressB()
    {
        return pressB;
    }
    #endregion
}
