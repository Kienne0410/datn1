using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveFollow : MonoBehaviour
{
    private PlayerControls _playerControls;

    private void Awake()
    {
        _playerControls = InputManager.Instance.playerControls;
    }
    private Vector2 _moveInput;

    private void Update()
    {
        MoveFollowWeapon();
    }
    private void MoveFollowWeapon()
    {
        _moveInput = _playerControls.Movement.Move.ReadValue<Vector2>();
        print(_moveInput);
        if (_moveInput.sqrMagnitude > 0.01f)
        {
            // Tính góc quay (theo độ) dựa trên vector _moveInput
            float angle = Mathf.Atan2(_moveInput.y, _moveInput.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            // Gán góc quay cho khẩu súng
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

}
