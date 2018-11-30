using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectOnContact : MonoBehaviour {

    public GameObject showThisObject;
    public GameObject particleEffect;
    private PlaySystem playSystem;
    private bool alreadyActivated; // 一回しか表示されない

	// Use this for initialization
	void Start () {
        playSystem = GameObject.Find("GameManager").GetComponent<PlaySystem>();
        alreadyActivated = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!alreadyActivated) return;

		if(alreadyActivated && Input.GetMouseButtonUp(0))
        {
            Continue();
        }
	}

    // RigidBodyに入ったら
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Hero" || alreadyActivated) return;

        showThisObject.SetActive(true);
        playSystem.SetProgress(GameProgress.Pause); // ゲームプログレスはポーズに設定
        playSystem.TranslucentPanelControl(true); // 透明画像をオンにする
        alreadyActivated = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Hero" || !alreadyActivated) return;

        Destroy(gameObject);
    }

    private void Continue()
    {
        playSystem.SetProgress(GameProgress.Play);
        playSystem.TranslucentPanelControl(false);
        
        showThisObject.SetActive(false);
    }
}
