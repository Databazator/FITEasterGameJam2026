using UnityEngine;

public class DreamLaunchInfoShow : MonoBehaviour
{
    public float DistanceFromCam = 5f;
    public Vector3 Offset;
    public Transform CameraTransform;
    public WUILabelPanel LaunchPanel;
    public void Trigger(float hideInDur)
    {
        if(CameraTransform == null || LaunchPanel == null)
        {
            Debug.LogError("Missing Reference!!!");
            return;
        }
        Vector3 newPos = CameraTransform.position + CameraTransform.forward * DistanceFromCam;
        LaunchPanel.transform.position = newPos;
        LaunchPanel.transform.LookAt(LaunchPanel.transform.position + CameraTransform.forward);
        LaunchPanel.transform.Translate(Offset, Space.Self);

        LaunchPanel.ShowPanel(0.5f);
        LaunchPanel.HidePanelWithDelay(hideInDur);
        LaunchPanel.DisablePanelWithDelay(hideInDur + 1f);

    }
}
