﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePath : MonoBehaviour
{
    [SerializeField]
    private Transform _objectToRotate;

    public Transform _pivot;

    public string _pathId;
    public int _pathDifficulty = 0;
    public LevelTheme _pathTheme;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        _objectToRotate.RotateAround(_pivot.position, Vector3.left, (105f * Time.deltaTime)/speed);
    }
}
