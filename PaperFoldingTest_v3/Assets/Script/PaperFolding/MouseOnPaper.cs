using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOnPaper : MonoBehaviour {

    private GameObject foldingMech;
    private bool enableFolding;

    private RaycastHit hit;
    private Ray ray;

    private GameObject manager;

    // Use this for initialization
    void Start () {
        manager = GameObject.Find("GameManager").gameObject;
        foldingMech = GameObject.Find("ProceduralGrid").gameObject;
        
        enableFolding = foldingMech.GetComponent<FoldingMech>().isActiveAndEnabled;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!enableFolding) return;

        hit = new RaycastHit();
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        

        if (Input.GetMouseButton(0))
        {
            HandleInput();
        }
        if (Input.GetMouseButtonUp(0))
        {
            foldingMech.GetComponent<FoldingMech>().ResetHighlight();
        }
	}

    void HandleInput()
    {
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Paper" && 
                !manager.GetComponent<PlaySystem>().IsDrawMode() && // 描画するモードじゃない時
                foldingMech.GetComponent<FoldingMech>().GetIsFolding() == false && // 折らせない時
                !manager.GetComponent<PlaySystem>().GetMenuPauseFlag() &&
                !manager.GetComponent<PlaySystem>().clearMenu.activeInHierarchy &&
                !manager.GetComponent<PlaySystem>().overMenu.activeInHierarchy
                )
            {
                Vector3 point = hit.point;
                foldingMech.GetComponent<FoldingMech>().FoldingHighlight(point);
                foldingMech.GetComponent<FoldingMech>().FirstInput(point);
            }
        }
    }

    public RaycastHit GetHit()
    {
        return hit;
    }
    public Ray GetRay()
    {
        return ray;
    }
}
