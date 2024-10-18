using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
     public Wave[] waves;
    public Enemy enemy;
  
    int currentWaveNumber;
    Wave currentWave;
    float nextSpawnTime;
    int enemiesRemainingToSpawn;
    int enemiesRemainingALive;

    void Start(){
        NextWave();
    }
    
    void Update(){
        if(Time.time>nextSpawnTime && enemiesRemainingToSpawn>0){
            enemiesRemainingToSpawn--;
            nextSpawnTime=Time.time+currentWave.timeBetweenSpawns;
            Enemy spawnedEnemy=Instantiate(enemy, Vector3.zero, Quaternion.identity) as Enemy; 
            spawnedEnemy.OnDeath+=OnEnemyDeath;
        }
    }

    void OnEnemyDeath(){
        enemiesRemainingALive--;
        if(enemiesRemainingALive==0){
            NextWave();
        }
       
    }

    void NextWave(){
        if(currentWaveNumber-1< waves.Length){
            currentWaveNumber++;
            currentWave=waves[currentWaveNumber-1];
            enemiesRemainingToSpawn=currentWave.enemyCount;
        }

        enemiesRemainingALive=enemiesRemainingToSpawn;
    }

    [System.Serializable]
    public class Wave{
        // public GameObject enemyPrefab;
        public int enemyCount;
        public float timeBetweenSpawns;
    }
}
