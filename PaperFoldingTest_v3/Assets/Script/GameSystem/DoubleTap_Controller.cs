using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTap_Controller : MonoBehaviour {

    public float lifeTime = 0.5f;
    public float delay = 2f;
    //public float positionOffset = 0.5f;
    public bool isLife;
    private PlaySystem playSystem;
    private float currentTime;
    public GameObject rippleParticle;
    public GameObject touchHand;
    public GameObject targetObject;
    private Vector3 currentPosition;

	// Use this for initialization
	void Start () {
        currentTime = 0.0f;
        isLife = false;
        playSystem = GameObject.Find("GameManager").GetComponent<PlaySystem>();
        currentPosition = targetObject.transform.position;
        gameObject.transform.position = currentPosition;
    }

    private void Update()
    {
        if (targetObject == null) return; // ターゲットが無い場合

        currentTime += Time.deltaTime;

        if (isLife)
        {
            Alive();
            return;
        }

        if (delay < currentTime && playSystem.GetHeroStopFlag())
        {
            ComparePosition();
            isLife = !isLife;
            rippleParticle.SetActive(isLife);
            touchHand.SetActive(isLife);
            touchHand.GetComponent<TapToStart>().StartThisCoroutine();
            currentTime = 0.0f;
        }

        if (!playSystem.GetHeroStopFlag() || playSystem.GetMenuPauseFlag())
        {
            isLife = false;
            rippleParticle.SetActive(isLife);
            touchHand.SetActive(isLife);
        }
    }

    private void Alive()
    {
        if(lifeTime < currentTime)
        {
            isLife = !isLife;
            rippleParticle.SetActive(isLife);
            touchHand.SetActive(isLife);
            currentTime = 0.0f;
        }

        if (!playSystem.GetHeroStopFlag() || playSystem.GetMenuPauseFlag())
        {
            isLife = false;
            rippleParticle.SetActive(isLife);
            touchHand.SetActive(isLife);
        }
    }
    
    private void ComparePosition()
    {
        if (V3Equal(currentPosition, targetObject.transform.position))
            return;

        currentPosition = targetObject.transform.position;
        transform.position = currentPosition;
    }

    /// <summary>
    /// 二つのベクトル３を比較する
    /// </summary>
    /// <param name="a">一番目</param>
    /// <param name="b">二番目</param>
    private bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.000001; // 1mmの隔たり
    }
}
