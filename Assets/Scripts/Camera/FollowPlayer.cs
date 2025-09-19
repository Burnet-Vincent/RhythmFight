using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform _cam;
    private Transform _player;
    void Start()
    {
        _cam = Camera.main!.transform;
        _player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        var playerPos = _player.position;
        _cam.transform.position = new Vector3(playerPos.x, playerPos.y, _cam.position.z);
    }
}
