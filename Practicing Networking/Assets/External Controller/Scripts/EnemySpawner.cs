using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {

    public GameObject enemyPrefab;
    public int numEnemies;

    public override void OnStartServer()
    {
        for (int i=0; i < numEnemies; i++)
        {
            var pos = new Vector3(
                Random.Range(-8.0f, 8.0f),
                Random.Range(-8.0f, 8.0f),
                0.2f
                );

            //var rotation = Quaternion.Euler( Random.Range(0,180), Random.Range(0,180), Random.Range(0,180));

            var enemy = (GameObject)Instantiate(enemyPrefab, pos, Quaternion.identity);
            NetworkServer.Spawn(enemy);
        }
    }
}