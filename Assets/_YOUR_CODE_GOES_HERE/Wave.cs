using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]
public class Wave : ScriptableObject 
{
    public Pattern pattern;
    public List<Enemy> enemies;
    public float vecticalSpacing;
    public float horizontalMaxShips;
}

public enum Pattern { Triangle, Line }
