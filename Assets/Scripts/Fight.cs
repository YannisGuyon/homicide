using System.Collections;
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

public class FightHouse : Fight
{
    public int lastDamageDealtByEnemyMs;
    public int lastDamageDealtByHomeMs;
    public FightHouse(Enemy enemy, Home home)
    {
        this.enemy = enemy;
        this.home = home;
    }

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
            main.camera_manager.Trebble(0.02f);
            Attack currentEnemyAttack = this.enemy.GeCurrentAttack();
            main.ui_manager.dialog_enemy.SelectAttack(this.enemy.attackPattern[this.enemy.currentPatternIndex]);
            this.enemy.updatePattern();
            home.life -= currentEnemyAttack.damage;
            lastDamageDealtByEnemyMs = -currentEnemyAttack.durationMs;
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
            }
        }
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
