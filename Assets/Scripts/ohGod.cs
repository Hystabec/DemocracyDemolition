using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ohGod : MonoBehaviour
{
    public bool start = false;
    public AnimationCurve curve;
    public float shakeDuration = 1;
    public float elapsedTime = 0;
    public Quaternion currentTransform;

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
        Quaternion startPosition = transform.rotation;


        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            Vector3 randomRotation = Random.insideUnitSphere;
            //float strength = curve.Evaluate(elapsedTime / shakeDuration);
            transform.rotation = startPosition * Quaternion.Euler(randomRotation * 1000);
            currentTransform = transform.rotation;
            yield return null;
        }

        transform.rotation = startPosition;
        elapsedTime = 0;
    }
}
