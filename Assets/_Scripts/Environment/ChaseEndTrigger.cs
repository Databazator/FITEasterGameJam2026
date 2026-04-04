using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaseEndTrigger : MonoBehaviour
{
    public string CorridorSceneName = "CorridorWalkScene";
    PlayerController player;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"collided with {other.gameObject}");

        player.SetFreezePlayer(true);
        player.Movement.CamEffects.SetFOV(179f, 0.3f);
        GUIManager.Instance.Fade(1f, 0.5f, Ease.InExpo);
        DOVirtual.DelayedCall(0.5f, () => SceneManager.LoadScene(CorridorSceneName));        
    }
}
