using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageProjection : MonoBehaviour {

    public Camera renderCamera;

    public Ray mainCameraRay;
    public RaycastHit mainCameraHit;

    public Ray renderCameraRay;
    public RaycastHit renderHit;
    
	// Update is called once per frame
	void Update ()
    {
        {
            mainCameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mainCameraRay, out mainCameraHit))
            {

                var localPoint = mainCameraHit.textureCoord;

                //Debug.Log(localPoint);

                renderCameraRay = renderCamera.ScreenPointToRay(
                new Vector2(localPoint.x * renderCamera.pixelWidth, localPoint.y * renderCamera.pixelHeight));

                //Debug.Log((localPoint.x * renderCamera.pixelWidth) + " / " + (localPoint.y * renderCamera.pixelHeight));

                if (Physics.Raycast(renderCameraRay, out renderHit))
                {
                    //Debug.DrawLine(renderCamera.transform.position, localPoint);

                    if (renderHit.transform.tag == "Hero" && Input.GetMouseButtonDown(0))
                    {
                        //Debug.Log("test");
                        //renderHit.transform.parent.GetComponent<Hero>().DoubleClick();
                    }
                }
            }
        }

	}

    public RaycastHit GetMCHit()
    {
        return mainCameraHit;
    }

    public Ray GetMCRay()
    {
        return mainCameraRay;
    }


    public RaycastHit GetHit()
    {
        return renderHit;
    }
    
    public Ray GetRay()
    {
        return renderCameraRay;
    }

}
