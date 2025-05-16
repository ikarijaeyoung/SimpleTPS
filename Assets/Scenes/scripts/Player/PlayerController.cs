using Cinemachine;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;
// ���� ������ �ӽ� ���ӽ����̽� ����
// �۾��� ���� �� �Ʒ� ���� �ּ�ó��
// using PlayerMovement = _Test.PlayerMovement;

public class PlayerController : MonoBehaviour, IDamagable
{
    public bool IsControlActivate { get; set; } = true;

    private PlayerStatus _status;
    private PlayerMovement _movement;

    [SerializeField] private CinemachineVirtualCamera _aimCamera;
    [SerializeField] private Gun _gun;

    [SerializeField] private KeyCode _aimKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode _shootKey = KeyCode.Mouse0;

    private Animator _animator;

    [SerializeField] private Animator _aimAnimator;
    private Image _aimImage;

    [SerializeField] private HpGaugeUI _hpUI;

    private void Awake()
    {
        Init();
    }
    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void Update()
    {
        HandlePlayerControl();
    }
    private void OnDisable()
    {
        UnsubscribeEvents();
    }





    private void Init()
    {
        _status = GetComponent<PlayerStatus>();
        _movement = GetComponent<PlayerMovement>();
        // _mainCamera = Camera.main.gameObject;
        _animator = GetComponent<Animator>();
        _aimImage = _aimAnimator.GetComponent<Image>();

        _hpUI.SetImageFillAmount(1);
        _status.CurrentHP.Value = _status.MaxHP;
    }
    private void HandlePlayerControl()
    {
        if (!IsControlActivate) return;

        HandleMovement();
        HandleAiming();
        HandleShooting();

        // Test
        if(Input.GetKey(KeyCode.Alpha1))
        {
            TakeDamage(1);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            RecoveryHP(1);
        }
    }
    private void HandleShooting()
    {
        if (_status.IsAiming.Value && Input.GetKey(_shootKey))
        {
            _status.IsAttacking.Value = _gun.Shoot();
        }
        else
        {
            _status.IsAttacking.Value = false;
        }
    }
    private void HandleMovement()
    {
        // (ȸ�� ���� ��)�¿� ȸ���� ���� ���� ��ȯ
        Vector3 camRotateDir = _movement.SetAimRotation();

        float moveSpeed;
        if (_status.IsAiming.Value) moveSpeed = _status.WalkSpeed;
        else moveSpeed = _status.RunSpeed;

        Vector3 moveDir = _movement.SetMove(moveSpeed);
        _status.IsMoving.Value = (moveDir != Vector3.zero);

        // TODO: ��ü�� ȸ����� ���� �� ȣ���ؾ� ��.

        Vector3 avatarDir;
        if (_status.IsAiming.Value) avatarDir = camRotateDir;
        else avatarDir = moveDir;

        _movement.SetAvatarRotation(avatarDir);

        // SetAnimationParamater
        // Aim ������ ����
        if (_status.IsAiming.Value)
        {
            Vector3 input = _movement.GetInputDirection();
            _animator.SetFloat("X", input.x);
            _animator.SetFloat("Z", input.z);
        }
    }

    private void HandleAiming()
    {
        _status.IsAiming.Value = Input.GetKey(_aimKey);

    }
    public void TakeDamage(int value)
    {
        // ���� ü���� ���ҽ�Ű����, ü���� 0���ϰ� �Ǹ� �÷��̾� ���ó��.
        _status.CurrentHP.Value -= value;
        if (_status.CurrentHP.Value <= 0) Dead();
    }
    public void RecoveryHP(int value)
    {
        // ���� ü���� ������Ű����, MaxHP ���ϱ�����.
        int hp = _status.CurrentHP.Value + value;

        _status.CurrentHP.Value = Mathf.Clamp(
            hp,
            0,
            _status.MaxHP);
    }
    public void Dead()
    {
        // �ָ��� ȥ�� �õ� �� ����.
        // ���
        Debug.Log("���");
    }
    public void SubscribeEvents()
    {
        _status.IsAiming.Subscribe(_aimCamera.gameObject.SetActive);

        _status.IsAiming.Subscribe(SetAimAnimation);
        _status.IsMoving.Subscribe(SetMoveAnimation);
        _status.IsAttacking.Subscribe(SetAttackAnimation);
        _status.CurrentHP.Subscribe(SetHpUIGauge);
    }

    public void UnsubscribeEvents()
    {
        _status.IsAiming.Unsubscribe(_aimCamera.gameObject.SetActive);

        _status.IsAiming.Unsubscribe(SetAimAnimation);
        _status.IsMoving.Unsubscribe(SetMoveAnimation);
        _status.IsAttacking.Unsubscribe(SetAttackAnimation);
        _status.CurrentHP.Unsubscribe(SetHpUIGauge);
    }

    // private void SetActivateAimCamera(bool value)
    // {
    //     _aimCamera.SetActive(value);
    //     _mainCamera.SetActive(!value);
    // }

    private void SetAimAnimation(bool value)
    {
        if (!_aimImage.enabled) _aimImage.enabled = true;
        _animator.SetBool("IsAim", value);
        _aimAnimator.SetBool("IsAim", value);
    }
    private void SetMoveAnimation(bool value) => _animator.SetBool("IsMove", value);
    private void SetAttackAnimation(bool value) => _animator.SetBool("IsAttack", value);


    // �����ġ / �ִ��ġ
    private void SetHpUIGauge(int currentHp)
    {
        float hp = currentHp / (float)_status.MaxHP; // ����ȯ �� �ϸ� ���� 0�� ������.
        _hpUI.SetImageFillAmount(hp);
    }


}