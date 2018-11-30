using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private float delay;
    private GameObject stage;
    public GameObject parent;

    private bool[] empty;               //down, right , left
    public GameObject prefab;
    private bool source = true;

    // Use this for initialization
    void Start()
    {
        stage = GameObject.Find("Stage");
        empty = new bool[3] { true, true, true };
        delay = 0.4f;
        StartCoroutine(NewWater());
    }

    // Update is called once per frame
    void Update()
    {
        if (parent == null && !source)
        {
            Destroy(gameObject, delay);
            return;
        }
    }

    public void SetEmpty(int index, bool state)
    {
        if (index > 2 || index < 0) return;
        empty[index] = state;
    }

    private IEnumerator NewWater()
    {
        yield return new WaitForSeconds(delay);
        if (empty[0] || empty[1] || empty[2]) ClonePrefab();
    }

    private void ClonePrefab()
    {
        if (transform.localPosition.y < -5.4) return;
        if (empty[0])
        {
            GameObject copy = Instantiate(prefab);
            copy.transform.SetParent(stage.transform);
            copy.transform.localPosition = this.transform.localPosition + new Vector3(0, -0.64f, 0);
            copy.GetComponent<Water>().parent = this.gameObject;
            copy.GetComponent<Water>().source = false;
        }
        else
        {
            if (empty[1])
            {
                GameObject copy = Instantiate(prefab);
                copy.transform.SetParent(stage.transform);
                copy.transform.localPosition = this.transform.localPosition + new Vector3(0.64f, 0, 0);
                copy.GetComponent<Water>().parent = this.gameObject;
                copy.GetComponent<Water>().source = false;
            }
            if (empty[2])
            {
                GameObject copy = Instantiate(prefab);
                copy.transform.SetParent(stage.transform);
                copy.transform.localPosition = this.transform.localPosition + new Vector3(-0.64f, 0, 0);
                copy.GetComponent<Water>().parent = this.gameObject;
                copy.GetComponent<Water>().source = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ground")) return;
        if (other.CompareTag("Hero"))
        {
            other.GetComponent<Hero>().Death();
            return;
        }
        if (!other.CompareTag("Detector"))
        {
            parent.GetComponent<Water>().StartCoroutine(NewWater());
            Destroy(gameObject, delay * 9 / 10.0f);
        }
    }
}
