using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EntityState
{
    public Vector3 position;
    public int animationState;
    public bool direction;

    public EntityState(Vector3 curr_position, 
        int curr_animationState,
        bool curr_direction) : this()
    {
        position = curr_position;
        animationState = curr_animationState;
        direction = curr_direction;
    }
}

public class StateRecorder : MonoBehaviour
{
    [SerializeField] TimeKeeper _timeKeeper;
    [SerializeField] StatePlayer _statePlayer;
    [SerializeField] Player _player;

    private Dictionary<int, EntityState> _stateHistory = 
        new Dictionary<int, EntityState>();
    private bool _isRecording = true;
    //private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        //_animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetButtonDown("RewindTime"))
        {
            //_isRecording = false;
            _statePlayer.SaveRecording(_stateHistory);
            //_timeKeeper.time_ = 0;
        }
    }

    void FixedUpdate()
    {
        if (_isRecording)
        {
            _stateHistory.Add(_timeKeeper.time_, new EntityState(
                transform.position,
                0,
                //_animator.GetCurrentAnimatorStateInfo(0).shortNameHash,
                _spriteRenderer.flipX));
                //transform.localScale.x > 0));
        }

        //Debug.Log(_stateHistory.Count);
    }
}
