using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{

    override protected void Awake()
    {
        base.Awake();
    }

    bool flag;
    bool endFlag;

    [SerializeField]
    private AudioSource[] _audios;
    

    // Use this for initialization
    void Start()
    {
        flag = false;
        endFlag = false;
    }

    public void Play()
    {
        if (SceneManager.GetActiveScene().name == "Title")  _audios[2].Play(); 
        if (SceneManager.GetActiveScene().name == "ChapterSelect") _audios[3].Play();
        if (SceneManager.GetActiveScene().name.Contains("Stage1")) _audios[0].Play();
        if (SceneManager.GetActiveScene().name.Contains("Stage2")) _audios[10].Play();
    }

    public void Stop()
    {
        if (SceneManager.GetActiveScene().name == "Title") _audios[2].Stop();
        if (SceneManager.GetActiveScene().name == "ChapterSelect") _audios[3].Stop();
        if (SceneManager.GetActiveScene().name.Contains("Stage1")) _audios[0].Stop();
        if (SceneManager.GetActiveScene().name.Contains("Stage2")) _audios[10].Stop();
    }

    public void Play_ClearBGM()
    {
        if (endFlag) return;

        _audios[0].Stop();
        _audios[1].Play();
        endFlag = true;

    }

    public void Play_TapSE()
    {
        if (SceneManager.GetActiveScene().name == "Title") _audios[4].Play();
        if (SceneManager.GetActiveScene().name == "ChapterSelect") _audios[5].Play();
        if (SceneManager.GetActiveScene().name.Contains("Stage")) _audios[5].Play();
    }

    public void Play_FoldSE()
    {
        _audios[11].Play();
    }

    public void Play_BackSE()
    {
        _audios[6].Play();
    }

    public void Play_PaintStartSE()
    {
        _audios[7].Play();
    }

    public void Play_PaintEndSE()
    {
        _audios[7].Stop();
        _audios[8].Play();
    }

    public void Play_DeathSE()
    {
        _audios[9].Play();
    }



    // Update is called once per frame
    void Update()
    {
        //_audios[0].Play();
        if (flag)
        {
            _audios[0].volume = 0.1f;
        }
        else
        {
            _audios[0].volume = 0.4f;
        }
    }

    public void SetFlag(bool flag)
    {
        this.flag = flag;
    }

    public void SetEndFlag(bool endFlag)
    {
        this.endFlag = endFlag;
    }
}
