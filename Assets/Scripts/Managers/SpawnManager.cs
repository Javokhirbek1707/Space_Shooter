using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _enemy;
    [SerializeField]
    private GameObject[] _powerups;

    private bool _stopSpawning = false;

    private bool _spawnWaveOne;
    private bool _spawnWaveTwo;
    private bool _spawnWaveThree;
    private bool _spawnBoss;

    public void StartSpawning()
    {
        _spawnWaveOne = true;
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false && _spawnWaveOne == true)
        {
            int randomEnemy = Random.Range(0, 6);
            Vector3 posToSpawn= new Vector3(Random.Range(-8f,8f),7,0);
            GameObject newEnemy = Instantiate(_enemy[randomEnemy], posToSpawn,Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }

        while (_stopSpawning == false && _spawnWaveTwo == true)
        {
            int randomEnemy = Random.Range(0,6);
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemy[randomEnemy], posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(3.5f);
        }

        while (_stopSpawning == false && _spawnWaveThree == true)
        {
            int randomEnemy = Random.Range(0,6);
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemy[randomEnemy], posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(2.0f);
        }

    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
            int randomRoll = Random.Range(0, 100);
            int randomPowerup;
            if (randomRoll <= 80)
            {
                randomPowerup = Random.Range(0, 5);
                Instantiate(_powerups[randomPowerup], posToSpawn, Quaternion.identity);
            }
            else if (randomRoll > 80)
            {
                randomPowerup = Random.Range(5, 7);
                Instantiate(_powerups[randomPowerup], posToSpawn, Quaternion.identity);
            }
            yield return new WaitForSeconds(Random.Range(5.0f, 8.0f));
        }
    }
    public void EmergencyAmmoSpawn()
    {
        Vector3 ammoSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
        Instantiate(_powerups[3], ammoSpawn, Quaternion.identity);
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void WaveTwo()
    {
        _spawnWaveOne = false;
        _spawnWaveTwo = true;
    }

    public void WaveThree()
    {
        _spawnWaveTwo = false;
        _spawnWaveThree = true;
    }

    public void BossWave()
    {
        _spawnWaveThree = false;
        StartCoroutine(SpawnBossRoutine());
        _spawnBoss = true;
        Debug.Log("Wave 4: Boss Wave");
    }

    IEnumerator SpawnBossRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        if (_spawnBoss == true)
        {
            Vector3 posToSpawn = new Vector3(0, 9, 0);
            GameObject boss = Instantiate(_enemy[4], posToSpawn, Quaternion.identity);
            boss.transform.parent = _enemyContainer.transform;
        }
    }

    public void BossDied()
    {
        _spawnBoss = false;
    }
}
