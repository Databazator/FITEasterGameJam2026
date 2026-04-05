using DG.Tweening;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{    
    GUIManager gui;
    public string SendoffMessage;

    private void Start()
    {
        gui = GUIManager.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        gui.Fade(1f, 0.5f, DG.Tweening.Ease.InOutQuad);

        gui.WriteText(SendoffMessage, 4f);


        DOVirtual.DelayedCall(5f, () =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        });
    }
}
