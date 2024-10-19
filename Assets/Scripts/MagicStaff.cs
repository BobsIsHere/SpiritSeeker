using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStaff : MonoBehaviour
{
    //[SerializeField]
    //public GameObject _yellowSpell;

    [SerializeField]
    public GameObject _blueSpell;

    [SerializeField]
    public GameObject _redSpell;

    private float spellCooldown = 1.0f;

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
