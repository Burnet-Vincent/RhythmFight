using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

public class RhythmManager : MonoBehaviour
{
    public static RhythmManager Instance;

    public enum PerfectionLevel
    {
        Perfect,
        Good,
        Fail
    }
    
    [SerializeField] private int bpm = 120;

    [SerializeField] private RectTransform rhythmBarPanel;
    
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private float speedInSeconds = 1.5f;
    [SerializeField] private bool playMetronome = true;
    [SerializeField] private GameObject soundPrefab;
    [SerializeField] private float audioOffset;
    [SerializeField] private float timeForPerfects ;
    [SerializeField] private float timeForRights;

    private RectTransform _center;
    private RectTransform _spawnLeft, _spawnRight;
    private float _timeBpm;
    private float _timer;
    private float _spawnRate;

    private float _speed;
    private float _spawnTimeOffset;
    private bool _canSpawn = true;
    private bool _canPlaySound = true;
    private bool _hasHit;
    private bool _canCheckHit = true;
    
    private readonly List<GameObject> _leftBars = new List<GameObject>();
    private readonly List<GameObject> _rightBars = new List<GameObject>();

    private GameObject[] _bars;
    private AudioSource[] _audioSources;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
    }
    
    private void Start()
    {
        _timeBpm = 60f / bpm;
        _timer = 0;

        _center = rhythmBarPanel.GetChild(1).transform as RectTransform;
        _spawnLeft = rhythmBarPanel.GetChild(2).transform as RectTransform;
        _spawnRight = rhythmBarPanel.GetChild(3).transform as RectTransform;
        
        var distance = _center!.position.x - _spawnLeft!.position.x;
        
        _speed = distance / speedInSeconds;

        
        _bars = new GameObject[Mathf.CeilToInt(speedInSeconds / _timeBpm) * 2 + 4];
        for(var i = 0; i < _bars.Length; i++)
        {
            var bar = Instantiate(barPrefab, rhythmBarPanel);
            bar.SetActive(false);
            _bars[i] = bar;
        }
        
        var cam = Camera.main!;
        _audioSources = new AudioSource[Mathf.CeilToInt(speedInSeconds / _timeBpm) + 2];
        for(var i = 0; i < _audioSources.Length; i++)
        {
            var source = Instantiate(soundPrefab, cam.transform);
            _audioSources[i] = source.GetComponent<AudioSource>();
        }
        
        int count;
        for (count = 1; count * _timeBpm < speedInSeconds; count++){}
        _spawnTimeOffset = speedInSeconds - (count - 1) * _timeBpm;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        PlayMetronome();

        SpawnBars();

        CheckIfHit();
        
        if (_timer >= _timeBpm)
        {
            _timer = 0;
            _canSpawn = true;
            _canPlaySound = true;
            _canCheckHit = true;
        }

        ChangeBarsPosition(_leftBars, false);
        ChangeBarsPosition(_rightBars, true);
    }

    private void CheckIfHit()
    {
        if (_canCheckHit && _timer >= timeForRights / 2)
        {
            _canCheckHit = false;
            if (!_hasHit)
            {
                ScoreManager.Instance.ResetScore();
            }
            
            _hasHit = false;
        }
    }

    private void ChangeBarsPosition(List<GameObject> bars, bool isGoingLeft)
    {
        GameObject barToRemove = null;
        foreach (GameObject bar in bars)
        {
            var positionX = bar.transform.position.x;
            var newPosX = positionX + _speed * Time.deltaTime * (isGoingLeft ? -1 : 1);
            
            if (isGoingLeft)
            {
                if (newPosX <= _center.position.x)
                {
                    barToRemove = bar;
                }
            }
            else
            {
                if (newPosX >= _center.position.x)
                {
                    barToRemove = bar;
                }
            }
            
            var oldPos = bar.transform.position;
            bar.transform.position = new Vector3(newPosX, oldPos.y, oldPos.z);
        }

        if (!barToRemove) return;
        bars.Remove(barToRemove);
        barToRemove.SetActive(false);
    }

    private void PlayMetronome()
    {
        if (playMetronome && _canPlaySound &&_timer >= _timeBpm + audioOffset)
        {
            _audioSources.FirstOrDefault(t => !t.isPlaying)!.Play();
            _canPlaySound = false;
        }
    }

    private void SpawnBars()
    {
        if (!_canSpawn || !(_timer >= _timeBpm - _spawnTimeOffset)) return;

        InitializeBar(_leftBars, _spawnLeft.position);
        InitializeBar(_rightBars, _spawnRight.position);
            
        _canSpawn = false;
    }

    private void InitializeBar(List<GameObject> bars, Vector3 position)
    {
        var bar = _bars.FirstOrDefault(t => !t.activeInHierarchy)!;
        bar.SetActive(true);
        bar.transform.position = position;
        bars.Add(bar);
    }

    public PerfectionLevel GetPerfectionLevel()
    {

        if (_hasHit) return PerfectionLevel.Fail;
        
        _hasHit = true;
        
        if (_timer <= timeForPerfects / 2 || _timer >= _timeBpm - timeForPerfects / 2)
        {
            return PerfectionLevel.Perfect;
        }
        
        if (_timer <= timeForRights / 2 || _timer >= _timeBpm - timeForRights / 2)
        {
            return PerfectionLevel.Good;
        }
        
        return PerfectionLevel.Fail;
    }
}
