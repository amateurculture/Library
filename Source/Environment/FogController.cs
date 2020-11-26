using UnityEngine;

public class FogController : MonoBehaviour
{
    [Range(0, 1)] public float fogDensity;
    public Gradient fogColor;

    [Range(0, 1)] float gradientIndex;
    LightingController lightingController;
    float fogLerp;
    float currentFogDensity;
    float t1;
    bool isLerping;
    int frameSkip;
    float originalDensity;

    private void Reset()
    {
        GradientAlphaKey[] alphaKeys = {
            new GradientAlphaKey(1, 0),
            new GradientAlphaKey(1, 1)
        };

        GradientColorKey[] keys2 = {
            /*
            new GradientColorKey(new Color32(15, 15, 15, 1), 0),
            new GradientColorKey(new Color32(114, 124, 135, 1), .04f),
            new GradientColorKey(new Color32(255, 255, 255, 1), .08f),
            new GradientColorKey(new Color32(255, 255, 255, 1), .5f),
            new GradientColorKey(new Color32(149, 77, 79, 1), .54f),
            new GradientColorKey(new Color32(15, 15, 15, 1), .58f),
            new GradientColorKey(new Color32(15, 15, 15, 1), 1)
            */
            new GradientColorKey(new Color32(7, 8, 11, 1), 0),
            new GradientColorKey(new Color32(7, 8, 11, 1), .2f),
            new GradientColorKey(new Color32(84, 125, 147, 1), .25f),
            new GradientColorKey(new Color32(183, 191, 195, 1), .32f),
            new GradientColorKey(new Color32(183, 191, 195, 1), .68f),
            new GradientColorKey(new Color32(108, 76, 76, 1), .75f),
            new GradientColorKey(new Color32(7, 8, 11, 1), .8f),
            new GradientColorKey(new Color32(7, 8, 11, 1), 1f)
        };
        fogColor = new Gradient();
        fogColor.SetKeys(keys2, alphaKeys);

        fogDensity = .02f;
    }

    private void Start()
    {
        lightingController = GetComponent<LightingController>();
        fogLerp = .05f;
        frameSkip = 60;
        isLerping = false;
        RenderSettings.fogColor = fogColor.Evaluate(RenderSettings.sun.transform.eulerAngles.x / 360);
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fog = true;
        currentFogDensity = fogDensity;
        
        UpdateFogColor();
    }

    public float GetGradientIndex()
    {
        if (lightingController == null) {
            lightingController = GetComponent<LightingController>();
        }

        if (lightingController != null && lightingController.timeController != null) {
            gradientIndex = lightingController.timeController.minute * .017f;
            gradientIndex += lightingController.timeController.hour;
            gradientIndex *= 0.04f;
        } else {
            gradientIndex = .5f;
        }
        return gradientIndex;
    }

    public void UpdateFogColor()
    {
        //if (RenderSettings.sun == null || lightingController == null) return;

        RenderSettings.fogColor = fogColor.Evaluate(GetGradientIndex());
    }

    void UpdateFogDensity()
    {
        currentFogDensity = Mathf.Lerp(originalDensity, fogDensity, t1);
        //print("Fog density: " + currentFogDensity);

        if (Mathf.Abs(currentFogDensity - fogDensity) <= .0001f)
        {
            currentFogDensity = originalDensity = fogDensity;
            isLerping = false;
        }
        RenderSettings.fogDensity = currentFogDensity;
        Debug.Log(RenderSettings.fogDensity);
    }

    private void Update()
    {
        if (isLerping)
        {
            t1 += Time.deltaTime * fogLerp;
            UpdateFogDensity();
        }
        else if (Time.frameCount % frameSkip == 0 && !isLerping)
        {
            if (currentFogDensity != fogDensity)
            {
                originalDensity = currentFogDensity;
                isLerping = true;
                t1 = 0;
            }
        }
    }
}
