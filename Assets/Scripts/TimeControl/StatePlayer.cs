using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayer : MonoBehaviour
{
    [SerializeField] TimeKeeper _timeKeeper;
    [SerializeField] GameObject _spriteTrailPrefab;

    private Dictionary<int, EntityState> _recording =
        new Dictionary<int, EntityState>();

    //private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private bool _isPlaying = false;
    private int _timePosition;

    void Start()
    {
        //_animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (_isPlaying && (Input.GetButton("RewindTime")))
        {
            if (_recording.ContainsKey(_timePosition)) {
                PlayState(_recording[_timePosition]);
                Debug.Log("_timePosition: " + _timePosition + "_recording size: " + _recording.Count);
                if (Input.GetButton("ForwardTime"))
                {
                    if (_timePosition < _recording.Count - 1)
                    {
                        _timePosition++;
                    }
                }
                else
                {
                    _timePosition--;
                }
                
            }
        }
        else
        {
            _recording.Clear();
            _timePosition = 0;
        }
    }

    public void SaveRecording(Dictionary<int, EntityState> recording)
    {
        _recording = new Dictionary<int, EntityState>(recording);
        _isPlaying = true;
        _timePosition = _recording.Count - 1;
        //Debug.Log("Rewinding! _recording size: " + 
        //    _recording.Count + 
        //    "_timeKeeper.time_: " 
        //    + _timeKeeper.time_);
    }

    private void UpdateSpriteTrail(bool direction)
    {
        GameObject spriteTrail = Instantiate(
            _spriteTrailPrefab, 
            transform.position, 
            Quaternion.identity);

        spriteTrail.GetComponent<SpriteRenderer>().sprite = 
            _spriteRenderer.sprite;
        spriteTrail.GetComponent<SpriteRenderer>().flipX = direction;
    }

    private void PlayState(EntityState entityState)
    {
        UpdateSpriteTrail(entityState.direction);

        transform.position = entityState.position;
        //_animator.Play(entityState.animationState);
        _spriteRenderer.flipX = entityState.direction;
    }
}
