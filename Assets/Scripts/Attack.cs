using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    public Text text;
    public GameObject arrow;

    private void Update()
    {
        arrow.GetComponent<RectTransform>().anchoredPosition = Random.insideUnitCircle;
    }
    public void SetText(string str)
    {
        text.text = str;
    }
    public void Select()
    {
        arrow.SetActive(true);
    }
    public void Unselect()
    {
        arrow.SetActive(false);
    }
    public bool IsSelected()
    {
        return arrow.activeSelf;
    }
}
