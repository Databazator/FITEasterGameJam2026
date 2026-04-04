using DG.Tweening;
using UnityEngine;

public class DreamStartSequence : MonoBehaviour
{
    GUIManager gui;
    PlayerController player;

    public float BlinkOpenDuration = 2.5f;
    void Start()
    {
        gui = GUIManager.Instance;
        player = FindAnyObjectByType<PlayerController>();

        player.SetHasControl(false);

        DOVirtual.DelayedCall(1f, () =>
        {

            gui.Fade(0.25f, 0.8f, Ease.InOutQuad);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                gui.Fade(1f, 0.5f, Ease.InOutQuad);
                DOVirtual.DelayedCall(0.4f, () =>
                {
                    gui.Fade(0f, 0.75f, Ease.InCubic);
                });
            });
        });

        DOVirtual.DelayedCall(BlinkOpenDuration, () => gui.WriteText("Uhh...is it the middle of the night?", 2f));
        
        DOVirtual.DelayedCall(BlinkOpenDuration + 5f, () =>
        {
            gui.WriteText("Did I leave a light on?", 2f);
            player.SetHasControl(true);
        });

        //DOVirtual.DelayedCall(BlinkOpenDuration + 8.5f, () =>
        //{
        //    gui.WriteText("I should probably switch it off", 2.5f);            
        //});
    }

    public void TextVocalizeDissolve()
    {
        DOVirtual.DelayedCall(1f, () => gui.WriteText("What the ...", 2f));
    }
  
}
