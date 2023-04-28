using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Enemy
{
    public override void OnNewSpawn()
    {
        base.OnNewSpawn();
        transform.Rotate(Random.Range(0f, 360f) * Vector3.forward);
    }
}
