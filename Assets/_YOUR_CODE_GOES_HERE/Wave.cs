using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]
public class Wave : ScriptableObject 
{
    public Pattern pattern;
    [Header("Enemies in wave")]
    public List<GameObject> enemies;
    public float vecticalSpacing =0;

    [Header("Only for custom pattern")]
    public List<Vector3> enemyLocations;
}

public enum Pattern { Triangle, Line, Custom }
