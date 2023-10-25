using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public AnimationCurve curve;
    public float timeShake = 1f;
    public GameObject playerObject;


   void Start()
    {
        GameObject playerObject = GetComponent<GameObject>();
    }

    void Update()
    {
        PlayerControl playerScript = playerObject.GetComponent<PlayerControl>();
        bool slashed = playerScript.slashedThorn;

        if (slashed)
        {
            slashed = false;
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking()
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < timeShake)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / timeShake);
            transform.position = startPos + Random.insideUnitSphere;
            yield return null;
        }

        transform.position = startPos;
    }
}
