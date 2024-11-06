using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowSpell : Spell
{
    private MagicStaff _magicStaff;

    protected override void Awake()
    {
        base.Awake();

        _magicStaff = FindObjectOfType<MagicStaff>();
    }

    protected override void FixedUpdate()
    {
        // Move with wand
        if (_magicStaff != null)
        {
            Vector3 wandPosition = _magicStaff.transform.position;
            Vector3 wandUp = _magicStaff.transform.up;

            transform.position = wandPosition + wandUp;
        }
    }
}
