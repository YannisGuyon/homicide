﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fight
{
    public Main main;
    public Home home;
    public Enemy enemy;

    public bool IsDone()
    {
        return (home.life <= 0 || enemy.life <= 0);
    }
    virtual public void Init()
    {
        this.home.life = 100;
        this.enemy.life = 100;

        main.ui_manager.dialog_home.SetAttacks(this.home.attacks);
        main.ui_manager.dialog_enemy.SetAttacks(this.enemy.attacks);
    }
    virtual public void Update() { }
}

public abstract class FightBase : Fight
{
    public int lastDamageDealtByEnemyMs;
    public int lastDamageDealtByHomeMs;
    public SoundGenerator soundGenerator;

    override public void Init()
    {
        base.Init();
        lastDamageDealtByEnemyMs = 0;
        lastDamageDealtByHomeMs = 0;
    }
    override public void Update()
    {
        base.Update();
        lastDamageDealtByEnemyMs += (int)(Time.deltaTime * 1000);
        lastDamageDealtByHomeMs += (int)(Time.deltaTime * 1000);
        if (lastDamageDealtByEnemyMs >= 5)
        {
            main.ui_manager.dialog_enemy.Enable();
            // Fx damage taken
            main.camera_manager.Trebble(0.005f);
            Attack currentEnemyAttack = this.enemy.GeCurrentAttack();
            main.ui_manager.dialog_enemy.SelectAttack(this.enemy.attackPattern[this.enemy.currentPatternIndex]);
            this.enemy.updatePattern();
            home.life -= currentEnemyAttack.damage;
            lastDamageDealtByEnemyMs = -currentEnemyAttack.durationMs;

            // Cottage
            if (currentEnemyAttack.name == "Shutter Shaker") main.Add(new FxShutterShaker(main.cottage_transform));
            if (currentEnemyAttack.name == "Chim chimney") main.Add(new FxChimChimney(main.cottage_transform));

            // factory
            if (currentEnemyAttack.name == "Sound the alarm") main.Add(new FxSoundTheAlarm(main.factory_transform));
            if (currentEnemyAttack.name == "Staff Evacuation") main.Add(new FxStaffEvacuation(main.factory_transform));
            if (currentEnemyAttack.name == "Pollution Puke") main.Add(new FxPollutionPuke(main.factory_transform));

            // Skyscraper
            if (currentEnemyAttack.name == "Spiky spiky") main.Add(new FxSpikySpiky(main.skymachin_transform));
            if (currentEnemyAttack.name == "Shuriken from the sky") main.Add(new FxChimChimney(main.skymachin_transform));
            if (currentEnemyAttack.name == "Snakescraper") main.Add(new FxShutterShaker(main.skymachin_transform));

            // Cathedral
            if (currentEnemyAttack.name == "Belly Belly") main.Add(new FxBellyBelly(main.catedral_transform));
            if (currentEnemyAttack.name == "Sacrushed Chapel") main.Add(new FxSacrushedChapel(main.catedral_transform));
            if (currentEnemyAttack.name == "Growing Faith") main.Add(new FxGrowingFaith(main.catedral_transform));
        }
        else
        {
            main.ui_manager.dialog_enemy.Disable();
        }
        if (lastDamageDealtByHomeMs >= 5)
        {
            main.ui_manager.dialog_home.Enable();
            if (Input.GetKeyDown(KeyCode.Return))
            {
                // Handle input
                Attack currentHomeAttack = this.home.attacks[main.ui_manager.dialog_home.GetSelectedAttack()];
                enemy.life -= currentHomeAttack.damage;
                lastDamageDealtByHomeMs = -currentHomeAttack.durationMs;
                main.ui_manager.dialog_home.Disable();

                if (currentHomeAttack.name == "Rollout") main.Add(new FxRollout(main.home_transform));
                if (currentHomeAttack.name == "Exploding chimney") main.Add(new FxExplodingChimney(main.home_transform));
                if (currentHomeAttack.name == "Freesby balcony") main.Add(new FxFreesbyBalcony(main.home_transform));
                if (currentHomeAttack.name == "Tree crush") main.Add(new FxTreeCrush(main.home_transform));

                soundGenerator.GenerateJapaneseSound();
            }
        }
    }
}

public class FightCottage : FightBase
{
    public FightCottage(Enemy enemy, Home home, SoundGenerator soundGenerator)
    {
        this.enemy = enemy;
        this.home = home;
        this.soundGenerator = soundGenerator;
    }
}

public class FightFactory : FightBase
{
    public FightFactory(Enemy enemy, Home home, SoundGenerator soundGenerator)
    {
        this.enemy = enemy;
        this.home = home;
        this.soundGenerator = soundGenerator;
    }
}

public class FightSkyscraper : FightBase
{
    public FightSkyscraper(Enemy enemy, Home home, SoundGenerator soundGenerator)
    {
        this.enemy = enemy;
        this.home = home;
        this.soundGenerator = soundGenerator;
    }
}

public class FightCathedral : FightBase
{
    public FightCathedral(Enemy enemy, Home home, SoundGenerator soundGenerator)
    {
        this.enemy = enemy;
        this.home = home;
        this.soundGenerator = soundGenerator;
    }
}

public class Attack
{
    public int durationMs;
    public int damage;
    public string name;

    public Attack(int durationMs, int damage, string name)
    {
        this.durationMs = durationMs;
        this.damage = damage;
        this.name = name;
    }
}

public abstract class Character
{
    public Attack[] attacks;
    public int life = 100;
}

public class Home : Character
{
    public Home()
    {
        this.attacks = new Attack[]{new Attack(500, 10, "Rollout"),
            new Attack(1000, 15, "Exploding chimney"),
            new Attack(600, 10, "Freesby balcony"),
            new Attack(2000, 30, "Tree crush") };
    }
}
