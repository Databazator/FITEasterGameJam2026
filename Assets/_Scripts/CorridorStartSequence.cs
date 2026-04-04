using DG.Tweening;
using UnityEngine;

public class CorridorStartSequence : MonoBehaviour
{
    PlayerController player;
    GUIManager gui;
    void Start()
    {
        gui = GUIManager.Instance;
        player = FindAnyObjectByType<PlayerController>();
        player.SetHasControl(false);

        DOVirtual.DelayedCall(0.25f, () =>
        {
            gui.Fade(0f, 0.3f, Ease.InQuad);

        });
        DOVirtual.DelayedCall(0.5f, () =>
        {
            player.Movement.CamEffects.ShakeCamera(5, 0.35f);
            gui.WriteText("oh... just a nightmare...", 2f);
            player.SetHasControl(true);
        });

        DOVirtual.DelayedCall(6f, () =>
        {
            gui.WriteText("I should still turn the light off", 2f);
        });
    }
}
