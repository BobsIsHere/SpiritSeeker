using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameWonHUD : MonoBehaviour
{
    private UIDocument _attachedDocument = null;
    private VisualElement _root = null;

    private Button _restartButton = null;
    private Button _goBackButton = null;
    
    void Start()
    {
        _attachedDocument = GetComponent<UIDocument>();

        if (_attachedDocument)
        {
            _root = _attachedDocument.rootVisualElement;
        }

        if (_root != null)
        {
            _restartButton = _root.Q<Button>("RestartButton");
            _goBackButton = _root.Q<Button>("GoBackButton");

            _restartButton.clicked += OnRestartClicked;
            _goBackButton.clicked += OnGoBackClicked;
        }
    }

    private void OnRestartClicked()
    {
        GameManager.Instance.LoadScene("SpiritSeeker");
    }

    private void OnGoBackClicked()
    {
        GameManager.Instance.LoadScene("MainMenu");
    }
}
