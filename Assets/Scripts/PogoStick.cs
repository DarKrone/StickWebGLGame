using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PogoStick : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Rigidbody2D _playerRb;
    [SerializeField] private Rigidbody2D _stickRb;
    [SerializeField] private CapsuleCollider2D _stickCollider;
    [SerializeField] private GameObject _rightPoint;
    [SerializeField] private float _maxGrow = 5f;
    [SerializeField] private float _minGrow = 1f;
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private float _shrinkGrowSpeed = 1f;
    [SerializeField] private float _autoShrineSpeed = 0.3f;
    [SerializeField] private bool _isGrowing = false;

    private float _size;
    private Vector3 _originalSize;

    [SerializeField]private bool _isAttached = false;

    // Start is called before the first frame update
    void Start()
    {
        _originalSize = transform.localScale;
        _size = _minGrow;
        _stickCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        //_playerRb.MovePosition(_stickRb.position);
        _player.transform.position = transform.position;
    }

    void FixedUpdate()
    {

        if (_isAttached)
        {
            //_playerRb.gravityScale = 0f;
        }
        else
        {
            //_playerRb.gravityScale = 1f;
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _stickRb.MoveRotation(_stickRb.rotation + _rotationSpeed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _stickRb.MoveRotation(_stickRb.rotation - _rotationSpeed);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _size += _shrinkGrowSpeed * Time.deltaTime;
            _isGrowing = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _size -= _shrinkGrowSpeed * Time.deltaTime;
        }
        //else
        //{
        //    _size -= _autoShrineSpeed * Time.deltaTime;
        //    _isGrowing = false;
        //}
        //if(_stickRb.)
        //foreach (Collider2D hit in hits)
        //{
        //    // Ignore our own collider.
        //    //if (hit.CompareTag("Player") || hit.CompareTag("Stick"))
        //    //{
        //    //    _isAttached = false;
        //    //    continue;
        //    //}
        //    _isAttached = true;
        //}



        //if (hits.Length == 0)
        //{
        //    _isAttached = false;
        //}
        //_isAttached = _stickCollider.attachedRigidbody;

        _size = Mathf.Clamp(_size, _minGrow, _maxGrow);
        if(transform.localScale.y != _size)
            transform.localScale = new Vector3(_originalSize.x, _size, _originalSize.z);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isAttached = true;
        //_playerRb.gravityScale = 0f;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log(collision.transform.name);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        _isAttached = false;
        //_playerRb.gravityScale = 1f;
    }
}
