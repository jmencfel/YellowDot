using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class WavesManager : MonoBehaviour
{
    public delegate void WaveSpawned(int spawnedWaveNo);
    public event WaveSpawned E_WaveSpawned;

    [SerializeField] private List<Wave> _EnemyWaves;
    private int _ShipsLeftInCurrentWave;
    private Vector3 _firstShipPosition = new Vector3(-8.0f, 4.0f, 0);
    
    public int CurrentWave { get; private set; }

    public void SetupWavesAndSpawn()
    {
        //TODO you code goes her
        CurrentWave = 0;
        SpawnNextWave();
    }
    public void OnShipDestroyed()
    {
        _ShipsLeftInCurrentWave--;
        if (_ShipsLeftInCurrentWave<=0)
            SpawnNextWave();
    }
 
    public void SpawnNextWave()
    {
        if (CurrentWave < _EnemyWaves.Count)
        {
            Pattern pattern = _EnemyWaves[CurrentWave].pattern;
            if (pattern != Pattern.Custom)
                SpawnRegularPattern(_EnemyWaves[CurrentWave], pattern);
            else
                SpawnCustomPattern(_EnemyWaves[CurrentWave]);

            CurrentWave++;
            E_WaveSpawned?.Invoke(CurrentWave);
        }
    }
    private void SpawnRegularPattern(Wave currentWave, Pattern pattern)
    {
        var _shipCount = _EnemyWaves[CurrentWave].enemies.Count;
        _ShipsLeftInCurrentWave = _shipCount;
        float _horizontalSpaceBetweenShips = Mathf.Clamp( 15.0f / _shipCount, 2.0f, 10.0f);
        float _vecticalSpaceBetweenShips = _EnemyWaves[CurrentWave].vecticalSpacing;
        var _currentShipPosition = _firstShipPosition;
        int currentLineLimit = 8;
        foreach (GameObject obj in _EnemyWaves[CurrentWave].enemies)
        {
            var enemy = obj.GetComponent<Enemy>();
            var Ship = Instantiate(enemy, _currentShipPosition, Quaternion.identity);
            Ship.E_ShipDestroyed += OnShipDestroyed;
            _currentShipPosition += new Vector3(_horizontalSpaceBetweenShips, 0, 0);
            if (_currentShipPosition.x > currentLineLimit)
            {
                if (pattern != Pattern.Triangle)
                    _currentShipPosition.x = -currentLineLimit;
                else
                    _currentShipPosition.x = -(--currentLineLimit);
                _currentShipPosition.y -= _vecticalSpaceBetweenShips;
            }
        }
    }
    private void SpawnCustomPattern(Wave currentWave)
    {
        var _shipCount = _EnemyWaves[CurrentWave].enemies.Count;
        _ShipsLeftInCurrentWave = _shipCount;
        for (int i=0; i< _shipCount; i++)
        {
            var Ship = Instantiate(currentWave.enemies[i], currentWave.enemyLocations[i], Quaternion.identity).GetComponent<Enemy>();
            Ship.E_ShipDestroyed += OnShipDestroyed;
        }
    }
    #region tool
    public void GenerateWave()
    {
        Debug.Log("GENERATING WAVE FROM EDITOR");
        List<Enemy> enemies = FindObjectsOfType<Enemy>().ToList();
        Wave newWave = new Wave()
        {
            pattern = Pattern.Custom,
            enemies = extractPrefabsFromList(enemies),
            enemyLocations = getEnemyLocations(enemies)
        };
        AssetDatabase.CreateAsset(newWave, "Assets/_YOUR_CODE_GOES_HERE/Waves/newWave.asset");

        foreach (Enemy o in enemies)
        {
            DestroyImmediate(o.gameObject);
        }
    }
    private List<GameObject> extractPrefabsFromList(List<Enemy> raw)
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (Enemy e in raw)
        {
            temp.Add(e.GetPrefabReference());
        }
        return temp;
    }
    private List<Vector3> getEnemyLocations(List<Enemy> enemies)
    {
        List<Vector3> temp = new List<Vector3>();
        foreach (Enemy e in enemies)
        {
            temp.Add(e.transform.position);
        }
        return temp;
    }
    #endregion

}
