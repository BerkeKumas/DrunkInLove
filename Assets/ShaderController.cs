using UnityEngine;

public class ShaderController : MonoBehaviour
{
    public Material drunkMaterial;

    [Range(0.0f, 1.0f)]
    public float wobbleIntensity = 1f;

    void Update()
    {
        drunkMaterial.SetFloat("_WobbleIntensity", wobbleIntensity * 0.075f);
    }
}
