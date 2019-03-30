using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] Player _player;
    [SerializeField] float _verticalOffset;
    [SerializeField] float _lookAheadDstX;
    [SerializeField] float _lookSmoothTimeX;
    [SerializeField] float _verticalSmoothTime;
    [SerializeField] Vector2 _focusAreaSize;

    private FocusArea focusArea;

    private float _currentLookAheadX;
    private float _targetLookAheadX;
    private float _lookAheadDirX;
    private float _smoothLookVelocityX;
    private float _smoothVelocityY;

    private bool _lookAheadStopped;

    void Start() { 
        focusArea = new FocusArea(_playerController._collider.bounds, _focusAreaSize);
    }

    void LateUpdate()
    {
        focusArea.Update(_playerController._collider.bounds);

        Vector2 focusPosition = focusArea.centre + Vector2.up * _verticalOffset;

        if (focusArea.velocity.x != 0)
        {
            _lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
            if ((Mathf.Sign(_player._axisInput.x) == 
                Mathf.Sign(focusArea.velocity.x)) && (_player._axisInput.x != 0))
            {
                _lookAheadStopped = false;
                _targetLookAheadX = _lookAheadDirX * _lookAheadDstX;
            }
            else
            {
                if (!_lookAheadStopped)
                {
                    _lookAheadStopped = true;
                    _targetLookAheadX = _currentLookAheadX + (
                        _lookAheadDirX * 
                        _lookAheadDstX - 
                        _currentLookAheadX) / 4f;
                }
            }
        }

        _currentLookAheadX = Mathf.SmoothDamp(
            _currentLookAheadX, 
            _targetLookAheadX, 
            ref _smoothLookVelocityX, 
            _lookSmoothTimeX);

        focusPosition.y = Mathf.SmoothDamp(
            transform.position.y, 
            focusPosition.y, 
            ref _smoothVelocityY, 
            _verticalSmoothTime);
        focusPosition += Vector2.right * _currentLookAheadX;
        transform.position = (Vector3)focusPosition + Vector3.forward * -10;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(focusArea.centre, _focusAreaSize);
    }

    struct FocusArea
    {
        public Vector2 centre;
        public Vector2 velocity;
        float left, right;
        float top, bottom;


        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}
