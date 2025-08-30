using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 5f;
    private float _speedMultiple = 2;
    private float _sheftSpeed = 8f;

    [SerializeField]
    private GameObject _laserPrefabs;
    [SerializeField]
    private GameObject _triplelaserPrefabs;
    [SerializeField]
    private GameObject _superLaserPrefab;
    [SerializeField]
    private GameObject _homingProjectilePrefab;
    [SerializeField] 
    private bool _canFireHoming;
    [SerializeField] 
    private float _homingFireRate = 1f;
    private float _whenCanHomingFire = -1f;

    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _shieldLives = 3;
    private SpawnManager _spawnManager;

    private bool _isSuperLaserActive = false;
    private bool _tripleLaserActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isSheildActive = false;
    private bool _isSheftSpeedActive = false;
    private bool _isAmmoReloadActive = false;

    [SerializeField]
    private GameObject _sheildVizualizer;
    [SerializeField]
    private GameObject _rightDamage, _leftDamage;
    [SerializeField]
    private GameObject _thursterLight;

    [SerializeField]
    private int _score;
    [SerializeField]
    private AudioClip _laserSound;
    private AudioSource _audioSource;
    private ShakeBehavior _shakeBehavior;
    private UIManager _uiManager;

    private float _thursterMaxEnergy = 100;
    private float _thursterCurrentEnergy;
    [SerializeField]
    private float _thursterMultiplyEnergy = 10f;

    [SerializeField]
    private int _maxAmmoCount = 15;
    [SerializeField]
    private int _currentAmmoCount;
    private int _currentKills = 0;


    void Start()
    {
        _thursterCurrentEnergy = _thursterMaxEnergy;    
        _currentAmmoCount = _maxAmmoCount;
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _uiManager.UpdateThursterBar(_thursterCurrentEnergy);
        _audioSource = GetComponent<AudioSource>();
        _shakeBehavior = GameObject.Find("Main Camera").GetComponent<ShakeBehavior>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is null.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is null.");
        }

        if (_shakeBehavior == null)
        {
            Debug.LogError("Main Camera Shake behavior is NULL");
        }

        else
        {
            _audioSource.clip = _laserSound;
        }
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        _sheftSpeedActivation();

        if (Input.GetKeyDown(KeyCode.M) && _whenCanHomingFire < Time.time && _canFireHoming)
        {
            _whenCanHomingFire = Time.time + _homingFireRate;
            Instantiate(_homingProjectilePrefab, transform.position, Quaternion.identity);
        }
    }

    public void EnemyKillCount()
    {
        _currentKills++;
        Debug.Log(_currentKills);

        if (_currentKills == 15)
        {
            _spawnManager.WaveTwo();
            _uiManager.WaveTwoUI();
        }

        if (_currentKills == 25)
        {
            _spawnManager.WaveThree();
            _uiManager.WaveThreeUI();
        }

        if (_currentKills == 40)
        {
            _spawnManager.BossWave();
            _uiManager.WaveFourUI();
        }
    }

    private void _sheftSpeedActivation()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _isSpeedBoostActive == false  && _thursterCurrentEnergy > 0)
        {
            _speed = _sheftSpeed;
            _thursterLight.SetActive(true);
            _thursterCurrentEnergy -= Time.deltaTime * _thursterMultiplyEnergy;
            _uiManager.UpdateThursterBar(_thursterCurrentEnergy);
        }
        else if (_isSpeedBoostActive == true)
        {
            _thursterLight.SetActive(true);
        }
        else
        {
            _speed = 5;
            _thursterLight.SetActive(false);
            if(_thursterCurrentEnergy < _thursterMaxEnergy)
            {
                _thursterCurrentEnergy += Time.deltaTime * _thursterMultiplyEnergy;
                _uiManager.UpdateThursterBar(_thursterCurrentEnergy);
            }
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y >= 5.9f)
        {
            transform.position = new Vector3(transform.position.x, 5.9f, 0);
        }

        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x >= 9.15f)
        {
            transform.position = new Vector3(9.15f, transform.position.y, 0);
        }

        else if (transform.position.x <= -9.15f)
        {
            transform.position = new Vector3(-9.15f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _currentAmmoCount -= 1;

        if (_currentAmmoCount < 1)
        {
            _currentAmmoCount = 0;
            _laserPrefabs.SetActive(false);
            _spawnManager.EmergencyAmmoSpawn();
        }

        else
        {
            _laserPrefabs.SetActive(true);
        }

        _uiManager.UpdateAmmoText(_currentAmmoCount);
        _canFire = Time.time + _fireRate;

        if (_tripleLaserActive == true)
        {
            Instantiate(_triplelaserPrefabs, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        else if (_isSuperLaserActive == true)
        {
            Instantiate(_superLaserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        else
        {
            Instantiate(_laserPrefabs, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }


    public void Damage()
    {
        if (_isSheildActive == true)
        {
            _shieldLives--;

            if (_shieldLives < 1)
            {
                _isSheildActive = false;
                _sheildVizualizer.SetActive(false);
                return;
            }

            switch (_shieldLives)
            {
                case 1:
                    _sheildVizualizer.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
                    break;
                case 2:
                    _sheildVizualizer.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
                    break;
                case 3:
                    _sheildVizualizer.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    break;
            }
        }
        else
        {
            _lives--;

            if (_lives == 2)
            {
                _rightDamage.SetActive(true);
            }
            else if (_lives == 1)
            {
                _leftDamage.SetActive(true);
            }
            _uiManager.UpdateLives(_lives);
            if (_lives < 1)
            {
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
            }
        }

        _shakeBehavior.TriggerShake();
    }

    public void TripleShotActive ()
    {
       _tripleLaserActive = true;
        StartCoroutine(TripleShotDownRoutine());
    }

    public void SuperLaserActive()
    {
        _isSuperLaserActive = true;
        StartCoroutine(TripleShotDownRoutine());
    }

    IEnumerator TripleShotDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleLaserActive = false;
        _isSuperLaserActive = false;
    }

   public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiple;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiple;
    }

    public void ShieldActive()
    {
        _isSheildActive = true;
        _sheildVizualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void AddAmmo(int ammopoint)
    {
        _currentAmmoCount -= ammopoint;
        _uiManager.UpdateAmmoText(_currentAmmoCount);
    }

    public void AmmoReloadActive()
    {
        _currentAmmoCount =+ 15;
    }

    public void AddHealthPowerup()
    {
        if(_lives < 3)
        {
            _lives += 1;
            if(_lives == 2)
            {
                _leftDamage.SetActive(false);
            }
            if(_lives == 3)
            {
                _rightDamage.SetActive(false);
            }
            _uiManager.UpdateLives(_lives);
        }
    }

    public void NegativePowerup()
    {
        _speed = 1;
        StartCoroutine(NegativePowerupRoutine());
    }

    IEnumerator NegativePowerupRoutine()
    {
        Debug.Log("It's Working");
        yield return new WaitForSeconds(5f);
        _speed = 5;
    }
}
