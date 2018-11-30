using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mouse : MonoBehaviour {

    private RaycastHit hit;
    private Ray ray;

	// Update is called once per frame
	void Update () {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hit = new RaycastHit();

        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("StageSelect", LoadSceneMode.Single);
        }
	}
}
