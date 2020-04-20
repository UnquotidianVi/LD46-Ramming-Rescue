using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Vector3 originalPosition;
    bool isShaking;
    float shakeIntensity;
    float shakeLength;

    private void Update()
    {
        if (isShaking)
            Shake();
    }

    public void ShakeCamera(float intensity, float length)
    {
        if (isShaking)
            return;

        shakeIntensity = intensity;
        shakeLength = length;
        isShaking = true;
        originalPosition = transform.position;
    }

    private void Shake()
    {
        if(shakeLength <= 0)
        {
            shakeLength = 0;
            transform.position = originalPosition;
            isShaking = false;
            return;
        }

        transform.position = new Vector3(originalPosition.x + Random.Range(-shakeIntensity, shakeIntensity), originalPosition.y + Random.Range(-shakeIntensity, shakeIntensity), originalPosition.z);
        shakeLength -= Time.deltaTime;
    }


}
