using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            Bouncebox box = child.GetComponent<Bouncebox>();
            if (box != null)
            {
                box.GameRestart();
            }
        }
    }
}
