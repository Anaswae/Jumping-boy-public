using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneratorController : MonoBehaviour {

    public GameObject enemyPrefabs;
    public float generatorTimer = 2f;


	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateEnemy(){
        Instantiate(enemyPrefabs, transform.position, Quaternion.identity);
    }

    public void StartGenerator(){
        InvokeRepeating("CreateEnemy", 0f, generatorTimer);
    }

    public void CancelGenerator(bool clean = false){
        CancelInvoke("CreateEnemy");
        if (clean){
            Object[] allEnemy = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in allEnemy){
                Destroy(enemy);
            }
        }
    }
}
