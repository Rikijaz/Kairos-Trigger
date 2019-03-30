using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : RaycastController
{
    public struct CollisionInfo
    {
        public bool above;
        public bool below;
        public bool left;
        public bool right;

        public void Reset()
        {
            above = false;
            below = false;
            left = false;
            right = false;
        }
    }

    public CollisionInfo _collisionInfo;

    public override void Start()
    {
        base.Start();
    }

    public void FixedUpdate()
    {
        //Debug.Log("Above: " + _collisionInfo.above);
        //Debug.Log("below: " + _collisionInfo.below);
        //Debug.Log("left: " + _collisionInfo.left);
        //Debug.Log("right: " + _collisionInfo.right);
    }

    public void Move(Vector3 moveDistance)
    {
        UpdateRaycastOrigins();
        _collisionInfo.Reset();

        if (moveDistance.x != 0)
        {
            HorizontalCollisions(ref moveDistance);
        }
        if (moveDistance.y != 0)
        {
            VerticalCollisions(ref moveDistance);
        }

        transform.Translate(moveDistance);
    }

    void HorizontalCollisions(ref Vector3 moveDistance)
    {
        float directionX = Mathf.Sign(moveDistance.x);
        float rayLength = Mathf.Abs(moveDistance.x) + _skinWidth;

        for (var i = 0; i < _horizontalRayCount; i++)
        {
            Vector2 rayOrigin;
            if (directionX == -1)
            {
                rayOrigin = _raycastOrigins.bottomLeft;
            }
            else
            {
                rayOrigin = _raycastOrigins.bottomRight;
            }
            rayOrigin += Vector2.up * (_horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin, 
                Vector2.right * directionX, 
                rayLength, 
                _collisionMask);

            Debug.DrawRay(
                rayOrigin, 
                Vector2.right * directionX * rayLength, 
                Color.red);

            if (hit)
            {
                moveDistance.x = (hit.distance - _skinWidth) * directionX;
                rayLength = hit.distance;

                _collisionInfo.left = directionX == -1;
                _collisionInfo.right = directionX == 1;
            }
        }
    }

    void VerticalCollisions(ref Vector3 moveDistance)
    {
        float directionY = Mathf.Sign(moveDistance.y);
        float rayLength = Mathf.Abs(moveDistance.y) + _skinWidth;

        for (var i = 0; i < _verticalRayCount; i++)
        {
            Vector2 rayOrigin;
            if (directionY == -1)
            {
                rayOrigin = _raycastOrigins.bottomLeft;
            }
            else
            {
                rayOrigin = _raycastOrigins.topLeft;
            }
            rayOrigin += Vector2.right * (_verticalRaySpacing * i + moveDistance.x);
            RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin, 
                Vector2.up * directionY, 
                rayLength, 
                _collisionMask);

            Debug.DrawRay(
                rayOrigin, 
                Vector2.up * directionY * rayLength, 
                Color.red);

            if (hit)
            {
                moveDistance.y = (hit.distance - _skinWidth) * directionY;
                rayLength = hit.distance;

                _collisionInfo.below = directionY == -1;
                _collisionInfo.above = directionY == 1;
            }
        }
    }
}
