using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    [System.Serializable]
    public class Level
    {
        public string levelText;
        public int unLocked;
        public bool isInteractable;
    }

    public List<Level> levelList;
    public GameObject levelButton;
    public Transform spacer;
    private static GameMaster GM;

	// Use this for initialization
	void Start ()
    {
        CheckForGameMaster();
        PopulateList();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void PopulateList ()
    {
        if (levelList == null) levelList = new List<Level>();

        levelList.Clear();

        // ボタンを作る。
        for (int x = 1; x <= GameMaster.chpAndLevel[GameMaster.selectedStage]; x++)
        {
            GameObject newButton = Instantiate(levelButton) as GameObject;
            newButton.GetComponent<LevelButton>().levelText.text = GameMaster.selectedStage.ToString() + "-" + x;   
            newButton.GetComponent<LevelButton>().unlocked = 1;
            newButton.transform.SetParent(spacer);
            newButton.GetComponent<Button>().onClick.AddListener(delegate {
                LoadLevels(newButton.GetComponent<LevelButton>().levelText.text); });
        }
    }

    void LoadLevels(string levelIndex)
    {
        SceneManager.LoadScene("Stage" + levelIndex, LoadSceneMode.Single);
    }


    void CheckForGameMaster()
    {
        GM = (GameMaster)FindObjectOfType(typeof(GameMaster));

        if (GM == null)
        {
            GM = new GameObject("GameMaster").AddComponent<GameMaster>();

            GameMaster.selectedStage = 1;

            DontDestroyOnLoad(GM);
        }
    }

}
