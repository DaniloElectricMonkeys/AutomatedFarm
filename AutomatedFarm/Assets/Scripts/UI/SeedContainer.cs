using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SeedContainer : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI growTime;

    public void Settings(Sprite sprite, float time)
    {
        icon.sprite = sprite;
        growTime.text = TimeSpan.FromSeconds(time).ToString(@"m\:ss");
    }
}
