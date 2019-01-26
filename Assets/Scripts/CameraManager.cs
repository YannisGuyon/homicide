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
        shader_camera.gameObject.transform.localRotation = Quaternion.Slerp(Quaternion.identity, Random.rotationUniform, 0.0002f);
        shader_camera.gameObject.transform.localPosition = Random.insideUnitSphere * (0.002f + trebble);
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
