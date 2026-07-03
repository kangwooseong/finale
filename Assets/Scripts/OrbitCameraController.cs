using UnityEngine;

public class OrbitCameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Orbit")]
    public float distance = 3.0f;
    public float minDistance = 0.8f;
    public float maxDistance = 8.0f;
    public float orbitSpeed = 180.0f;
    public float zoomSpeed = 2.0f;

    [Header("Pitch Limit")]
    public float minPitch = -20.0f;
    public float maxPitch = 70.0f;

    [Header("Pan")]
    public float panSpeed = 0.005f;

    private float yaw = 0.0f;
    private float pitch = 15.0f;
    private Vector3 targetOffset = Vector3.zero;

    void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("OrbitCameraController target is not assigned.");
            return;
        }

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        UpdateCameraPosition();
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // 우클릭 드래그: 회전
        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * orbitSpeed * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * orbitSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        // 마우스 휠: 확대/축소
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.001f)
        {
            distance -= scroll * zoomSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }

        // 휠 클릭 드래그: 화면 평행 이동
        if (Input.GetMouseButton(2))
        {
            Vector3 right = transform.right;
            Vector3 up = transform.up;

            targetOffset -= right * Input.GetAxis("Mouse X") * panSpeed * distance;
            targetOffset -= up * Input.GetAxis("Mouse Y") * panSpeed * distance;
        }

        // R 키: 카메라 리셋
        if (Input.GetKeyDown(KeyCode.R))
        {
            yaw = 0.0f;
            pitch = 15.0f;
            distance = 3.0f;
            targetOffset = Vector3.zero;
        }

        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0.0f);
        Vector3 targetPosition = target.position + targetOffset;

        Vector3 cameraPosition = targetPosition - rotation * Vector3.forward * distance;

        transform.position = cameraPosition;
        transform.rotation = rotation;
    }
}