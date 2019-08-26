using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {


    public float velocity = 2f;

    public Rigidbody2D body2d;

	// Use this for initialization
	void Start () {
        body2d = GetComponent<Rigidbody2D>();
        body2d.velocity = Vector2.left * velocity;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Destroyer") Destroy(gameObject);
    }
}
