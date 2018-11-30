using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    private int chapter;
    private int stage;

    //デバッグ用
    [SerializeField]
    bool soundDebug;

    private void Awake()
    {
        chapter = -1;
        stage = -1;
        if (!soundDebug) SoundManager.Instance.Play();
    }

    private void Update()
    {
        //Debug.Log(chapter);
        //Debug.Log(stage);
    }

    public void Chapter(int chapter)
    {
        this.chapter = chapter;
    }

    public void Stage(int stage)
    {
        this.stage = stage;
        GoToSelectedStage();
    }

    public void ChapterPanel(bool isOpen)
    {


        Debug.Log("      通過");

        switch (chapter)
        {
            case 0:
                transform.Find("Chapter0 Menu").gameObject.SetActive(isOpen);
                if (!soundDebug) SoundManager.Instance.Play_TapSE();
                break;

            case 1:
                transform.Find("Chapter1 Menu").gameObject.SetActive(isOpen);
                if (!soundDebug) SoundManager.Instance.Play_TapSE();
                break;

            case 2:
                transform.Find("Chapter2 Menu").gameObject.SetActive(isOpen);
                if (!soundDebug) SoundManager.Instance.Play_TapSE();
                break;

            case 3:
                transform.Find("Chapter3 Menu").gameObject.SetActive(isOpen);
                if (!soundDebug) SoundManager.Instance.Play_TapSE();
                break;

            case 4:
                transform.Find("Chapter4 Menu").gameObject.SetActive(isOpen);
                if (!soundDebug) SoundManager.Instance.Play_TapSE();
                break;
        }
        if (!soundDebug) SoundManager.Instance.Play_BackSE();
    }

    private void GoToSelectedStage()
    {
        //StartCoroutine(GetComponent<Fading>().FadeOutAndChangeScene(stage+1));

        SceneManager.LoadScene(stage + 1);
        if (!soundDebug) SoundManager.Instance.Play_TapSE();
        if (!soundDebug) SoundManager.Instance.Stop();
    }

    public void ChangeScene(string chapterName)
    {
        if (!soundDebug) SoundManager.Instance.Play_TapSE();
        if (!soundDebug) SoundManager.Instance.Stop();
        SceneManager.LoadScene(chapterName);
    }

}
