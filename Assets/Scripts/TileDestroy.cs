using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileDestroy : MonoBehaviour
{
    private Transform canvas;
    Transform[] childList;

    public void Awake()
    {
        canvas = FindFirstObjectByType<Canvas>().transform;
        childList = gameObject.GetComponentsInChildren<Transform>();
    }

    public void StartDestroyeffect()
    {
        //StartCoroutine(DestroyEffect());
    }

    IEnumerator DestroyEffect()
    {
        for(int i = 1; i < childList.Length; i++)
        {
            childList[i].SetParent(canvas);
            childList[i].GetComponent<Rigidbody2D>().gravityScale = -Random.Range(100f, 200f);
            childList[i].GetComponent<ConstantForce2D>().force = new Vector2(Random.Range(-400f, 400f), 0);
            childList[i].GetComponent<ConstantForce2D>().torque = Random.Range(-50f, 50f);
            //StartCoroutine(Gravity(childList[i]));
        }

        yield return new WaitForSecondsRealtime(1f);

        for(int i = 1; i < childList.Length; i++)
        {
            childList[i].GetComponent<Rigidbody2D>().gravityScale *= -1;
        }

        Destroy(childList[0].gameObject);
    }
}
