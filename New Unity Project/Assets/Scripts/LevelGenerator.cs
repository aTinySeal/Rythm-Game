﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    List<Transform> _pathsEmpty;
    [SerializeField]
    List<Transform> _pathsLeft;
    [SerializeField]
    List<Transform> _pathsRight;
    [SerializeField]
    List<Transform> _pathsJump;
    [SerializeField]
    List<Transform> _pathsSlide;

    public bool isGenerating = true;

    [SerializeField]
    List<Transform> _activePaths;

    [SerializeField]
    Vector3 _pathSpawnPoint;


    [SerializeField]
    Transform _pathData;
    [SerializeField]
    Transform _nextPath;
    [SerializeField]
    Transform _recycledPath;

    public Level _level;
    public float _levelSpeed;

    [SerializeField]
    BPMProcessor _bpmProcessor;
    [SerializeField]
    PathData pathsData;

    [SerializeField]
    List<Level> _loadedLevels = new List<Level>();

    [SerializeField]
    SpawnNotes _noteSpawner;

    [SerializeField]
    Settings _settings;

    public float _globalPropScaler = 1f;
    // Start is called before the first frame update
    void Start()
    {
        GenerateCircle();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPathForGarabge();
        
    }

    //Takes song and difficulty and all the appropriate objects for the level.
    private Level CreateLevel(AudioClip songClip, int difficulty, LevelTheme theme = LevelTheme.park)
    {
        Level level;

        level = new Level(songClip.name, songClip, difficulty, LevelTheme.debug, null);
        return level;
    }

    private void GenerateCircle()
    {
        CreatePath(_pathsEmpty[Random.Range(0, _pathsEmpty.Count - 1)]);
    }

    private void CheckPathForGarabge()
    {
        if (isGenerating)
        {
            if (_activePaths[0].eulerAngles.x <= 320f && _activePaths[0].eulerAngles.x >= 91f)
            {
                RecyclePath();
            }
            if (_activePaths[_activePaths.Count - 1].eulerAngles.x <= 75f)
            {
                CreatePath(_nextPath);
            }
        }
    }

    //if the next path is the same as this path, recycle this to save performance
    private void RecyclePath(int index = 0, bool doRecycle = true)
    {
        if (doRecycle)
        {
            if (_recycledPath == null && _nextPath.GetComponent<RotatePath>()._pathId == _activePaths[index].GetComponent<RotatePath>()._pathId)
            {
                _recycledPath = _activePaths[index];
                _activePaths.RemoveAt(index);
            }else if (_recycledPath != null || _nextPath.GetComponent<RotatePath>()._pathId != _activePaths[index].GetComponent<RotatePath>()._pathId)
            {
                Destroy(_activePaths[index].gameObject);
                _activePaths.RemoveAt(index);
            }
        }else
        {
            Destroy(_activePaths[index].gameObject);
            _activePaths.RemoveAt(index);
        }
        
    }

    //Spawn a new path or use the recycled one
    private Transform CreatePath(Transform path, float rotation = 90)
    {
        bool useRecycling = false;
        Transform newPath = null;
        if (_recycledPath != null)
        {
            if (_recycledPath.GetComponent<RotatePath>()._pathId == path.GetComponent<RotatePath>()._pathId)
            {
                useRecycling = true;
            }
        }

        if (useRecycling)
        {
            newPath = _recycledPath;
            newPath.position = this.transform.position + _pathSpawnPoint;
            newPath.eulerAngles = new Vector3(90f, 0f ,0f);
            _activePaths.Add(newPath);
            _recycledPath = null;
            Debug.Log("Recycled");
        }
        else
        {
            newPath = Instantiate(path, this.transform.position + _pathSpawnPoint, Quaternion.Euler(rotation, 0, 0), this.transform);
            newPath.GetComponent<RotatePath>()._pivot = this.transform;
            newPath.GetComponent<RotatePath>().speed = _levelSpeed;
            _activePaths.Add(newPath);
        }
        
        _nextPath = null;
        SetNextPath(_pathsEmpty[0], false);
        return newPath;
    }

    private void SetNextPath(Transform nextPath, bool overwrite = false)
    {
        if (_nextPath == null || overwrite)
        {
            _nextPath = nextPath;
        }
    }

    //called 1.35 seconds before the note should be hit
    public void noteSpawned(int Lane) {
        switch (Lane)
        {
            case 0:
                {
                    SetNextPath(_pathsSlide[Random.Range(0, _pathsSlide.Count)], true);
                    break;
                }
            case 1:
                {
                    SetNextPath(_pathsLeft[Random.Range(0, _pathsLeft.Count)], true);
                    break;
                }
            case 2:
                {
                    SetNextPath(_pathsEmpty[Random.Range(0, _pathsEmpty.Count)], true);
                    break;
                }
            case 3:
                {
                    SetNextPath(_pathsRight[Random.Range(0, _pathsRight.Count)], true);
                    break;
                }
            case 4:
                {
                    SetNextPath(_pathsJump[Random.Range(0, _pathsJump.Count)], true);
                    break;
                }
            default:
                {
                    SetNextPath(_pathsEmpty[Random.Range(0, _pathsEmpty.Count)], true);
                    break;
                }
        }
        
    }

   



}
