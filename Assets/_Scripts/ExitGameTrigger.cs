using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
