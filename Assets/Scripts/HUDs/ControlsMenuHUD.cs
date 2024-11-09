using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ControlsMenuHUD : MonoBehaviour
{
    private UIDocument _attachedDocument = null;
    private VisualElement _root = null;

    private Button _goBackButton = null;

    void Start()
    {
        //UI
        _attachedDocument = GetComponent<UIDocument>();

        if (_attachedDocument)
        {
            _root = _attachedDocument.rootVisualElement;
        }

        if (_root != null)
        {
            _goBackButton = _root.Q<Button>("GoBackButton");

            _goBackButton.clicked += OnGoBackToMenuClicked;
        }
    }

    void OnGoBackToMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
