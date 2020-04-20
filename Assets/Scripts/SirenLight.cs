using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SirenLight : MonoBehaviour
{
    [SerializeField]
    private Color mainColor;
    [SerializeField]
    private Color secondaryColor;
    [SerializeField]
    private float topIntensity;
    [SerializeField]
    private float lowestIntensity;
    [SerializeField]
    private float cycleLength;
    [SerializeField]
    private Light2D light2D;
    [SerializeField]
    private bool brighteningUp = true;

    private void Update()
    {
        UpdateLight();
    }

    private void UpdateLight()
    {
        float changePerStep = (topIntensity - lowestIntensity) / cycleLength;
        if (brighteningUp)
            light2D.intensity += changePerStep * Time.deltaTime;
        else
            light2D.intensity -= changePerStep * Time.deltaTime;

        if (light2D.intensity <= lowestIntensity)
        {
            brighteningUp = true;

            if (light2D.color == mainColor)
                light2D.color = secondaryColor;
            else
                light2D.color = mainColor;
        }
        else if(light2D.intensity >= topIntensity)
        {
            brighteningUp = false;
        }
    }
}
