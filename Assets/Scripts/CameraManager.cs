using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera cam;
    public ShaderCamera shader_camera;
    public Color calm_color;
    public Color angry_color;
    private float trebble;
    private bool enable_trebble;

    void Start()
    {
        trebble = 0;
        enable_trebble = false;
    }
    
    void Update()
    {
        if (enable_trebble)
        {
            shader_camera.gameObject.transform.localRotation = Quaternion.Slerp(Quaternion.identity, Random.rotationUniform, 0.00005f);
            shader_camera.gameObject.transform.localPosition = Random.insideUnitSphere * (0.00005f + trebble);
        }
        trebble *= Mathf.Clamp01(1 - Time.deltaTime * 10.0f);
    }

    public void SetColor(Color color)
    {
        shader_camera.material.color = color;
    }
    public void Trebble(float intensity)
    {
        trebble += intensity;
    }
    public void EnableTrebble()
    {
        enable_trebble = true;
    }
    public void DisableTrebble()
    {
        enable_trebble = false;
    }
}
