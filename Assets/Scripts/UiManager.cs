using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public UiLifeBar life_bar_home;
    public UiLifeBar life_bar_enemy;
    public UiDialog dialog_home;
    public UiDialog dialog_enemy;

    public void Init()
    {
        Hide();
    }
    public void UpdateUi(Main main)
    {
        if (main.fight != null)
        {
            life_bar_home.Set(main.fight.home.life);
            life_bar_enemy.Set(main.fight.enemy.life);
        }
    }
    
    public void Hide()
    {
        life_bar_home.Hide();
        life_bar_enemy.Hide();
        dialog_home.Hide();
        dialog_enemy.Hide();
    }
    public void Show()
    {
        life_bar_home.Show();
        life_bar_enemy.Show();
        dialog_home.Show();
        dialog_enemy.Show();
    }
}
