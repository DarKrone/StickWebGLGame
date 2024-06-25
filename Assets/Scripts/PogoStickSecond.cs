using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PogoStickSecond : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _maxGrow = 5f;
    [SerializeField] private float _minGrow = 1f;
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private float _shrinkGrowSpeed = 1f;
    [SerializeField] private float _autoShrinkSpeed = 10f;

    [SerializeField] private GameObject _leftPoint;
    [SerializeField] private GameObject _rightPoint;

    public float _sizeLeft;
    public float _sizeRight;

    private Rigidbody2D _playerRB;
    public bool _isAttachedRight = false;
    public bool _isAttachedLeft = false;
    public bool _isGrowing = false;

    [SerializeField] private bool _isGroundedRight = false;
    [SerializeField] private bool _isGroundedLeft = false;

    void Start()
    {
        _playerRB = _player.GetComponent<Rigidbody2D>();
        _sizeRight = _minGrow;
        _sizeLeft = _minGrow;
    }
    private Vector3 UpperPoint()
    {

        return Vector3.zero;
    }
    // Update is called once per frame
    void Update()
    {   
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (_isGroundedRight && !_isGroundedLeft)
            {
                transform.RotateAround(_rightPoint.transform.position, Vector3.forward, _rotationSpeed);
            }
            else if (_isGroundedLeft && !_isGroundedRight)
            {
                transform.RotateAround(_leftPoint.transform.position, Vector3.forward, _rotationSpeed);
            }
            else if (!_isGroundedLeft && !_isGroundedRight)
            {
                transform.Rotate(0, 0, 1 * _rotationSpeed);
            }
            //else
            //{
            //    transform.RotateAround(_leftPoint.transform.position, Vector3.back, _rotationSpeed);
            //}

        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (_isGroundedRight && !_isGroundedLeft)
            {
                transform.RotateAround(_rightPoint.transform.position, Vector3.back, _rotationSpeed);
            }
            else if (_isGroundedLeft && !_isGroundedRight)
            {
                transform.RotateAround(_leftPoint.transform.position, Vector3.back, _rotationSpeed);
            }
            else if (!_isGroundedLeft && !_isGroundedRight)
            {
                transform.Rotate(0, 0, -1 * _rotationSpeed);
            }
            //else
            //{
            //    transform.RotateAround(_rightPoint.transform.position, Vector3.back, _rotationSpeed);
            //}
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (!_isAttachedRight)
            {
                _sizeRight += _shrinkGrowSpeed * Time.deltaTime;
                
                // Т.к. одна сторона прикреплена то она больше не растёт => другая сторона должна расти и за прикреплённую тоже, поэтому maxGrow + (maxGrow - sizeLeft)
                _sizeRight = Mathf.Round(Mathf.Clamp(_sizeRight, _minGrow, _maxGrow + (_maxGrow - _sizeLeft)) * 100) / 100;

            }
            if (!_isAttachedLeft)
            {
                _sizeLeft += _shrinkGrowSpeed * Time.deltaTime;
                _sizeLeft = Mathf.Round(Mathf.Clamp(_sizeLeft, _minGrow, _maxGrow + (_maxGrow - _sizeRight)) * 100) / 100;

            }
            _isGrowing = true;

        }
        else
        {
            if (_isGrowing)
            {
                // Нам нужно обоим точкам присвоить равную удалённость от центра (только один раз). 
                // Разная она из-за того что описано выше (одна точка растёт и за другую)
                float middlePoint = (_sizeLeft + _sizeRight) / 2;
                _sizeLeft = middlePoint;
                _sizeRight = middlePoint;
            }
            _sizeRight -= _autoShrinkSpeed * Time.deltaTime;
            _sizeLeft -= _autoShrinkSpeed * Time.deltaTime;
            _sizeRight = Mathf.Round(Mathf.Clamp(_sizeRight, _minGrow, _maxGrow) * 100) / 100;
            _sizeLeft = Mathf.Round(Mathf.Clamp(_sizeLeft, _minGrow, _maxGrow) * 100) / 100;
            _isGrowing = false;
        }

        //_sizeLeft = Mathf.Round(Mathf.Clamp(_sizeLeft, _minGrow, _maxGrow + (_maxGrow -_sizeRight)) * 100) / 100;
        
        _leftPoint.transform.localPosition = new Vector3(-_sizeLeft, 0, 0);
        _rightPoint.transform.localPosition = new Vector3(_sizeRight, 0, 0);


        //--------------------------------------------Правый-------------------------------------------
        RaycastHit2D hit = Physics2D.CircleCast(_rightPoint.transform.position, 0.1f,Vector2.zero,0,LayerMask.GetMask("Default"));

        _isAttachedRight = hit.transform != null &&
                           _isGrowing;

        _isGroundedRight = _isAttachedRight &&
                           _player.transform.position.y >= hit.point.y;




        //--------------------------------------------Левый-------------------------------------------

        hit = Physics2D.CircleCast(_leftPoint.transform.position, 0.1f, Vector2.zero, 0, LayerMask.GetMask("Default"));

        _isAttachedLeft = hit.transform != null &&
                           _isGrowing;

        _isGroundedLeft = _isAttachedLeft &&
                           _player.transform.position.y >= hit.point.y;





        //Вставить игрока между точками
        //if (_isAttachedLeft || _isAttachedRight)
        //    _player.transform.position = (_rightPoint.transform.position + _leftPoint.transform.position) / 2;

        if (!_isAttachedRight && !_isAttachedLeft)
        {
            transform.position = _player.transform.position;
            _playerRB.gravityScale = 1f;
        }
        else
        {
            _playerRB.gravityScale = 0f;
        }

        Debug.DrawLine(_player.transform.position, (_rightPoint.transform.position + _leftPoint.transform.position) / 2);
    }

    

}
