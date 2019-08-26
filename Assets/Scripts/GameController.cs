using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof(Image))]
public class GameController : MonoBehaviour {

    private float finalParallaxSpeed;

    [Range (0f, 0.20f)]
    public float parallaxSpped = 0.02f;
    public RawImage background;
    public RawImage plataform;
    public GameObject uiIdle;
    public GameObject uiScore;
    public GameObject uiContinue;
    public GameObject player;
    public GameObject sound;
    public GameObject enemyGenerator;
    public AudioSource audioGame;
    public float scaleTime = 6f;
    public float scaleInc = 0.25f;
    private int points = 0;
    public Text textPoint;
    public Text textScore;
    public Sprite altavoz;
    public Sprite mute;


    public enum SoundState { Mute, Altavoz }
    public SoundState soundState = SoundState.Altavoz;
    public enum GameState { Idle, Playing, Ended, Ready}
    public GameState gameState = GameState.Idle;

	// Use this for initialization
	void Start () {
        audioGame = GetComponent<AudioSource>();
        textScore.text = "Best: "+GetMaxScore().ToString();

        this.InitializeSound();

	}

    // Update is called once per frame
    void Update() {
        GetGameState();
	}

    private void InitializeSound(){
        if (GetSoundState() == 0){
            soundState = SoundState.Altavoz;
            sound.GetComponent<Image>().sprite = altavoz;
        }else {
            soundState = SoundState.Mute;
            sound.GetComponent<Image>().sprite = mute;
        }
    }

    public void FunStateSound(){
        if (soundState == SoundState.Altavoz){
            soundState = SoundState.Mute;
            sound.GetComponent<Image>().sprite = mute;
            SetSoundState(1);
        }else {
            soundState = SoundState.Altavoz;
            sound.GetComponent<Image>().sprite = altavoz;
            SetSoundState(0);
        }
    }

    public void StartGame(){
        if (gameState == GameState.Idle){
            gameState = GameState.Playing;
            uiIdle.SetActive(false);
            uiScore.SetActive(true);
            player.SendMessage("UpdateState","PlayerRun");
            player.SendMessage("DustPlay");
            enemyGenerator.SendMessage("StartGenerator");
            InvokeRepeating("GameTimeScale", scaleTime, scaleTime);

            if (soundState == SoundState.Altavoz) { audioGame.Play(); }
        }
    }

    private void GetGameState(){
        if (gameState == GameState.Playing){
            Parallax();
        }else if (gameState == GameState.Ready){
            uiContinue.SetActive(true);
            if (Input.GetMouseButtonDown(0)){
                RestartGame();
            }
        }
    }

    private void Parallax(){
        finalParallaxSpeed = parallaxSpped * Time.deltaTime;
        background.uvRect = new Rect(background.uvRect.x + finalParallaxSpeed, 0f, 1f, 1f);
        plataform.uvRect = new Rect(plataform.uvRect.x + finalParallaxSpeed * 4, 0f, 1f, 1f);
    }

    public void RestartGame(){
        SceneManager.LoadScene("PrincipalScene");
    }

    void GameTimeScale(){
        Time.timeScale += scaleInc;
    }

    public void ResetTimeScale(){
        CancelInvoke("GameTimeScale");
        Time.timeScale = 1f;
    }

    public void IncreasePoints(){
        textPoint.text = (++points).ToString();
        if (points >= GetMaxScore()){
            textScore.text = "Best: " + points.ToString();
            SetMaxScore(points);
        }

    }

    public int GetMaxScore(){
        return PlayerPrefs.GetInt("MaxScore", 0);
    }

    public void SetMaxScore(int currentScore){
        PlayerPrefs.SetInt("MaxScore", currentScore);
    }

    public int GetSoundState(){
        return PlayerPrefs.GetInt("SoundState", 0);
    }

    public void SetSoundState(int currentState){
        PlayerPrefs.SetInt("SoundState", currentState);
    }
}
