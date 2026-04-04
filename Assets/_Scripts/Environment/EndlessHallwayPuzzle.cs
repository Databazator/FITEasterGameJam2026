
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
    }
   
    void Start()
    {
        //SecretButton.SetActive(false);
    }

   
}
