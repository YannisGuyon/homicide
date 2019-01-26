using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public LifeBar life_bar_home;
    public LifeBar life_bar_enemy;

    public void Init()
    {
        Hide();
    }
    public void UpdateUi(Main main)
    {
        if (main.fight != null)
        {
            life_bar_home.Set(main.fight.life_home);
            life_bar_enemy.Set(main.fight.enemy.life);
        }
    }
    
    public void Hide()
    {
        life_bar_home.Hide();
        life_bar_enemy.Hide();
    }
    public void Show()
    {
        life_bar_home.Show();
        life_bar_enemy.Show();
    }
}
