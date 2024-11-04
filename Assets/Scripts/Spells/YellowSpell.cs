using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowSpell : Spell
{
    private GameObject _lightObject;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        // Move with wand for the yellow spell
    }
}
