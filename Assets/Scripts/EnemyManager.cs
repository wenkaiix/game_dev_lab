using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        foreach (Transform child in transform)
        {
            // attempting to recreate goomba on reset if i destroy it 
            if (child == null)
            {
                Instantiate(child, transform. position, Quaternion.identity);                
            }
            child.GetComponent<EnemyMovement>().GameRestart();
        }
    }

    void Awake(){
		// other instructions
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }
}
