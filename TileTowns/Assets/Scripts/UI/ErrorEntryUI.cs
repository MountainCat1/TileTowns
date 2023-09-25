using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorEntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textScript;

    public void SetError(string message)
    {
        textScript.text = message;
    }
}
