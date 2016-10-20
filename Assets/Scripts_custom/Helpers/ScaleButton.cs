using UnityEngine;
using System.Collections;

public class ScaleButton : MonoBehaviour {

    Vector3 originalScale;
    Vector3 targetScale;

    bool controllerIntersected;

    float scaleTo = 75;

	void Start()
    {
        originalScale = transform.localScale;
    }


    IEnumerator GrowButton(float time)
    {
        targetScale = originalScale + new Vector3(scaleTo, scaleTo, scaleTo);
        float originalTime = time;

        transform.localScale = Vector3.Lerp(originalScale, targetScale, time / originalTime);
        yield return null;

    }

    IEnumerator ReturnButton(float time)
    {
        float originalTime = time;

        time -= Time.deltaTime;

        transform.localScale = originalScale;

        yield return null;
    }


    void OnTriggerEnter(Collider col)
    {
        controllerIntersected = true;
        StartCoroutine(GrowButton(1.5f));
        Debug.Log("Controller intersected: " + gameObject.name);
    }

    void OnTriggerExit(Collider col)
    {
        controllerIntersected = false;
        StartCoroutine(ReturnButton(0.5f));
    }


}
