using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverHUD : MonoBehaviour
{
    private UIDocument _attachedDocument = null;
    private VisualElement _root = null;

    private Button _restartButton = null;
    private Button _goBackButton = null;
    // Start is called before the first frame update
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
        Debug.Log("Attempting to restart. Last Scene: " + GameManager._lastScene);

        if (!string.IsNullOrEmpty(GameManager._lastScene))
        {
            GameManager.Instance.LoadScene(GameManager._lastScene); // Load the last scene
        }
        else
        {
            Debug.LogError("Last scene is empty! Cannot load.");
        }
    }

    private void OnGoBackClicked()
    {
        GameManager.Instance.LoadScene("MainMenu");
    }
}
