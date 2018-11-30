using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    //Debug.LogError(typeof(T) + "is nothing");
                }
            }

            return instance;
        }
    }
    
    virtual protected void Awake()
    {
        // 他のGameObjectにアタッチされているか調べる.
        // アタッチされている場合は破棄する.
        if (this != Instance)
        {
            Destroy(this);
            Debug.Log(
                typeof(T) +
                " は既に他のGameObjectにアタッチされているため、コンポーネントを破棄しました." +
                " アタッチされているGameObjectは " + Instance.gameObject.name + " です.");
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        // なんとかManager的なSceneを跨いでこのGameObjectを有効にしたい場合は
        // ↓コメントアウト外してください.
       
    }


    //virtual protected void Awake()
    //{
    //    // 他のゲームオブジェクトにアタッチされているか調べる
    //    // アタッチされている場合は破棄する。
    //    CheckInstance();
    //}

    //protected bool CheckInstance()
    //{
    //    if (instance == null)
    //    {
    //        instance = this as T;
    //        return true;
    //    }
    //    else if (Instance == this)
    //    {
    //        return true;
    //    }
    //    Destroy(this);
    //    return false;
    //}

}
