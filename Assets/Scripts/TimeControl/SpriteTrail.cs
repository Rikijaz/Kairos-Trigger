using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTrail : MonoBehaviour
{
    //private const float destroyTime = 0.65f;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Color spriteColor = _spriteRenderer.color;
        spriteColor.a = 0.65f;
        _spriteRenderer.color = spriteColor;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    Color spriteColor = _spriteRenderer.color;
    //    spriteColor.a -= 0.1f;
    //    _spriteRenderer.color = spriteColor;

    //    //Destroy(gameObject, destroyTime);
    //}

    void FixedUpdate()
    {
        Color spriteColor = _spriteRenderer.color;
        spriteColor.a -= 0.05f;

        if (spriteColor.a > 0)
        {
            _spriteRenderer.color = spriteColor;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
