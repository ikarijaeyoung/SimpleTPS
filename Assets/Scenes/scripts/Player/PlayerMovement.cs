using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _avatar;
    [SerializeField] private Transform _aim;

    private Rigidbody _rigidbody;
    private PlayerStatus _playerStatus;

    [Header("Mouse Config")]
    [SerializeField][Range(-90, 0)] private float _minPitch;
    [SerializeField][Range(0, 90)] private float _maxPitch;
    [SerializeField][Range(0, 5)] private float _mouseSensitivity = 1;
    public Vector2 InputDirection { get; private set; }
    public Vector2 MouseDirection { get; private set; }

    private Vector2 _currentRotation;
    private void Awake() => Init();
    private void Init()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerStatus = GetComponent<PlayerStatus>();
    }
    public Vector3 SetMove(float moveSpeed)
    {
        Vector3 moveDirection = GetMoveDirection();

        Vector3 velocity = _rigidbody.velocity;
        velocity.x = moveDirection.x * moveSpeed;
        velocity.z = moveDirection.z * moveSpeed;

        _rigidbody.velocity = velocity;

        return moveDirection;
    }
    public Vector3 SetAimRotation()
    {
        // Vector2 mouseDir = GetMouseDirection();
        // 
        // //  x���� ����� ������ �� �ʿ� ����
        _currentRotation.x += MouseDirection.x;

        _currentRotation.y = Mathf.Clamp(
            _currentRotation.y + MouseDirection.y,
            _minPitch,
            _maxPitch
            );

        // Vector2 currentRotation = new()
        // {
        //     x = transform.rotation.eulerAngles.x,
        //     y = transform.rotation.eulerAngles.y
        // };

        // currentRotation.x += mouseDir.x;
        // currentRotation.y = Mathf.Clamp(
        //     currentRotation.y + mouseDir.y,
        //     _minPitch,
        //     _maxPitch
        //     );

        // ĳ���� ������Ʈ�� ��쿡�� Y�� ȸ���� �ݿ�
        transform.rotation = Quaternion.Euler(0, _currentRotation.x, 0);

        // ������ ��� ���� ȸ�� �ݿ�
        Vector3 currentEuler = _aim.localEulerAngles;
        _aim.localEulerAngles = new Vector3(_currentRotation.y, currentEuler.y, currentEuler.z);

        // ȸ�� ���� ���� ��ȯ
        Vector3 rotateDirVector = transform.forward;
        rotateDirVector.y = 0;
        return rotateDirVector.normalized;
    }
    public void SetAvatarRotation(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Quaternion targetRoation = Quaternion.LookRotation(direction);
        _avatar.rotation = Quaternion.Lerp(
            _avatar.rotation,
            targetRoation,
            _playerStatus.RotateSpeed * Time.deltaTime);
    }
    public Vector3 GetMoveDirection()
    {
        // Vector2 input = InputDirection;

        Vector3 direction =
            (transform.right * InputDirection.x) +
            (transform.forward * InputDirection.y);

        return direction.normalized;
    }
    public void OnMove(InputValue value)
    {
        InputDirection = value.Get<Vector2>();
        // Debug.Log(InputDirection);
    }
    public void OnRotate(InputValue value)
    {
        Vector2 mouseDir = value.Get<Vector2>();
        mouseDir.y *= -1;
        MouseDirection = mouseDir * _mouseSensitivity;
    }

    // public Vector3 GetInputDirection()
    // {
    //     float x = Input.GetAxisRaw("Horizontal");
    //     float z = Input.GetAxisRaw("Vertical");
    // 
    //     return new Vector3(x, 0, z);
    // }
    // private Vector2 GetMouseDirection()
    // {
    //     float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
    //     float mouseY = -Input.GetAxis("Mouse Y") * _mouseSensitivity;
    // 
    //     return new Vector2(mouseX, mouseY);
    // }
}
