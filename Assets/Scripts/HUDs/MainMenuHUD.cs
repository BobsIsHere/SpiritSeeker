using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuHUD : MonoBehaviour
{
    private UIDocument _attachedDocument = null;
    private VisualElement _root = null;

    private Button _startButton = null;
    private Button _controlsButton = null;
    private Button _quitButton = null;

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
            _startButton = _root.Q<Button>("StartButton");
            _controlsButton = _root.Q<Button>("ControlsButton");
            _quitButton = _root.Q<Button>("QuitButton");

            _startButton.clicked += OnStartGameClicked;
            _controlsButton.clicked += OnControlsClicked;
            _quitButton.clicked += OnQuitGameClicked;
        }
    }

    private void OnStartGameClicked()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    private void OnControlsClicked()
    {
        SceneManager.LoadScene("ControlMenu");
    }

    private void OnQuitGameClicked()
    {
        Application.Quit();
    }
}
