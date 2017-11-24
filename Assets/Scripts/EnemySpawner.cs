using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : NetworkBehaviour
{

    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;
    public int numberOfEnemies;
    public int counter;

    public List<GameObject>  monster;
    public List<Vector3> monsterPosition;
    public List<Quaternion> monsterRotation;




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

            monster.Add(enemy);
            monsterPosition.Add(spawnPosition);
            //monsterRotation.Add(monsterRotation);


            NetworkServer.Spawn(enemy);
            enemy.GetComponent<MonsterID>().UniquieMonsterID = "Monster1_" + i;
            counter = i;

            StartCoroutine(IRespawn());
        }
    }
    [ServerCallback]
    void Start()
    {
        
    }

    [ServerCallback]
    void Update()
    {
        //StartCoroutine(IRespawn());
    }
    [ServerCallback]
    IEnumerator IRespawn()
    {
        for (;;)
        {
            RespawnMonster();
            yield return new WaitForSeconds(10f);
        }
       
        
    }

    void RespawnMonster()
    {
        for (int i = 0; i < monster.Count; i++)
        {
            //Debug.Log("Monster : " + monster[i].name);
            if (monster[i] == null)
            {


                var spawnRotation = Quaternion.Euler(
                    0.0f,
                    Random.Range(0, 180),
                    0.0f);
                Debug.Log("Monster Death");
                var enemy = (GameObject)Instantiate(enemyPrefab, monsterPosition[i], spawnRotation);
                NetworkServer.Spawn(enemy);
                enemy.GetComponent<MonsterID>().UniquieMonsterID = "Monster1_" + i;
                monster[i] = enemy;
            }
        }
    }
}