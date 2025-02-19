using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private float _zoomAmount;
    [SerializeField] 
    private float _zoomSpeed;
    [SerializeField]
    private CinemachineCamera _cinemachineCamera;

    private CinemachineFollow _cinemachineFollow;
    private Vector3 _targetFollowOffset;

    private void Start()
    {
        _cinemachineFollow = _cinemachineCamera.GetComponent<CinemachineFollow>();
        _targetFollowOffset = _cinemachineFollow.FollowOffset;
    }
    private void Update()
    {
        CameraMovement();
        CameraRotation();
        CameraZoom();
    }

    private void CameraMovement()
    {
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();


        //for proper application for camera rotation
        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
        transform.position += moveVector * _moveSpeed * Time.deltaTime;

    }

    private void CameraRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);
        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();

        transform.eulerAngles += rotationVector * _rotateSpeed * Time.deltaTime;
    }

    private void CameraZoom()
    {
      
        float zoomIncreaseAmount = 1f;
        _targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;

        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        _cinemachineFollow.FollowOffset = Vector3.Lerp(_cinemachineFollow.FollowOffset, _targetFollowOffset, _zoomAmount * Time.deltaTime);

    }
}
