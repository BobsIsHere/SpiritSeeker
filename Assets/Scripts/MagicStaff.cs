using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicStaff : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _spellPrefabs = new List<GameObject>();

    private GameObject _currentSpell = null;
    private int _currentSpellIndex;

    private void Start()
    {
        _currentSpellIndex = 0;

        if (_spellPrefabs.Count > 0)
        {
            _currentSpell = _spellPrefabs[_currentSpellIndex];
        }
    }

    public void SwitchSpell()
    {
        if (_spellPrefabs.Count == 0)
        {
            return;
        }

        Debug.Log("Spell prefabs count: " + _spellPrefabs.Count);

        //Cycle to next spell
        _currentSpellIndex = (_currentSpellIndex + 1) % _spellPrefabs.Count;
        _currentSpell = _spellPrefabs[_currentSpellIndex];

        //Print out spell name
        Debug.Log("Current spell: " + _currentSpell);
    }

    public void CastSpell()
    {
        if (_currentSpell == null)
        {
            return;
        }

        // Get mouse psotion in world space
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        Vector3 worldMousePosition = Vector3.zero;

        if (Physics.Raycast(ray, out hit))
        {
            worldMousePosition = hit.point;

            Vector3 direction = (worldMousePosition - transform.position).normalized;
            direction.y = 0;

            // Instantiate & cast current spell
            GameObject spellInstance = Instantiate(_currentSpell, transform.position, transform.rotation);

            // Set direction of spell
            Spell spellComponent = spellInstance.GetComponent<Spell>();
            if (spellComponent != null)
            {
                spellComponent.SetDirection(direction);
            }
        }
    }
}
