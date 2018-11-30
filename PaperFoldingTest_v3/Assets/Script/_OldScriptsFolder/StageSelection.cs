using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageSelection : MonoBehaviour {

    private static GameMaster GM;
    public GameObject stageSelectionPanel;

	// Use this for initialization
	void Start ()
    {
        CheckForGameMaster();

        float xPos = -1;
        float yPos = 7;

        for(int x = 0; x < GameMaster.chpAndLevel[GameMaster.selectedStage]; x++)
        {
            if (x % 2 == 0) yPos -= 4;
            GameObject stage = Instantiate(stageSelectionPanel, new Vector3(3.14f * xPos, yPos, 0), Quaternion.identity);
            
            xPos *= -1;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void CheckForGameMaster()
    {
        GM = (GameMaster)FindObjectOfType(typeof(GameMaster));

        if (GM == null)
        {
            GM = new GameObject("GameMaster").AddComponent<GameMaster>();

            GameMaster.selectedStage = 4;

            DontDestroyOnLoad(GM);
        }
    }
}
