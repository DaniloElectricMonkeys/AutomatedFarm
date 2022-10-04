using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolTip : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    private void Update() {
            transform.position = Input.mousePosition + new Vector3(1,1,0);
    }

}