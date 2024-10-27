using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStaff : MonoBehaviour
{
    [SerializeField]
    private List<Spell> _spellPrefabs = new List<Spell>();

    private Spell _currentSpell = null;
    private int _currentSpellIndex = 0;

    private void Start()
    {
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
        Debug.Log("Current spell: " + _currentSpell.name);
    }

    public void CastSpell()
    {
        if (_currentSpell == null)
        {
            return;
        }

        // Instantiate & cast current spell
        Spell spellInstance = Instantiate(_currentSpell, transform.position, transform.rotation);
        spellInstance.transform.forward = transform.forward;
    }
}
