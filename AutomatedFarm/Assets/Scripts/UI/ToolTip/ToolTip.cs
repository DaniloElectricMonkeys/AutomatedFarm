using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ToolTip : MonoBehaviour
{
    public RectTransform rect;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public LayoutElement layout;
    public int wrapLimit;

    public void SetContent(string title, string description)
    {
        this.title.text = title;
        this.description.text = description;

        int titleLenght = this.title.text.Length;
        int descriptionLenght = this.description.text.Length;

        //enable and disable layou component based on text lenght
        layout.enabled = (titleLenght > wrapLimit || descriptionLenght > wrapLimit) ? true : false;
    }

    private void Update()
    {
        MoveAndAnchor();
    }

    private void MoveAndAnchor()
    {
        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rect.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }
}