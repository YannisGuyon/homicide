using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiAttack : MonoBehaviour
{
    public Text text;
    public GameObject container;
    public GameObject arrow;
    public GameObject dots;

    private void Start()
    {
        SetAvailable(true);
    }
    private void Update()
    {
        arrow.GetComponent<RectTransform>().anchoredPosition = Random.insideUnitCircle;
        dots.transform.Rotate(Vector3.forward, Time.deltaTime * 360.0f * 2);
    }
    public void SetText(string str)
    {
        text.text = str;
    }
    public void Select()
    {
        container.SetActive(true);
    }
    public void Unselect()
    {
        container.SetActive(false);
    }
    public bool IsSelected()
    {
        return container.activeSelf;
    }
    public void SetAvailable(bool available)
    {
        arrow.SetActive(available);
        dots.SetActive(!available);
    }
}
