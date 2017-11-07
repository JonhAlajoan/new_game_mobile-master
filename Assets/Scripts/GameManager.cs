using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public enum SpawnState { SPAWNING, WAITING, COUNTING }; 

    [System.Serializable]
    public class Wave
    {
        public string name;

        //quantidade
        public int count;

        //quantos segundos pra sapawnar
        public float spawnRate;

    }

    public Wave[] waves;
    private int nextWave = 0;

    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    public Transform[] muzzleEnemy;
    private void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    private void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {

                WaveCompleted();
                return;
            }
            else
            {
                return;
            }
        }
        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));                
            }           
           
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }

    }

    void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;



        nextWave++;
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {

                return false;
            }
        }
        return true;
    }

    void SpawnEnemy(string enemy)
    {
        for (int i = 0; i < muzzleEnemy.Length; i++)
        {
            TrashMan.spawn(enemy, muzzleEnemy[i].transform.position, muzzleEnemy[i].transform.rotation);
        }
        
    }

    IEnumerator SpawnWave(Wave wave)
    {
        state = SpawnState.SPAWNING;
        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy("Enemy1");
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }

        state = SpawnState.WAITING;
        yield break;
    }
}
