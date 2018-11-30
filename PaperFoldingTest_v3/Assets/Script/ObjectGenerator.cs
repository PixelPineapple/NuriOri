using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public float yOffset = .20f;

    private GameObject manager;
    

    public List<GameObject> objFormList;//生成リスト

    // 生成した物のリスト (static -> dynamicのRigidBodyを変えるために)
    private List<GameObject> duplicatedObj;

    GameObject obj;

    bool copy;
    
    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("GameManager");
        if (duplicatedObj != null)
        {
            duplicatedObj.Clear();
        }
        else
        {
            duplicatedObj = new List<GameObject>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ////Debug.Log(duplicationDirection.ToString());
        //duplicationDirection = manager.GetComponent<GeneralButton>().GetPressB();
        //copy = false;

        //if (Input.GetMouseButtonDown(0))
        //{
        //    //プレイ会に向けて一時的に使用
        //    bool isDrawMode = manager.GetComponent<PlaySystem>().IsDrawMode();
        //    if (isDrawMode) return;

        //    //マウスの座標
        //    Vector2 aTapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //    if (aTapPoint.y >= 7.5f) return;

        //    //オブジェクト生成
        //    if (objFormList != null)
        //    {
        //        for (int i = 0; i < objFormList.Count; i++)
        //        {
        //            obj = Instantiate(objFormList[i]) as GameObject;

        //            obj.GetComponent<ColorChange>().ChangeAddCheck(true);

        //            Vector2 objPosi = objFormList[i].transform.position;

        //            //以下、生成位置計算・配置

        //            if (!duplicationDirection)
        //            {
        //                //横複製

        //                //プレイ会に向けて一時的に使用
        //                obj.transform.position = new Vector2(aTapPoint.x, objPosi.y);
        //                /* ※消さない
        //                float distans = objPosi.x - aTapPoint.x;
        //                Mathf.Abs(distans);

        //                if (objPosi.x > aTapPoint.x)
        //                {
        //                    obj.transform.position = new Vector2(aTapPoint.x - distans, objPosi.y);
        //                }
        //                else if (objPosi.x < aTapPoint.x)
        //                {
        //                    obj.transform.position = new Vector2(aTapPoint.x - distans, objPosi.y);
        //                }
        //                */
        //            }
        //            else if (duplicationDirection)
        //            {
        //                //横複製

        //                //プレイ会に向けて一時的に使用
        //                obj.transform.position = new Vector2(objPosi.x, aTapPoint.y);

        //                /*  ※消さない
        //               float distans = objPosi.y - aTapPoint.y;
        //               Mathf.Abs(distans);

        //               if (objPosi.y > aTapPoint.y)
        //               {
        //                   obj.transform.position = new Vector2(objPosi.x, aTapPoint.y - distans);
        //               }
        //               else if (objPosi.y < aTapPoint.y)
        //               {
        //                   obj.transform.position = new Vector2(objPosi.x, aTapPoint.y - distans);
        //               }
        //                */
        //            }
        //        }
        //        copy = true;

        //    }
        //    //生成したら削除
        //    objFormList.Clear();

        //}
    }

    public void GenerateDuplicate (float centerPoint, Side side, float paperHeight, float paperWidth)
    {
        for (int i = 0; i < objFormList.Count; i++)
        {
            //Debug.Log("GenerateDuplicate " + objFormList.Count);

            Vector2 objPosi = objFormList[i].transform.position;

            //以下、生成位置計算・配置
            float distance = 0.0f;
            float affectedArea = 0.0f;
            switch (side)
            {
                case Side.LEFT:
                case Side.RIGHT:
                    distance = Mathf.Abs(objPosi.x - centerPoint);

                    if (centerPoint > (paperWidth - centerPoint))
                        affectedArea = paperWidth - centerPoint;
                    else
                        affectedArea = centerPoint;

                    if (distance < affectedArea)
                    {
                        obj = Instantiate(objFormList[i]) as GameObject;
                        obj.GetComponent<ColorChange>().ChangeAddCheck(true);
                        obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

                        objFormList[i].GetComponent<ColorChange>().SetIsDuplicated(true);
                        
                        if (objPosi.x < centerPoint)
                        {
                            obj.transform.position = new Vector3(centerPoint + distance, objPosi.y, 20f);
                        }
                        else
                        {
                            obj.transform.position = new Vector3(centerPoint - distance, objPosi.y, 20f);
                        }

                        // 左右変更ブロックを生成
                        if (obj.transform.tag == "DirectionBlock")
                        {
                            obj.transform.GetComponent<DirectionBlock>().FlipDirection();
                            obj.transform.localScale = new Vector3(-1, 1, 1);
                        }
                    }

                    break;

                case Side.UP:
                case Side.BOTTOM:
                    distance = Mathf.Abs(objPosi.y - centerPoint);
                    Debug.Log(distance);
                    if (centerPoint > (paperHeight - centerPoint))
                        affectedArea = paperHeight - centerPoint;
                    else
                        affectedArea = centerPoint;

                    if (distance < affectedArea)
                    {
                        obj = Instantiate(objFormList[i]) as GameObject;
                        obj.GetComponent<ColorChange>().ChangeAddCheck(true);
                        obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

                        objFormList[i].GetComponent<ColorChange>().SetIsDuplicated(true);

                        if (objPosi.y < centerPoint)
                        {
                            print("Center + distance - offset = " + (centerPoint + distance - yOffset));
                            obj.transform.position = new Vector3(objPosi.x, centerPoint + distance - yOffset, 20f);
                        }
                        else
                        {
                            obj.transform.position = new Vector3(objPosi.x, centerPoint - distance - yOffset, 20f);
                        }
                    }
                    break;

                case Side.NONE:
                    return;
            }
            if (obj != null) duplicatedObj.Add(obj);
            obj = null;
        }
        
        objFormList.RemoveAll(x => x.GetComponent<ColorChange>().GetIsDuplicated());
    }

    //生成リストに追加
    public void AddGameObject(GameObject gameObject)
    {
        if (gameObject == null)
        {
            return;
        }

        objFormList.Add(gameObject);
    }

    public bool GetCopy()
    {
        return copy;
    }

    public List<GameObject> GetDupList()
    {
        return duplicatedObj;
    }

    // 修正終わったら、削除。
    public void ClearDuplicatedObjList()
    {
        duplicatedObj.Clear();
    }
}
