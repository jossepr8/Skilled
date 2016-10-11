﻿using UnityEngine;
using System.Collections;

public abstract class ActionBlock : MonoBehaviour {

    public abstract void Activate(GameObject activator);

    protected Bounds _bounds;

    void Start()
    {
        _bounds = GetComponent<SpriteRenderer>().bounds;
        Initialize();
    }

    protected virtual void Initialize()
    {

    }

    protected bool inBounds(Vector2 position)
    {
        if (_bounds.Contains(position)) return true;
        return false;
    }

    void Update()
    {
        
        BlockUpdate();
    }
    protected virtual void BlockUpdate()
    {
        //
    }
}
