using DG.Tweening;
using UnityEngine;

public class DreamStartSequence : MonoBehaviour
{
    GUIManager gui;
    PlayerController player;
    AudioManager audioMngr;

    public AudioClip AmbientClip;
    public AudioClip DangerClip;

    public float BlinkOpenDuration = 2.5f;
    void Start()
    {
        audioMngr = AudioManager.Instance;
        gui = GUIManager.Instance;
        player = FindAnyObjectByType<PlayerController>();

        player.SetHasControl(false);
        audioMngr.PlayAmbient(AmbientClip, 0.2f);
        audioMngr.SetDangerClip(DangerClip);

        DOVirtual.DelayedCall(1f, () =>
        {

            gui.Fade(0.25f, 0.8f, Ease.InOutQuad);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                gui.Fade(1f, 0.5f, Ease.InOutQuad);
                DOVirtual.DelayedCall(0.7f, () =>
                {
                    gui.Fade(0f, 0.75f, Ease.InCubic);
                });
            });
        });

        DOVirtual.DelayedCall(BlinkOpenDuration, () => gui.WriteText("uhh...is it the middle of the night?", 2f));
        
        DOVirtual.DelayedCall(BlinkOpenDuration + 4.5f, () =>
        {
            gui.WriteText("did I leave a light on?", 2f);
            player.SetHasControl(true);
        });

        //DOVirtual.DelayedCall(BlinkOpenDuration + 8.5f, () =>
        //{
        //    gui.WriteText("I should probably switch it off", 2.5f);            
        //});
    }

    public void TextVocalizeDissolve()
    {
        DOVirtual.DelayedCall(1.75f, () => gui.WriteText("What the ...", 2f));
    }
       
  
}
