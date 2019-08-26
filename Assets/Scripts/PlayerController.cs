using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Animator animator;
    private AudioSource audioPlayer;
    public AudioClip jumpClip;
    public AudioClip dieClip;
    public AudioClip pointClip;
    private float startY;
    public ParticleSystem dust;

    public GameObject game;
    public GameObject enemyGenerator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        startY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {

        bool isGrounded = transform.position.y == startY;
        bool gamePlaying = game.GetComponent<GameController>().gameState == GameController.GameState.Playing;

        Debug.Log(" grounded "+isGrounded);

        if (isGrounded && gamePlaying && Input.GetMouseButtonDown(0)){
            UpdateState("PlayerJump");
        }
    }

    public void UpdateState(string state = null){
        if (state != null){
            animator.Play(state);
            if (state == "PlayerJump"){
                audioPlayer.clip = jumpClip;
                bool sound = game.GetComponent<GameController>().soundState == GameController.SoundState.Altavoz;
                if (sound) { audioPlayer.Play(); }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
        bool sound = game.GetComponent<GameController>().soundState == GameController.SoundState.Altavoz;
        if (collision.gameObject.tag == "Enemy"){
            UpdateState("PlayerDie");
            game.GetComponent<GameController>().gameState = GameController.GameState.Ended;
            enemyGenerator.SendMessage("CancelGenerator", true);
            game.SendMessage("ResetTimeScale");
            DustStop();
            
            game.GetComponent<AudioSource>().Stop();
            audioPlayer.clip = dieClip;
            if (sound) { audioPlayer.Play(); }
        }
        else if (collision.gameObject.tag == "Point"){
            audioPlayer.clip = pointClip;
            if (sound) { audioPlayer.Play(); }
            game.SendMessage("IncreasePoints");
        }
    }

    void GameReady(){
        game.GetComponent<GameController>().gameState = GameController.GameState.Ready;
    }

    void DustPlay(){
        dust.Play();
    }

    void DustStop(){
        dust.Stop();
    }
}
