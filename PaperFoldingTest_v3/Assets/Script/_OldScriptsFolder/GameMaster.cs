using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public static GameMaster GM;

    public static Dictionary<int, int> chpAndLevel;

    public static int selectedStage;

    void Awake()
    {
        if (GM != null)
        {
            GameObject.Destroy(GM);
        }
        else
            GM = this;

        PopulateLevel();

        selectedStage = 0;
        
        DontDestroyOnLoad(this);
    }

    void PopulateLevel()
    {
        if (chpAndLevel != null)
        {
            chpAndLevel.Clear();
        }
        else
        {
            chpAndLevel = new Dictionary<int, int>();
        }

        // チャプターとレベル数を編集
        chpAndLevel.Add(1, 4);
        chpAndLevel.Add(2, 4);
        chpAndLevel.Add(3, 4);
        chpAndLevel.Add(4, 4);
    }
}
