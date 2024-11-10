using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    private UIDocument _attachedDocument = null;
    private VisualElement _root = null;
    private ProgressBar _healthbar = null;
    private ProgressBar _coolDownBar = null;

    private Health _playerHealth;
    private AttackBehaviour _attackBehaviour;

    // Start is called before the first frame update
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
            _healthbar = _root.Q<ProgressBar>("LivesBar");
            _coolDownBar = _root.Q<ProgressBar>("MagicCoolDownBar");

            // Initialze cooldown bar value & text
            if (_coolDownBar != null)
            {
                _coolDownBar.value = 0;
                _coolDownBar.title = "0.0 seconds";
            }

            PlayerCharacter player = FindObjectOfType<PlayerCharacter>();

            if (player != null)
            {
                // --- LIVES BAR --- //

                _playerHealth = player.GetComponent<Health>();

                if (_playerHealth)
                {
                    // initialize
                    UpdateHealth(_playerHealth.MaxLives, _playerHealth.CurrentLives);

                    // hook to monitor changes
                    _playerHealth.OnHealthChanged += UpdateHealth;
                }

                // --- COOLDOWN BAR --- //
                _attackBehaviour = player.GetComponent<AttackBehaviour>();

                if(_attackBehaviour)
                {
                    _attackBehaviour.OnSpellCast += StartCoolDownBar;
                }
            }
        }
    }

    public void UpdateHealth(float startHealth, float currentHealth)
    {
        if (_healthbar == null) return;
        _healthbar.value = (currentHealth / startHealth) * 100.0f;
        _healthbar.title = string.Format("{0}/{1}", currentHealth, startHealth);
    }

    public void StartCoolDownBar(float coolDownDuration)
    {
        StartCoroutine(UpdateCooldownBar(coolDownDuration));
    }

    private IEnumerator UpdateCooldownBar(float coolDownDuration)
    {
        while (true)
        {
            if (_coolDownBar == null || _attackBehaviour == null)
            {
                yield break;
            }

            if (_attackBehaviour.IsOnCooldown)
            {
                float elapsedTime = 0.0f;

                while (elapsedTime < coolDownDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float remainingTime = coolDownDuration - elapsedTime;

                    float progress = Mathf.Clamp01(elapsedTime / coolDownDuration);
                    _coolDownBar.value = progress * 100;
                    _coolDownBar.title = string.Format("{0:0.0} seconds", remainingTime);

                    yield return null;
                }
            }

            _coolDownBar.value = 0;
            _coolDownBar.title = "0.0 seconds";

            yield return null;
        }
    }
}
