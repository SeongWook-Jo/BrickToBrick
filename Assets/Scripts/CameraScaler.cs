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
            // ���� ȭ���� �� ���� ���
            cam.orthographicSize = referenceOrthographicSize;
        }
        else
        {
            // ���� ȭ���� �� ���� ���
            float differenceInSize = targetRatio / screenRatio;
            cam.orthographicSize = referenceOrthographicSize * differenceInSize;
        }
    }

    private void Update()
    {
        // ȭ�� ũ�Ⱑ ����� ������ ī�޶� ������ ������Ʈ
        if (Screen.width != cam.pixelWidth || Screen.height != cam.pixelHeight)
        {
            UpdateCameraSize();
        }
    }
}