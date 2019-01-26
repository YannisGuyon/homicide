using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiDialog : MonoBehaviour
{
    public RectTransform dialog_transform;
    public Image dialog_image;
    public UiAttack attack_prefab;
    public Transform attacks_container;
    private List<UiAttack> attacks = new List<UiAttack>();
    public Color color_enabled;
    public Color color_disabled;
    private bool is_enabled = false;
    private bool show = true;
    public bool isHome;

    private void Start()
    {
        dialog_image.material = Instantiate(dialog_image.material);
    }
    private void Update()
    {
        dialog_transform.anchoredPosition = new Vector2(dialog_transform.anchoredPosition.x,
            Mathf.Lerp(dialog_transform.anchoredPosition.y, (show ? 20 : -300), Time.deltaTime * 20.0f));
        if (is_enabled && show)
            dialog_image.material.color = color_enabled;
        else
            dialog_image.material.color = color_disabled;

        if (is_enabled && show)
        {
            if (isHome)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                    SelectAttack(GetSelectedAttack() - 1);
                if (Input.GetKeyDown(KeyCode.DownArrow))
                    SelectAttack(GetSelectedAttack() + 1);
            }
        }
    }

    public void SetAttacks(params Attack[] attacks_texts)
    {
        for (int i = 0; i < attacks.Count; ++i)
            Destroy(attacks[i].gameObject);
        attacks.Clear();
        for (int i = 0; i < attacks_texts.Length; ++i)
        {
            UiAttack attack = Instantiate(attack_prefab, attacks_container);
            attacks.Add(attack);
            attack.SetText(attacks_texts[i].name);
            if (i == 0)
                attack.Select();
            else
                attack.Unselect();
        }
    }

    public void SelectAttack(int attack_index)
    {
        if (attack_index < 0) attack_index = 0;
        if (attack_index >= attacks.Count) attack_index = attacks.Count - 1;

        for (int i = 0; i < attacks.Count; ++i)
        {
            if (i == attack_index)
                attacks[i].Select();
            else
                attacks[i].Unselect();
        }
    }
    public int GetSelectedAttack()
    {
        for (int i = 0; i < attacks.Count; ++i)
            if (attacks[i].IsSelected())
                return i;
        return -1;
    }

    public void Hide()
    {
        show = false;
    }
    public void Show()
    {
        show = true;
    }

    public void Enable()
    {
        is_enabled = true;
        for (int i = 0; i < attacks.Count; ++i)
            attacks[i].SetAvailable(true);
    }
    public void Disable()
    {
        is_enabled = false;
        for (int i = 0; i < attacks.Count; ++i)
            attacks[i].SetAvailable(false);
    }
}
