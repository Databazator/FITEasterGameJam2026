using UnityEngine;

public class FPSCameraLook : MonoBehaviour
{
    public Transform PlayerTransform;
    public Transform CameraHolderTransform;
    public Camera Cam;

    public float MouseSensitivity = 10f;
    float pitch;
    public float MinPitch = -90f;
    public float MaxPitch = 90f;


    private void Awake()
    {
        Cam = GetComponentInChildren<Camera>();
    }
    public void CameraLook(Vector2 lookInput)
    {
        lookInput *= MouseSensitivity * Time.deltaTime;
        float yawDelta = lookInput.x;

        pitch -= lookInput.y;
        pitch = Mathf.Clamp(pitch, MinPitch, MaxPitch);

        Cam.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        //Vector3 localUp = PlayerTransform.up;
        //Quaternion rotation = Quaternion.AngleAxis(yawDelta, localUp);
        //PlayerTransform.rotation = rotation * PlayerTransform.rotation;
        CameraHolderTransform.Rotate(Vector3.up * yawDelta, Space.Self);
    }

    public void SetRotation(Vector3 eulerAngles)
    {
        CameraHolderTransform.localRotation = Quaternion.Euler(eulerAngles.x, 0f, 0f);
        PlayerTransform.localRotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
        pitch = eulerAngles.x;
    }

    public Vector3 GetRotation()
    {
        return new Vector3(CameraHolderTransform.localRotation.eulerAngles.x,
                           PlayerTransform.localRotation.eulerAngles.y,
                           0f);
    }

    void SetMouseSensitivity(float value)
    {
        if (value <= 0f) value = 0.1f;
        MouseSensitivity = value;
    } 
}
