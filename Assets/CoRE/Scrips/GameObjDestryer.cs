using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameObjDestryer : MonoBehaviour
{
    public float lifetime = 10f;
    void Start()
    {
        Destroy(this.gameObject, lifetime);
    }
}
