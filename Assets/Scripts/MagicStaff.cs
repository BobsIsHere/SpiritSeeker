using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStaff : MonoBehaviour
{
    //[SerializeField]
    //public Spell _yellowSpell;

    [SerializeField]
    public Spell _blueSpell;

    [SerializeField]
    public Spell _redSpell;

    private float spellCooldown = 5.0f;

    private enum SpellType
    {
        //Yellow,
        Blue,
        Red
    }
    private SpellType currentSpell = SpellType.Blue;

    private void Update()
    {
        
    }

    public void SwitchSpell()
    {

    }

    public void CastSpell()
    {

    }
}
