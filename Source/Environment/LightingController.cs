using UnityEngine;

public class LightingController : MonoBehaviour
{
    Gradient sunLight;
    public Color32 sunColor;
    public Gradient ambientLight;
    [HideInInspector] public FogController fogController;
    [HideInInspector] public TimeController timeController;
    //public Light moon;
    [HideInInspector] public ReflectionProbe reflectionProbe;
    int reflectionFrameSkip = 60;
    
    float gradientIndex;
    float lightLevel;
    Camera cameraMain;

    private void Reset()
    {
        var possibleLights = FindObjectsOfType<Light>();

        foreach (var light in possibleLights)
        {
            if (light.type == LightType.Directional && !light.name.Contains("Indirect"))
            {
                RenderSettings.sun = light;
                break;
            }
        }

        sunColor = Color.white;

        GradientAlphaKey[] alphaKeys = {
            new GradientAlphaKey(1, 0),
            new GradientAlphaKey(1, 1)
        };

        GradientColorKey[] keys1 = {
            new GradientColorKey(new Color32(8, 8, 10, 1), 0),
            new GradientColorKey(new Color32(8, 8, 10, 1), .2f),
            new GradientColorKey(new Color32(67, 84, 99, 1), .3f),
            new GradientColorKey(new Color32(128, 128, 128, 1), .4f),
            new GradientColorKey(new Color32(128, 128, 128, 1), .6f),
            new GradientColorKey(new Color32(91, 67, 60, 1), .7f),
            new GradientColorKey(new Color32(8, 8, 10, 1), .8f),
            new GradientColorKey(new Color32(8, 8, 10, 1), 1f)
        };

        timeController = GetComponent<TimeController>();
        ambientLight = new Gradient();
        ambientLight.SetKeys(keys1, alphaKeys);

        fogController = GetComponent<FogController>();
        reflectionProbe = GetComponent<ReflectionProbe>();

        if (reflectionProbe != null)
        {
            reflectionProbe.enabled = true;
            reflectionProbe.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
            reflectionProbe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake;
            reflectionProbe.size = new Vector3(10000, 50, 10000);
            reflectionProbe.RenderProbe();
        }
    }

    private void Start()
    {
        GradientAlphaKey[] alphaKeys = {
            new GradientAlphaKey(1, 0),
            new GradientAlphaKey(1, 1)
        };

        GradientColorKey[] keys = {
            new GradientColorKey(Color.black, 0),
            new GradientColorKey(Color.black, .25f),
            new GradientColorKey(Color.white, .32f),
            new GradientColorKey(Color.white, .68f),
            new GradientColorKey(Color.black, .75f),
            new GradientColorKey(Color.black, 1f)
        };
        sunLight = new Gradient();
        sunLight.SetKeys(keys, alphaKeys);

        cameraMain = Camera.main;

        timeController = GetComponent<TimeController>();
        fogController = GetComponent<FogController>();
        reflectionProbe = GetComponent<ReflectionProbe>();

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;

        if (RenderSettings.sun != null)
        {
            lightLevel = GetGradientIndex();
            RenderSettings.sun.intensity = sunLight.Evaluate(lightLevel).grayscale;
            RenderSettings.ambientLight = ambientLight.Evaluate(lightLevel);
            RenderSettings.sun.color = sunColor;
        }

        if (reflectionProbe != null)
        {
            reflectionProbe.enabled = true;
            reflectionProbe.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
            reflectionProbe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake;
            reflectionProbe.size = new Vector3(10000, 50, 10000);
            reflectionProbe.RenderProbe();
        }
    }

    void Update()
    {
        if (Time.frameCount % reflectionFrameSkip == 0 && reflectionProbe != null && reflectionProbe.isActiveAndEnabled)
        {
            reflectionProbe.backgroundColor = cameraMain.backgroundColor;
            reflectionProbe.RenderProbe();
        }
    }

    public void UpdateLighting()
    {
        UpdateSunLight();
        UpdateAmbientLight();
        //UpdateSkyColor();
    }

    float GetGradientIndex()
    {
        if (timeController != null) {
            gradientIndex = timeController.minute * .017f;
            gradientIndex += timeController.hour;
            gradientIndex *= 0.04f;
        } else {
            gradientIndex = .5f;
        }
        return gradientIndex;
    }

    void UpdateAmbientLight()
    {
        gradientIndex = GetGradientIndex();
        RenderSettings.ambientLight = ambientLight.Evaluate(gradientIndex);
    }

    void UpdateSunLight()
    {
        gradientIndex = GetGradientIndex();
        lightLevel = RenderSettings.sun.intensity = sunLight.Evaluate(gradientIndex).grayscale;

        RenderSettings.sun.enabled = (lightLevel <= 0) ? false : true;
        RenderSettings.sun.color = sunColor;
        //if (moon != null) moon.enabled = (lightLevel <= 0) ? true : false;
    }
}
