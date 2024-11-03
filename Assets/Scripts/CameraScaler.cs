using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScaler : MonoBehaviour
{
    [SerializeField] private float referenceWidth = 1920f;
    [SerializeField] private float referenceHeight = 1080f;
    [SerializeField] private float referenceOrthographicSize = 5f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (!cam.orthographic)
        {
            Debug.LogWarning("This script is designed for orthographic cameras. Please switch the camera to orthographic mode.");
        }
    }

    private void Start()
    {
        UpdateCameraSize();
    }

    private void UpdateCameraSize()
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = referenceWidth / referenceHeight;

        if (screenRatio >= targetRatio)
        {
            // 현재 화면이 더 넓은 경우
            cam.orthographicSize = referenceOrthographicSize;
        }
        else
        {
            // 현재 화면이 더 좁은 경우
            float differenceInSize = targetRatio / screenRatio;
            cam.orthographicSize = referenceOrthographicSize * differenceInSize;
        }
    }

    private void Update()
    {
        // 화면 크기가 변경될 때마다 카메라 사이즈 업데이트
        if (Screen.width != cam.pixelWidth || Screen.height != cam.pixelHeight)
        {
            UpdateCameraSize();
        }
    }
}