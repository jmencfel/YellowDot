using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesManager : MonoBehaviour
{
    public delegate void WaveSpawned(int spawnedWaveNo);
    public event WaveSpawned E_WaveSpawned;



    [SerializeField] private List<Wave> _EnemyWaves;
    private List<Enemy> _CurrentWave;
    private int _ShipsInCurrentWave;
    private int _ShipsLeftInCurrentWave;
    private Vector3 _firstShipPosition = new Vector3(-8.0f, 4.0f, 0);
    
    public int CurrentWave { get; private set; }

    public void SetupWavesAndSpawn()
    {
        //TODO you code goes her
        _CurrentWave = new List<Enemy>();
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
        if (_EnemyWaves[CurrentWave] != null)
        {

            Pattern pattern = _EnemyWaves[CurrentWave].pattern;
            var shipCount = _EnemyWaves[CurrentWave].enemies.Count;
            _ShipsInCurrentWave = shipCount;
            _ShipsLeftInCurrentWave = shipCount;
            var shipSpacing = 15 / shipCount;
            var VecticalSpacing = _EnemyWaves[CurrentWave].vecticalSpacing;
            var CurrentShipPosition = _firstShipPosition;
            if (shipSpacing < 2) shipSpacing = 2;
            int currentLineLimit = 8;
            foreach (Enemy enemy in _EnemyWaves[CurrentWave].enemies)
            {
                var Ship = Instantiate(enemy, CurrentShipPosition, Quaternion.identity);
                Ship.E_ShipDestroyed += OnShipDestroyed;
                CurrentShipPosition += new Vector3(shipSpacing, 0, 0);
                if (CurrentShipPosition.x > currentLineLimit)
                {
                    if (pattern != Pattern.Triangle)
                        CurrentShipPosition.x = -currentLineLimit;
                    else
                        CurrentShipPosition.x = -(--currentLineLimit);
                    CurrentShipPosition.y -= VecticalSpacing;
                }
            }
            //TODO you code goes here
            CurrentWave++;
            E_WaveSpawned?.Invoke(CurrentWave);
        }
    }
    
    
}
