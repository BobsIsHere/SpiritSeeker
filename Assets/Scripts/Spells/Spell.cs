using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField]
    private float spellSpeed = 10.0f;

    [SerializeField]
    private float spellDuration = 5.0f;

    protected virtual void Start()
    {
        Destroy(gameObject, spellDuration);
    }

    protected virtual void Update()
    {
        MoveSpell();
    }

    private void MoveSpell()
    {
        transform.Translate(Vector3.forward * spellSpeed * Time.deltaTime);
    }
}
