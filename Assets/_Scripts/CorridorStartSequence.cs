using DG.Tweening;
using UnityEngine;

public class CorridorStartSequence : MonoBehaviour
{
    public AudioClip Ambience;
    public PlayerController player;
    GUIManager gui;
    void Start()
    {
        gui = GUIManager.Instance;
        if(!player) player = FindAnyObjectByType<PlayerController>();
        player.SetHasControl(false);

        AudioManager.Instance?.PlayAmbient(Ambience, 0.2f);

        DOVirtual.DelayedCall(0.25f, () =>
        {
            gui.Fade(0f, 0.3f, Ease.InQuad);

        });
        DOVirtual.DelayedCall(0.5f, () =>
        {
            player.Movement.CamEffects.ShakeCamera(5, 0.35f);
            gui.WriteText("ohh... just a dream...", 2f);
            player.SetHasControl(true);
        });

        DOVirtual.DelayedCall(6f, () =>
        {
            gui.WriteText("I should still turn that light off", 2f);
        });
    }

    public void VocalizeCorridorEntry()
    {
        gui.WriteText("i have no memory of this place", 2.5f);
    }
}
