using System;
using TMPro;
using UnityEngine;

public class VersionView : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _versionText;

    private void Start() {
        _versionText.text = $"{Application.version}V";
    }
}
