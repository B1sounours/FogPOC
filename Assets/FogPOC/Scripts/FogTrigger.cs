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
}

[System.Serializable]
public class FogSettings
{
    public Color ambientLight;
    public float FlareFadeSpeed;
    public float FlareStrength;
    
    public bool FogEnabled;
    public Color FogColor;
    public float FogDensity;
    public float FogEndDistance;
    public FogMode FogMode;
    public float FogStartDistance;

    public float HaloStrength;
    public Material Skybox;

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