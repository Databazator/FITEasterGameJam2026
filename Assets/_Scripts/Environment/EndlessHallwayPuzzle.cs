
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class EndlessHallwayPuzzle : MonoBehaviour
{
    private int _cycleCounter = 0;
    public int CounterReadout;

    public float PaintingRotationAmount;
    public float PaintingRotationDuration;

    public GameObject SecretButton;
    //public Doorway SecretDoorway;

    public int UnlockSecretAt;

    public List<Transform> Paintings;

    public List<ParticleSystemRenderer> godrayRenderers;
    private List<Material> godrayMaterials = new List<Material>();
    public float godrayMatStartAlpha;
    public ParticleSystemRenderer secretDoorGuide;
    private Material secretDoorGuideMat;
    public Color godrayColor;

    public int FadeGodraysAfterUnlockCount = 3;

    public Color NewGodrayColor;
    public float NewGodrayAlpha;

    public void IncreaseCounter()
    {
        _cycleCounter++;
        CounterReadout = _cycleCounter;

        if (_cycleCounter <= UnlockSecretAt)
        {
            foreach (Transform t in Paintings)
            {
                t.DOLocalRotate(new Vector3(PaintingRotationAmount * _cycleCounter, 0f, 0f), PaintingRotationDuration).SetEase(Ease.InOutCubic);
            }

            if (_cycleCounter >= UnlockSecretAt)
            {
                SecretButton.SetActive(true);                
            }
        }
        else
        {
            if (_cycleCounter - UnlockSecretAt <= FadeGodraysAfterUnlockCount)
            {
                float factor = (float)Mathf.Min(FadeGodraysAfterUnlockCount, _cycleCounter - UnlockSecretAt) / (float)FadeGodraysAfterUnlockCount;

                float newAlpha = Mathf.Lerp(godrayMatStartAlpha, 0f, factor);
                NewGodrayAlpha = newAlpha;
                Color newColor = new Color(godrayColor.r, godrayColor.g, godrayColor.b, newAlpha);
                NewGodrayColor = newColor;
                foreach (Material mat in godrayMaterials)
                {
                    mat.color = newColor;
                }
            }
        }
    }

    public void ShowSecretDoorGodray()
    {
        secretDoorGuideMat.color = new Color(godrayColor.r, godrayColor.g, godrayColor.b, 1f);
        secretDoorGuide.material = secretDoorGuideMat;
    }
   
    void Start()
    {
        SecretButton.SetActive(false);

        foreach(ParticleSystemRenderer rend in godrayRenderers)
        {
            godrayMaterials.Add(rend.material);
        }
        
        secretDoorGuideMat = secretDoorGuide.material;
        godrayMatStartAlpha = secretDoorGuideMat.color.a;
        godrayColor = secretDoorGuideMat.color;
    }

   
}
