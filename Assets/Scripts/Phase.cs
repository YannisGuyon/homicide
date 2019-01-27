using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Phase
{
    public Main main;

    abstract public bool IsDone(float elapsed_time);
    virtual public void Init() { }
    virtual public void Update(float elapsed_time) { }
    abstract public Phase GetNextPhase();
}

public abstract class PhaseDuration : Phase
{
    override public bool IsDone(float elapsed_time)
    {
        return elapsed_time >= GetDuration();
    }
    abstract public float GetDuration();
}

// - - - -

public class PhaseWorldIntro : PhaseDuration
{
    override public float GetDuration()
    {
        return 10;
    }
    override public void Init()
    {
        Debug.Log(GetType());
        main.camera_manager.SetColor(main.camera_manager.calm_color);
        main.audio_source.clip = main.cineMusic;
        main.audio_source.Play();
    }
    override public Phase GetNextPhase()
    {
        return new PhaseCityGrowth();
    }
}

public class PhaseCityGrowth : PhaseDuration
{
    override public float GetDuration()
    {
        return 4;
    }
    override public void Init()
    {
        Debug.Log(GetType());
        // Grow buildings animation
    }
    public override void Update(float elapsed_time)
    {
        main.soundGenerator.GenerateBuildingSound(elapsed_time);
    }
    override public Phase GetNextPhase()
    {
        return new PhaseAngryness();
    }
}

public class PhaseAngryness : PhaseDuration
{
    override public float GetDuration()
    {
        return 5;
    }
    override public void Init()
    {
        Debug.Log(GetType());
        // Launch house transformation animation
        // Zoom camera
        main.audio_source.clip = main.mainTheme;
        main.audio_source.loop = true;
        main.audio_source.Play();
        main.soundGenerator.GenerateScream();
    }
    override public void Update(float elapsed_time)
    {
        if (elapsed_time >= 1)
        {
            main.camera_manager.SetColor(Color.Lerp(main.camera_manager.calm_color, main.camera_manager.angry_color, (elapsed_time - 1.0f) * 5.0f));
        }
    }
    override public Phase GetNextPhase()
    {
        return new PhaseGoToNextFight();
    }
}

public class PhaseGoToNextFight : PhaseDuration
{
    override public float GetDuration()
    {
        return 8;
    }
    override public void Init()
    {
        main.camera_manager.EnableTrebble();
        Debug.Log(GetType());
        main.soundGenerator.GenerateRun();
        // Camera behind house
        // House move
    }
    override public Phase GetNextPhase()
    {
        return new PhaseFightIntro();
    }
}

public class PhaseFightIntro : PhaseDuration
{
    override public float GetDuration()
    {
        return 4;
    }
    override public void Init()
    {
        Debug.Log(GetType());
        // Camera on the side
    }
    override public Phase GetNextPhase()
    {
        return new PhaseFightWarmup();
    }
}

public class PhaseFightWarmup : PhaseDuration
{
    override public float GetDuration()
    {
        return 4;
    }
    override public void Init()
    {
        Debug.Log(GetType());
        main.ui_manager.Show();
        main.ui_manager.dialog_home.Enable();
        main.fight.main = main;
        main.fight.Init();
        main.audio_source.clip = main.introBattleTheme;
        main.audio_source.loop = false;
        main.audio_source.Play();
        for (int i = 0; i < main.animators.Length; ++i)
            main.animators[i].enabled = false;
    }
    override public Phase GetNextPhase()
    {
        return new PhaseFight();
    }
}

public class PhaseFight : Phase
{
    override public bool IsDone(float elapsed_time)
    {
        return main.fight.IsDone();
    }
    override public void Init()
    {
        Debug.Log(GetType());
        main.audio_source.clip = main.battleTheme;
        main.audio_source.loop = true;
        main.audio_source.Play();
    }
    public override void Update(float elapsed_time)
    {
        main.fight.Update();
    }
    override public Phase GetNextPhase()
    {
        return new PhaseFightOutro();
    }
}

public class PhaseFightOutro : PhaseDuration
{
    override public float GetDuration()
    {
        return 2;
    }
    override public void Init()
    {
        Debug.Log(GetType());
        // Fx Boom tchak
        for (int i = 0; i < main.animators.Length; ++i)
            main.animators[i].enabled = true;
        main.ui_manager.dialog_home.Disable();
        main.ui_manager.Hide();
        if (main.fight.GetType() == typeof(FightHouse)) main.fight = null;  // Set next fight
    }
    override public Phase GetNextPhase()
    {
        if (main.fight != null)
        {
            main.soundGenerator.GenerateLaugh();
            return new PhaseGoToNextFight();
        }
        return new PhaseWorldOutro();
    }
}

public class PhaseWorldOutro : PhaseDuration
{
    override public float GetDuration()
    {
        return 23;
    }
    override public void Init()
    {
        Debug.Log(GetType());
        // Cut
        // Happy house
        main.audio_source.clip = main.endingTheme;
        main.audio_source.loop = true;
        main.audio_source.Play();
    }
    override public Phase GetNextPhase()
    {
        return null;
    }
}
