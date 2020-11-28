using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class QualityManager : MonoBehaviour
{
    //ReflectionProbe reflectionProbe;
    public int qualityLevel;
    public int frameRate;
    public bool tonemapping;
    //public bool enableReflections;
    public bool ambientOcclusion;
    public bool depthOfField;
    public bool HDR;

    //bool previousEnableReflections;
    Camera postprocessingCamera;
    ColorGrading colorGrading;
    DepthOfField dof;
    AmbientOcclusion ao;
    PostProcessVolume postProcessingVolume;
    int frameSkip;

    private void Reset()
    {
        frameRate = 60;
        qualityLevel = 3;
    }

    void Start()
    {
        //reflectionProbe = GetComponent<ReflectionProbe>();
        //enableReflections = (qualityLevel == 0) ? false : enableReflections;
        frameSkip = 120;
        postprocessingCamera = Camera.main;
        postProcessingVolume = postprocessingCamera.GetComponent<PostProcessVolume>();
        postProcessingVolume.profile.TryGetSettings(out colorGrading);
        postProcessingVolume.profile.TryGetSettings(out dof);
        postProcessingVolume.profile.TryGetSettings(out ao);
        QualitySettings.SetQualityLevel(qualityLevel);

        if (frameRate < 240) Application.targetFrameRate = frameRate;
        
        //previousEnableReflections = enableReflections;
    }

    void Update()
    {
        // todo this logic will eventually be moved into a game menu and taken out of the update loop
        // todo investigate property drawers
        if (Time.frameCount % frameSkip == 0)
        {
            QualitySettings.SetQualityLevel(qualityLevel);
            //enableReflections = (qualityLevel == 0) ? false : true;

            Application.targetFrameRate = frameRate;

            if (colorGrading != null)
                colorGrading.active = tonemapping;
            
            if (dof != null)
                dof.active = depthOfField;
            
            if (ao != null)
                ao.active = ambientOcclusion;
            
            postprocessingCamera.allowHDR = HDR;
        }
    }
}
