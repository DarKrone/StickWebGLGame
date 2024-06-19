using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PogoStickSecond : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _maxGrow = 5f;
    [SerializeField] private float _minGrow = 1f;
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private float _shrinkGrowSpeed = 1f;
    [SerializeField] private float _autoShrineSpeed = 0.3f;

    [SerializeField] private GameObject _leftPoint;
    [SerializeField] private GameObject _rightPoint;

    private float _sizeLeft;
    private float _sizeRight;
    private Rigidbody2D _playerRB;
    private bool _isAttachedRight = false;
    private bool _isAttachedLeft = false;
    private bool _isGrowing = false;

    void Start()
    {
        _playerRB = _player.GetComponent<Rigidbody2D>();
        _sizeRight = _minGrow;
        _sizeLeft = _minGrow;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isAttachedRight && !_isAttachedLeft)
        {
            transform.position = _player.transform.position;
            _playerRB.gravityScale = 1f;
        }
        else
        {
            _playerRB.gravityScale = 0f;
        }

        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //_leftPoint.transform.RotateAround(transform.position, new Vector3(0, 0, 1), _rotationSpeed);
            //_rightPoint.transform.RotateAround(transform.position, new Vector3(0, 0, 1), _rotationSpeed);
            if (_isAttachedRight && !_isAttachedLeft)
            {
                transform.RotateAround(_rightPoint.transform.position, Vector3.forward, _rotationSpeed);
            }
            else if (_isAttachedLeft && !_isAttachedRight)
            {
                transform.RotateAround(_leftPoint.transform.position, Vector3.forward, _rotationSpeed);
            }
            else
            {
                transform.Rotate(0, 0, 1 * _rotationSpeed);
            }

        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //_leftPoint.transform.RotateAround(transform.position, new Vector3(0, 0, -1), _rotationSpeed);
            //_rightPoint.transform.RotateAround(transform.position, new Vector3(0, 0, -1), _rotationSpeed);
            if (_isAttachedRight && !_isAttachedLeft)
            {
                transform.RotateAround(_rightPoint.transform.position, Vector3.back, _rotationSpeed);
            }
            else if (_isAttachedLeft && !_isAttachedRight)
            {
                transform.RotateAround(_leftPoint.transform.position, Vector3.back, _rotationSpeed);
            }

            else
            {
                transform.Rotate(0, 0, -1 * _rotationSpeed);
                
            }
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if(!_isAttachedRight) 
                _sizeRight += _shrinkGrowSpeed * Time.deltaTime;
            if (!_isAttachedLeft)
                _sizeLeft += _shrinkGrowSpeed * Time.deltaTime;
            _isGrowing = true;

        }
        else
        {
            _sizeRight -= _autoShrineSpeed * Time.deltaTime;
            _sizeLeft -= _autoShrineSpeed * Time.deltaTime; ;
            _isGrowing = false;
        }

        _sizeLeft = Mathf.Clamp(_sizeLeft, _minGrow, _maxGrow);
        _sizeRight = Mathf.Clamp(_sizeRight, _minGrow, _maxGrow);

        _leftPoint.transform.localPosition = new Vector3(-_sizeLeft, 0, 0);
        _rightPoint.transform.localPosition = new Vector3(_sizeRight, 0, 0);


        //--------------------------------------------Правый-------------------------------------------
        Collider2D[] hits = Physics2D.OverlapCircleAll(_rightPoint.transform.position, 0.1f, LayerMask.GetMask("Default"));

        if (hits.Length > 0)
        {
            if (_isGrowing)
            {
                _isAttachedRight = true;
            }
            else
            {
                _isAttachedRight = false;
            }
        }
        
        else
        {
            _isAttachedRight = false;
        }




        //--------------------------------------------Левый-------------------------------------------

        hits = Physics2D.OverlapCircleAll(_leftPoint.transform.position, 0.1f, LayerMask.GetMask("Default"));

        if (hits.Length > 0)
        {
            if (_isGrowing)
            {
                _isAttachedLeft = true;
            }
            else
            {
                _isAttachedLeft = false;
            }
        }
        else
        {
            _isAttachedLeft = false;
        }




        //Вставить игрока между точками
        if (_isAttachedLeft || _isAttachedRight)
            _player.transform.position = (_rightPoint.transform.position + _leftPoint.transform.position) / 2;

        Debug.DrawLine(_player.transform.position, (_rightPoint.transform.position + _leftPoint.transform.position) / 2);
    }

    

}
