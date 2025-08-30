using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _textScore;
    [SerializeField]
    private Text _maxAmmoCount;
    [SerializeField] 
    private Text _ammoCount;
    [SerializeField]
    private Text _gameOver;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private TMP_Text _gameOverWinText;
    [SerializeField]
    private Image _liveImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private TMP_Text[] _waves;

    [SerializeField]
    private Slider _thursterBar;

    private GameManager _gameManager;

    void Start()
    {
        _textScore.text = "Score: " + 0;
        _maxAmmoCount.text = "Max Ammo Count: " + 15;
        _gameOver.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        SetWavePos();
    }

    public void UpdateThursterBar(float thursterEnergy)
    {
        _thursterBar.value = thursterEnergy;
    }

    public void UpdateScore(int playerScore)
    {
        _textScore.text = "Score: " + playerScore.ToString();
    }

    private void SetWavePos()
    {
        for (int i = 0; i < _waves.Length; i++)
        {
            _waves[i].gameObject.SetActive(false);
        }
    }

    public void WaveOneUI()
    {
        _waves[0].gameObject.SetActive(true);
    }

    public void WaveTwoUI()
    {
        _waves[1].gameObject.SetActive(true);
        _waves[0].gameObject.SetActive(false);
    }

    public void WaveThreeUI()
    {
        _waves[2].gameObject.SetActive(true);
        _waves[1].gameObject.SetActive(false);
    }

    public void WaveFourUI()
    {
        _waves[3].gameObject.SetActive(true);
        _waves[2].gameObject.SetActive(false);
    }

    public void UpdateAmmoText(int ammoCount)
    {
        _ammoCount.text = "CURRENT AMMO: " + ammoCount.ToString();

        if (ammoCount == 0)
        {
            StartCoroutine(LowAmmoFlicker());
        }
        
    }

    IEnumerator LowAmmoFlicker()
    {
        while (true)
        {
            _ammoCount.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            _ammoCount.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            _ammoCount.gameObject.SetActive(false);
        }
    }

    public void UpdateLives(int currentLives)
    {
        if(currentLives < 0 || currentLives >= _liveSprites.Length)
        {
            return;
        }
        _liveImg.sprite = _liveSprites[currentLives];
        if(currentLives == 0)
        {
            GameOverSquences();
        }
    }

    public void GameOverSquences()
    {
        _gameManager.GameOver();
        _gameOver.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
        _restartText.gameObject.SetActive(true);
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOver.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOver.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void GameWinner()
    {
        _gameOverWinText.gameObject.SetActive(true);
        GameOverSquences();
    }
}
