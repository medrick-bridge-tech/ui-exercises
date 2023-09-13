using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomHelper {
    public static Vector3 RandomVector3(float max) => new Vector3(Random.Range(0, max), Random.Range(0, max), Random.Range(0, max));
}