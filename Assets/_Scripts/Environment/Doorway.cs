using DG.Tweening;
using UnityEngine;


public class Doorway : MonoBehaviour
{
    public Transform Hinge;
    public float OpenedAngle = 110f;
    public float ClosedAngle = 0f;
    public bool OpenAtStart = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!Hinge) Debug.LogWarning("Hidge not assigned");
        if (OpenAtStart)
            OpenDoor();
    }
    
    public void OpenDoor(float duration = 2f)
    {
        Hinge.DOLocalRotate(new Vector3(0f, OpenedAngle, 0f), duration).SetEase(Ease.InOutCubic);
    }
   
    public void CloseDoor(float duration = 2f)
    {
        Hinge.DOLocalRotate(new Vector3(0f, ClosedAngle, 0f), duration).SetEase(Ease.InOutCubic);
    }

    [ContextMenu("Open Door")]
    public void TestDoorOpen()
    {
        OpenDoor();
    }
    [ContextMenu("Close Door")]
    public void TestCloseDoor()
    {
        CloseDoor();
    }
}
