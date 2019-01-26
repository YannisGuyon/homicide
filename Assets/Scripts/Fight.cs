using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fight
{
    public Main main;
    public int life_home;
    public Enemy enemy;

    public bool IsDone()
    {
        return (life_home <= 0 || enemy.life <= 0);
    }
    virtual public void Init()
    {
        life_home = 100;
        enemy.life = 100;

        main.ui_manager.dialog_home.SetAttacks("Home attack 1", "Home attack 2", "Home attack 3");     // #############
        main.ui_manager.dialog_enemy.SetAttacks("Enemy attack 1", "Enemy attack 2", "Enemy attack 3");    // #############
    }
    virtual public void Update() { }
}

public class FightHouse : Fight
{
    public int lastDamageDealtByEnemyMs;
    public FightHouse(Enemy enemy)
    {
        this.enemy = enemy;
    }

    override public void Init()
    {
        base.Init();
        lastDamageDealtByEnemyMs = 0;
    }
    override public void Update()
    {
        base.Update();
        lastDamageDealtByEnemyMs += (int)(Time.deltaTime * 1000);
        if (lastDamageDealtByEnemyMs >= 5)
        {
            // Fx damage taken
            main.camera_manager.Trebble(0.05f);
            Attack currentEnemyAttack = this.enemy.GeCurrentAttack();
            life_home -= currentEnemyAttack.damage;
            lastDamageDealtByEnemyMs -= currentEnemyAttack.durationMs;

            main.ui_manager.dialog_enemy.SelectAttack(0);                         // #############
            main.ui_manager.dialog_enemy.Enable();                         // #############
            main.ui_manager.dialog_enemy.Disable();                         // #############
            main.ui_manager.dialog_home.Enable();                         // #############
            main.ui_manager.dialog_home.Disable();                         // #############

            Debug.Log(currentEnemyAttack.name + " : life_home "+life_home);
        }
        if (Input.GetKey(KeyCode.KeypadEnter))
        {
            // Handle input
            int attack_index = main.ui_manager.dialog_home.GetSelectedAttack();     // #############
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
