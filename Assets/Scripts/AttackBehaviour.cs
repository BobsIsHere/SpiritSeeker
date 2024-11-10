using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject _staffTemplate = null;

    [SerializeField]
    private GameObject _staffSocket = null;

    [SerializeField]
    private float _attackCooldown = 5.0f;

    private MagicStaff _magicStaff;
    private bool _isOnCooldown = false;

    public bool IsOnCooldown
    {
        get { return _isOnCooldown; }
    }

    public float AttackCooldown
    {
        get { return _attackCooldown; }
    }

    public delegate void SpellCastDelegate(float coolDownDuration);
    public event SpellCastDelegate OnSpellCast;

    private void Awake()
    {
        if (_staffTemplate != null && _staffSocket != null)
        {
            GameObject staffObject = Instantiate(_staffTemplate, _staffSocket.transform, true);

            staffObject.transform.localPosition = Vector3.zero;
            staffObject.transform.localRotation = Quaternion.identity;

            _magicStaff = staffObject.GetComponent<MagicStaff>();
        }
    }

    public void Attack()
    {
        if (_isOnCooldown || _magicStaff == null)
        {
            return;
        }

        _magicStaff.CastSpell();

        OnSpellCast?.Invoke(_attackCooldown);

        StartCoroutine(SpellCoolDown());
    }

    private IEnumerator SpellCoolDown()
    {
        _isOnCooldown = true;
        yield return new WaitForSeconds(_attackCooldown);
        _isOnCooldown = false;
    }

    public MagicStaff GetMagicStaff()
    {
        return _magicStaff;
    }
}
