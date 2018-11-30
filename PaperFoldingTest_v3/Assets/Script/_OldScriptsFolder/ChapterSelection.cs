using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChapterSelection : MonoBehaviour {

    public Button chapter1;
    public Button chapter2;
    private static GameMaster GM;

    // Use this for initialization
    void Start ()
    {
        CheckForGameMaster();
        
        // チャプター選択ボタンのOnClickEvent
        chapter1.onClick.AddListener(delegate { StageSelect(1); });
        chapter2.onClick.AddListener(delegate { StageSelect(2); });
    }

    void StageSelect(int stage)
    {
        GameMaster.selectedStage = stage;
        SceneManager.LoadScene("StageSelect_Buttons", LoadSceneMode.Single);
    }

    void CheckForGameMaster()
    {
        GM = (GameMaster)FindObjectOfType(typeof(GameMaster));

        if (GM == null)
        {
            GM = new GameObject("GameMaster").AddComponent<GameMaster>();
            
            DontDestroyOnLoad(GM);
        }
    }
}
