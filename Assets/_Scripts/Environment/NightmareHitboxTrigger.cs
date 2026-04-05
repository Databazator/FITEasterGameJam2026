using DG.Tweening;
using System;
using UnityEngine;

public class NightmareHitboxTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.LogWarning("Player triggered nightmare hitbox");

            PlayerController player = FindAnyObjectByType<PlayerController>();
            GUIManager.Instance.Fade(1f, 0.5f, DG.Tweening.Ease.InQuad);

            GUIManager.Instance.WriteText("It's time to go ... ", 4f);


            DOVirtual.DelayedCall(3f, () =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            });
        }
    }
}
