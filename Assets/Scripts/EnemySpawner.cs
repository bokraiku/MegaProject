using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour
{

    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;
    public int numberOfEnemies;
    public int counter;

    public override void OnStartServer()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            var spawnPosition = new Vector3(
                Random.Range((enemySpawnPoint.position.x - 20), enemySpawnPoint.position.x),
                0.0f,
                Random.Range((enemySpawnPoint.position.z - 20), enemySpawnPoint.position.z));

            var spawnRotation = Quaternion.Euler(
                0.0f,
                Random.Range(0, 180),
                0.0f);

            var enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, spawnRotation);
            NetworkServer.Spawn(enemy);
            enemy.GetComponent<MonsterID>().UniquieMonsterID = "Monster1_" + i;
        }
    }
}