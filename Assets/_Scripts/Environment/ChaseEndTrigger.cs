using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaseEndTrigger : MonoBehaviour
{
    public string CorridorSceneName = "CorridorWalkScene";

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"collided with {other.gameObject}");

        SceneManager.LoadScene(CorridorSceneName);
    }
}
