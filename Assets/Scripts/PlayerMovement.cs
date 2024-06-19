using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float _speed = 9;

    [SerializeField, Tooltip("Acceleration while _grounded.")]
    float _walkAcceleration = 75;

    [SerializeField, Tooltip("Acceleration while in the air.")]
    float _airAcceleration = 30;

    [SerializeField, Tooltip("Deceleration applied when character is _grounded and not attempting to move.")]
    float _groundDeceleration = 70;

    [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
    float _jumpHeight = 4;

    [SerializeField] PogoStickSecond _stickSecond;

    private CapsuleCollider2D _playerCollider;

    private Vector2 _velocity;

    /// <summary>
    /// Set to true when the character intersects a collider beneath
    /// them in the previous frame.
    /// </summary>
    private bool _grounded;

    private void Awake()
    {
        _playerCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        float moveInput = 0;
        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1;
        }

        if (_grounded)
        {
            _velocity.y = 0;

            if (Input.GetButtonDown("Jump"))
            {
                // Calculate the velocity required to achieve the target jump height.
                _velocity.y = Mathf.Sqrt(2 * _jumpHeight * Mathf.Abs(Physics2D.gravity.y));
            }
        }

        float acceleration = _grounded ? _walkAcceleration : _airAcceleration;
        float deceleration = _grounded ? _groundDeceleration : 0;

        if (moveInput != 0)
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, _speed * moveInput, acceleration * Time.deltaTime);
        }
        else
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, 0, deceleration * Time.deltaTime);
        }

        _velocity.y += Physics2D.gravity.y * Time.deltaTime;

        transform.Translate(_velocity * Time.deltaTime);

        _grounded = false;

        // Retrieve all colliders we have intersected after velocity has been applied.
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, _playerCollider.size, 0);

        foreach (Collider2D hit in hits)
        {
            // Ignore our own collider.
            if (hit == _playerCollider || hit.CompareTag("Stick"))
                continue;

            ColliderDistance2D colliderDistance = hit.Distance(_playerCollider);

            // Ensure that we are still overlapping this collider.
            // The overlap may no longer exist due to another intersected collider
            // pushing us out of this one.
            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                // If we intersect an object beneath us, set grounded to true. 
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && _velocity.y < 0)
                {
                    _grounded = true;
                }
            }
        }
    }
}
