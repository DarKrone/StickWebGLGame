using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PogoStick : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Rigidbody2D _playerRb;
    [SerializeField] private GameObject _rightPoint;
    [SerializeField] private float _maxGrow = 5f;
    [SerializeField] private float _minGrow = 1f;
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private float _shrinkGrowSpeed = 1f;
    [SerializeField] private float _autoShrineSpeed = 0.3f;
    
    private float _size;
    private Vector3 _originalSize;

    private bool _isAttached = false;

    // Start is called before the first frame update
    void Start()
    {
        _originalSize = transform.localScale;
        _size = _minGrow;
    }

    private void Update()
    {
        transform.position = _player.transform.position;
    }

    void FixedUpdate()
    {

        if (!_isAttached)
        {
            _playerRb.gravityScale = 1f;
        }
        else
        {
            _playerRb.gravityScale = 0f;
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, 0, 1 * _rotationSpeed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, 0, -1 * _rotationSpeed);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _size += _shrinkGrowSpeed * Time.deltaTime;
        }
        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    _size -= _shrinkGrowSpeed * Time.deltaTime;
        //}
        else
        {
            _size -= _autoShrineSpeed * Time.deltaTime;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(_rightPoint.transform.position, 0.1f, LayerMask.GetMask("Default")) ;
        Debug.Log(hits.Length);
        Debug.DrawLine(transform.position, _rightPoint.transform.position);

        foreach (Collider2D hit in hits)
        {
            // Ignore our own collider.
            //if (hit.CompareTag("Player") || hit.CompareTag("Stick"))
            //{
            //    _isAttached = false;
            //    continue;
            //}

            _isAttached = true;
            if (true)
            {
                Vector2 movePlayer = _player.transform.position - _rightPoint.transform.position;

                _player.transform.Translate(new Vector3(movePlayer.x, movePlayer.y) * Time.deltaTime * _shrinkGrowSpeed);
            }

        }

        if (hits.Length == 0)
        {
            _isAttached = false;
        }
        

        _size = Mathf.Clamp(_size, _minGrow, _maxGrow);
        transform.localScale = new Vector3(_originalSize.x, _size, _originalSize.z);
    }
}
