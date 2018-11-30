using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadGamePlay()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("Stage1-1");
    }
}
