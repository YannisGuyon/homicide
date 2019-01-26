using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLifeBar : MonoBehaviour
{
    public RectTransform life_transform;
    public RectTransform bar_transform;
    public Image bar_image;
    public Color color_empty;
    public Color color_full;
    public bool flipped = false;
    private bool show = true;

    private void Update()
    {
        life_transform.anchoredPosition = new Vector2(life_transform.anchoredPosition.x,
            Mathf.Lerp(life_transform.anchoredPosition.y, (show ? -20 : 100), Time.deltaTime * 20.0f));
    }
    public void Set(int life)
    {
        float ratio = Mathf.Clamp01(life / 100.0f);
        if (flipped)
            bar_transform.anchorMin = new Vector2(1 - ratio, bar_transform.anchorMin.y);
        else
            bar_transform.anchorMax = new Vector2(ratio, bar_transform.anchorMax.y);
        bar_image.color = Color.Lerp(color_empty, color_full, ratio);
    }

    public void Hide()
    {
        show = false;
    }
    public void Show()
    {
        show = true;
    }
}
