using DG.Tweening;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{    
    GUIManager gui;

    private void Start()
    {
        gui = GUIManager.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        gui.Fade(1f, 0.5f, DG.Tweening.Ease.InOutQuad);

        gui.WriteText("Shh. Almost. Just let go...", 4f);


        DOVirtual.DelayedCall(6f, () =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        });
    }
}
