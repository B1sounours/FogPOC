using UnityEngine;
using System.Collections;

public class FogTrigger : MonoBehaviour
{
    public FogSettings fogSett;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        fogSett.ApplySettings();
    }
}

[System.Serializable]
public class FogSettings
{
    
    // TODO: Use Reflection to get RenderSettings
    // TODO: Add tickboxes to allow selective application

    public Color ambientLight;
    public float FlareStrength;
    public float FlareFadeSpeed;

    [HideInInspector]
    public bool applyAmbientLight;
    [HideInInspector]
    public bool applyFlareStrength;
    [HideInInspector]
    public bool applyFlareFadeSpeed;

    public bool FogEnabled;
    public Color FogColor;
    public float FogDensity;
    public float FogEndDistance;
    public FogMode FogMode;
    public float FogStartDistance;

    [HideInInspector]
    public bool applyFogEnabled;
    [HideInInspector]
    public bool applyFogColor;
    [HideInInspector]
    public bool applyFogDensity;
    [HideInInspector]
    public bool applyFogEndDistance;
    [HideInInspector]
    public bool applyFogMode;
    [HideInInspector]
    public bool applyFogDist;

    public float HaloStrength;
    public Material Skybox;

    [HideInInspector]
    public bool applyHaloStrength;
    [HideInInspector]
    public bool applySkybox;

    public void ApplySettings()
    {
        RenderSettings.ambientLight = ambientLight;
        RenderSettings.flareFadeSpeed = FlareFadeSpeed;
        RenderSettings.flareStrength = FlareStrength;

        RenderSettings.fog = FogEnabled;
        RenderSettings.fogColor = FogColor;
        RenderSettings.fogDensity = FogDensity;
        RenderSettings.fogEndDistance = FogEndDistance;
        RenderSettings.fogMode = FogMode;
        RenderSettings.fogStartDistance = FogStartDistance;

        RenderSettings.haloStrength = HaloStrength;
        RenderSettings.skybox = Skybox;
    }

    public void LoadSettings()
    {
        ambientLight = RenderSettings.ambientLight;
        FlareFadeSpeed = RenderSettings.flareFadeSpeed;
        FlareStrength = RenderSettings.flareStrength;

        FogEnabled = RenderSettings.fog;
        FogColor = RenderSettings.fogColor;
        FogDensity = RenderSettings.fogDensity;
        FogEndDistance = RenderSettings.fogEndDistance;
        FogMode = RenderSettings.fogMode;
        FogStartDistance = RenderSettings.fogStartDistance;

        HaloStrength = RenderSettings.haloStrength;
        Skybox = RenderSettings.skybox;
    }
}