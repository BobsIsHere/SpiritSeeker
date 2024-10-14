using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject _staffTemplate = null;

    [SerializeField]
    private GameObject _staffSocket = null;

    private MagicStaff _staff;

    private void Awake()
    {
        if (_staff != null && _staffSocket != null)
        {
            GameObject staffObject = Instantiate(_staffTemplate, _staffSocket.transform, true);

            staffObject.transform.localPosition = Vector3.zero;
            staffObject.transform.localRotation = Quaternion.identity;

            _staff = staffObject.GetComponent<MagicStaff>();
        }
    }
}
