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

    void Start()
    {
        trebble = 0;
    }
    
    void Update()
    {
        shader_camera.gameObject.transform.localPosition = Random.insideUnitSphere * trebble;
        trebble *= Mathf.Clamp01(1 - Time.deltaTime * 5.0f);
    }

    public void SetColor(Color color)
    {
        shader_camera.material.color = color;
    }
    public void Trebble(float intensity)
    {
        trebble += intensity;
    }
}
