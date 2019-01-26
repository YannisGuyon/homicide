using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fight
{
    public Main main;
    public int life_house;
    public int life_enemy;

    public bool IsDone()
    {
        return (life_house <= 0 || life_enemy <= 0);
    }
    virtual public void Init()
    {
        life_house = 100;
        life_enemy = 100;
    }
    virtual public void Update() { }
}

public class FightHouse : Fight
{
    public float last_damage_dealt;

    override public void Init()
    {
        base.Init();
        last_damage_dealt = 0;
    }
    override public void Update()
    {
        base.Update();
        last_damage_dealt += Time.deltaTime;
        if (last_damage_dealt >= 5)
        {
            // Fx damage taken
            life_house -= 19;
            last_damage_dealt = 0;
            main.camera_manager.Trebble(0.05f);
        }
        if (Input.GetKey(KeyCode.KeypadEnter))
        {
            // Handle input
        }
    }
}