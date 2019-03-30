using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeKeeper : MonoBehaviour
{
    [SerializeField] public int time_;
    [SerializeField] Player _player;

    private bool _isTimeForward = true;
    private bool _isTimeReverse = false;

    void Update()
    {
    }

    void FixedUpdate()
    {
        if (_isTimeForward)
        {
            time_++;
        }
    }
}
