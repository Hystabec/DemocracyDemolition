using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class screenShake : MonoBehaviour
{
    bool start = false;
    public AnimationCurve curve;
    public float shakeDuration = 1;

    [SerializeField]
    Animator camAnim;

    bool complete = false;

    public bool IsComplete()
    {
        return complete;
    }

    public void reset()
    {
        complete = false;
    }

    public void TriggerShake()
    {
        var intRet = PlayerPrefs.GetInt("UseScreenShake");

        if(intRet == 1)
            StartCoroutine(Shaking());
    }

    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0;
        camAnim.enabled = false;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            Vector3 randomRotation = Random.insideUnitSphere;
            float strength = curve.Evaluate(elapsedTime / shakeDuration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        complete = true;
        transform.position = startPosition;
        camAnim.enabled = true;
    }
}
