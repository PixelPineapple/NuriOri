using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameProgress
{
    Initial = 0,
    Play = 1,
    Pause = 2,
}

public class PlaySystem : MonoBehaviour
{
    //game clear/over用
    private GameObject stage;
    private GameObject hero;
    private GameObject goal;
    [HideInInspector]
    public GameObject clearMenu;
    [HideInInspector]
    public GameObject overMenu;

    //pause用
    GameProgress progress;
    bool menuPauseFlag;
    bool heroStopFlag;
    private GameObject pauseButton;
    private GameObject pauseMenu;
    private GameObject modeTrigger;
    private GameObject translucentPanel;

    //塗る/畳みモード用    
    private bool drawMode;

    //デバッグ用
    [SerializeField]
    bool soundDebug;
    
    // Use this for initialization
    void Awake()
    {
        stage = GameObject.Find("Stage").gameObject;
        hero = stage.transform.Find("Hero").gameObject;
        goal = stage.transform.Find("Goal").gameObject;

        progress = GameProgress.Initial;
        menuPauseFlag = false;
        heroStopFlag = true;
        GameObject canvas = transform.Find("Canvas").gameObject;
        pauseButton = canvas.transform.Find("PauseButton").gameObject;
        modeTrigger = canvas.transform.Find("ModeTrigger").gameObject;
        pauseMenu = canvas.transform.Find("PauseMenu").gameObject;
        clearMenu = canvas.transform.Find("ClearMenu").gameObject;
        overMenu = canvas.transform.Find("OverMenu").gameObject;
        translucentPanel = canvas.transform.Find("TranslucentPanel").gameObject;

        drawMode = false;

        // サウンド追加
        if (!soundDebug)
        {
            SoundManager.Instance.Play();
            SoundManager.Instance.SetEndFlag(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //clear
        if (goal.GetComponent<Goal>().IsGoal()) GameClear();

        //player death
        if (hero == null) GameOver();
    }

    private void SetUI(bool UIstate)
    {
        pauseButton.SetActive(UIstate);
        modeTrigger.SetActive(UIstate);
        translucentPanel.SetActive(!UIstate);
    }

    public bool GetSoundDebug()
    {
        return soundDebug;
    }


    #region Game Clear/Over
    private void GameClear()
    {
        clearMenu.SetActive(true);
        SetUI(false);
        if (!soundDebug) SoundManager.Instance.Play_ClearBGM();
    }
    private void GameOver()
    {
        overMenu.SetActive(true);
        SetUI(false);
    }
    #endregion

    #region Game Progress
    public void SetProgress(GameProgress value) { progress = value; }
    public GameProgress GetProgress() { return progress; }

    public void HeroPause()
    {
        heroStopFlag = !heroStopFlag;
        GamePause();
    }

    public void MenuPause()
    {
        menuPauseFlag = !menuPauseFlag;
        if (!soundDebug) SoundManager.Instance.Play_TapSE();
        GamePause();
    }

    private void GamePause()
    {
        SetUI(!menuPauseFlag);
        pauseMenu.SetActive(menuPauseFlag);
        if (!soundDebug) SoundManager.Instance.SetFlag(menuPauseFlag);
        if (heroStopFlag || menuPauseFlag)
        {
            //Time.timeScale = 0.0f;
            progress = GameProgress.Pause;
        }
        else {
            //Time.timeScale = 1.0f;
            progress = GameProgress.Play;
        }
    }
    #endregion

    #region Draw Mode
    public void SetDrawMode(bool drawMode)
    {
        this.drawMode = drawMode;
    }
    public bool IsDrawMode()
    {
        return drawMode;
    }
    #endregion


    #region Getter Setter Method
    /// <summary>
    /// ヒーローの動く状態
    /// </summary>
    /// <returns>heroStopFlag値</returns>
    public bool GetHeroStopFlag()
    {
        return heroStopFlag;
    }

    /// <summary>
    /// ヒーローの動く状態値を設定
    /// </summary>
    /// <param name="value">新しい値</param>
    public void SetHeroStopFlag(bool value)
    {
        heroStopFlag = value;
    }

    public bool GetMenuPauseFlag()
    {
        return menuPauseFlag;
    }

    public void TranslucentPanelControl(bool value)
    {
        translucentPanel.SetActive(value);
    }
    #endregion
}
