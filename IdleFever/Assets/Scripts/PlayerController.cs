using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5;
    [SerializeField]private PlayerStack _stack;

    private Vector3 _input;
    private Animator _animator;
    private bool _isCarrying;

    private readonly int defaultMovementBlendTreeHash = Animator.StringToHash("Default_Walk");
    private readonly int boxMovementBlendTreeHash = Animator.StringToHash("WBox_Walk");
    private readonly int movementHash = Animator.StringToHash("MovementSpeed");
    private const float AnimationDampTime = 0.1f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.CrossFadeInFixedTime(defaultMovementBlendTreeHash, AnimationDampTime);

    }
    private void Update()
    {
        GatherInput();
        Look();
        AnimationController();
        BlendTreeSetter();
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void BlendTreeSetter()
    {
        if (_stack.CarryingStack.Count > 0 && !_isCarrying)
        {
            _animator.CrossFadeInFixedTime(boxMovementBlendTreeHash, AnimationDampTime);
            _isCarrying = true;
        }
        else if (_stack.CarryingStack.Count == 0 && _isCarrying)
        {
            _animator.CrossFadeInFixedTime(defaultMovementBlendTreeHash, AnimationDampTime);
            _isCarrying = false;
        }
    }
    private void AnimationController()
    {
        if (_input.sqrMagnitude > 0)
        {
            _animator.SetFloat(movementHash, 1);
        }
        else
            _animator.SetFloat(movementHash, 0);
    }
    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }
    private void Look()
    {
        if (_input == Vector3.zero) return;

        var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        transform.rotation = rot;
    }
    private void Move()
    {
        _rb.MovePosition(transform.position + transform.forward * _input.normalized.magnitude * _speed * Time.deltaTime);
    }
}
