using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerMT : MonoBehaviour {
    public EnemyTM enemy;
    public int xPos;
    public int zPos;
    public int enemyCount;

    void Start() {  
        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop() {
        while (enemyCount <10){
            xPos =-Random.Range(48, 53);
            zPos = Random.Range(0, 20);
            Vector3 spawnPosition = new Vector3(xPos, 17, zPos);

            Debug.Log("Spawning enemy at: " + spawnPosition);

            Instantiate(enemy, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(6); 
            enemyCount++;
        }
    }
}