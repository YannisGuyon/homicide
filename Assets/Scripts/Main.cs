﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public CameraManager camera_manager;
    public SceneManager scene_manager;
    public UiManager ui_manager;

    private Phase phase;
    private float elapsed_time;
    public Fight fight;

    void Start()
    {
        camera_manager.shader_camera.material = Instantiate(camera_manager.shader_camera.material);
        phase = new PhaseWorldIntro();
        phase.main = this;
        phase.Init();
        elapsed_time = 0;
        ui_manager.Init();
        fight = new FightHouse(new Cottage());
    }

    void Update()
    {
        // Debug
        if (Input.GetKey(KeyCode.U))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                Time.timeScale = 0;
            else
                Time.timeScale = Mathf.Clamp(Time.timeScale - Time.deltaTime, 0, 5);
            Debug.Log("Time.timeScale = " + Time.timeScale);
        }
        if (Input.GetKey(KeyCode.I))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                Time.timeScale = 1;
            else
                Time.timeScale = Mathf.Clamp(Time.timeScale + Time.deltaTime, 0, 5);
            Debug.Log("Time.timeScale = " + Time.timeScale);
        }
        
        if (phase != null) phase.Update(elapsed_time);
        if (phase.IsDone(elapsed_time))
        {
            elapsed_time = 0;
            if (phase.GetType() == typeof(PhaseFight) && fight != null && fight.life_home <= 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver", UnityEngine.SceneManagement.LoadSceneMode.Single);
                // Game over
                Debug.Log("Game Over");
            }
            else
            {
                phase = phase.GetNextPhase();
                phase.main = this;
                phase.Init();
            }
        }
        ui_manager.UpdateUi(this);
        elapsed_time += Time.deltaTime;
        if (phase == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("End", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
