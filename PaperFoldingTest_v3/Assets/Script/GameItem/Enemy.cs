using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private GameObject manager;
    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        bool soundDebug = manager.GetComponent<PlaySystem>().GetSoundDebug();
        if (other.gameObject.tag == "Rock")
        {
            if (!soundDebug) SoundManager.Instance.Play_DeathSE();
            Death();
        }

        if (other.gameObject.tag == "Hero")
        {
            other.gameObject.GetComponent<Hero>().Death();
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
