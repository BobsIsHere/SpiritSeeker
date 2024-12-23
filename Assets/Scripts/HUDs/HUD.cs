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
    private ProgressBar _glowStickBar = null;

    private Health _playerHealth;
    private AttackBehaviour _attackBehaviour;

    private Coroutine _coolDownCoroutine = null;

    private const float MAX_PERCENTAGE = 100.0f;

    private void Start()
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
            _glowStickBar = _root.Q<ProgressBar>("GlowStickBar");

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

                // --- GLOWSTICK BAR --- //
                player._onGlowStickCountChanged.AddListener(UpdateGlowStickBar);
                UpdateGlowStickBar(player.GetCurrentAmountOfGlowSticks());
            }
        }
    }

    private void UpdateHealth(float startHealth, float currentHealth)
    {
        if (_healthbar != null)
        {
            _healthbar.value = (currentHealth / startHealth) * MAX_PERCENTAGE;
            _healthbar.title = string.Format("{0}/{1}", currentHealth, startHealth);
        }
    }

    private void StartCoolDownBar(float coolDownDuration)
    {
        if (_coolDownCoroutine != null)
        {
            StopCoroutine(_coolDownCoroutine);
        }

        _coolDownCoroutine = StartCoroutine(UpdateCooldownBar(coolDownDuration));
    }

    private IEnumerator UpdateCooldownBar(float coolDownDuration)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < coolDownDuration)
        {
            elapsedTime += Time.deltaTime;
            float remainingTime = coolDownDuration - elapsedTime;

            float progress = Mathf.Clamp01(elapsedTime / coolDownDuration);
            _coolDownBar.value = progress * MAX_PERCENTAGE;
            _coolDownBar.title = string.Format("{0:0.0} seconds", remainingTime);

            yield return null;
        }

        _coolDownBar.value = 0;
        _coolDownBar.title = "0.0 seconds";
    }

    private void UpdateGlowStickBar(int amountOfSticks)
    {
        if (_glowStickBar != null)
        {
            _glowStickBar.value = amountOfSticks * MAX_PERCENTAGE;
            _glowStickBar.title = string.Format("{0} Glowsticks", amountOfSticks);
        }
    }
}
