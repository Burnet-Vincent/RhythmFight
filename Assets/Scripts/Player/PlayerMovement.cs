using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int stepSize = 1;
    
    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Move(context.ReadValue<Vector2>());
            // var state = RhythmManager.Instance.GetPerfectionLevel();
            // switch (state)
            // {
            //     case RhythmManager.PerfectionLevel.Perfect:
            //         Move(context.ReadValue<Vector2>());
            //         ScoreManager.Instance.AddScore(2);
            //         break;
            //     case RhythmManager.PerfectionLevel.Good:
            //         Move(context.ReadValue<Vector2>());
            //         ScoreManager.Instance.AddScore(1);
            //         break;
            //     case RhythmManager.PerfectionLevel.Fail:
            //         ScoreManager.Instance.ResetScore();
            //         break;
            // }
        }
    }

    private void Move(Vector2 movement)
    {
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            transform.position += new Vector3(Mathf.Sign(movement.x) * stepSize, 0, 0);
        }
        else
        {
            transform.position += new Vector3(0, Mathf.Sign(movement.y) * stepSize, 0);
        }
    }
}
