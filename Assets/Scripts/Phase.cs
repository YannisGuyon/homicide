using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Phase
{
    public Main main;

    abstract public bool IsDone(double elapsed_time);
    virtual public void Init() { }
    virtual public void Update(double elapsed_time) { }
    abstract public Phase GetNextPhase();
}

public abstract class PhaseDuration : Phase
{
    override public bool IsDone(double elapsed_time)
    {
        return elapsed_time >= GetDuration();
    }
    abstract public double GetDuration();
}

public class PhaseWorldIntro : PhaseDuration
{
    override public double GetDuration()
    {
        return 3;
    }
    override public void Init()
    {
        // Set shader to green
    }
    override public Phase GetNextPhase()
    {
        return new PhaseCityGrowth();
    }
}

public class PhaseCityGrowth : PhaseDuration
{
    override public double GetDuration()
    {
        return 3;
    }
    override public void Init()
    {
        // Grow buildings animation
    }
    override public Phase GetNextPhase()
    {
        return new PhaseAngryness();
    }
}

public class PhaseAngryness : PhaseDuration
{
    override public double GetDuration()
    {
        return 3;
    }
    override public void Init()
    {
        // Launch house transformation animation
        // Zoom camera
    }
    override public void Update(double elapsed_time)
    {
        // Fade shader
    }
    override public Phase GetNextPhase()
    {
        return new PhaseGoToNextFight();
    }
}

public class PhaseGoToNextFight : PhaseDuration
{
    override public double GetDuration()
    {
        return 3;
    }
    override public void Init()
    {
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
    override public double GetDuration()
    {
        return 3;
    }
    override public void Init()
    {
        // Camera on the side
    }
    override public Phase GetNextPhase()
    {
        return new PhaseFight();
    }
}

public class PhaseFight : Phase
{
    override public bool IsDone(double elapsed_time)
    {
        return main.fight.IsDone();
    }
    override public void Init()
    {
        // Display UI
        main.fight.main = main;
        main.fight.Init();
    }
    public override void Update(double elapsed_time)
    {
        main.fight.Update();
    }
    override public Phase GetNextPhase()
    {
        return new PhaseCityGrowth();
    }
}

public class PhaseFightOutro : PhaseDuration
{
    override public double GetDuration()
    {
        return 1;
    }
    override public void Init()
    {
        // Fx Boom tchak
        // Hide UI
        if (main.fight.GetType() == typeof(FightHouse)) main.fight = null;  // Set next fight
    }
    override public Phase GetNextPhase()
    {
        if (main.fight != null)
            return new PhaseGoToNextFight();
        return new PhaseWorldOutro();
    }
}

public class PhaseWorldOutro : PhaseDuration
{
    override public double GetDuration()
    {
        return 3;
    }
    override public void Init()
    {
        // Cut
        // Happy house
    }
    override public Phase GetNextPhase()
    {
        return null;
    }
}
