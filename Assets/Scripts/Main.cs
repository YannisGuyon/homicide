using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public CameraManager camera_manager;
    public SceneManager scene_manager;
    public UiManager ui_manager;

    public Phase phase;
    public double elapsed_time;
    public Fight fight;

    void Start()
    {
        phase = new PhaseWorldIntro();
        elapsed_time = 0;
        fight = new FightHouse();
    }

    void Update()
    {
        if (phase != null) phase.Update(elapsed_time);
        if (phase.IsDone(elapsed_time))
        {
            elapsed_time = 0;
            if (fight != null && fight.life_house <= 0)
            {
                // Game over
            }
            else
            {
                phase = phase.GetNextPhase();
            }
        }
        elapsed_time += Time.deltaTime;
        if (phase == null)
        {
            // Go to end scene
        }
    }
}
