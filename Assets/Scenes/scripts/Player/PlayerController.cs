using Cinemachine;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;
// 참조 생성용 임시 네임스페이스 참조
// 작업물 병합 시 아래 내용 주석처리
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
        // (회전 수행 후)좌우 회전에 대한 벡터 반환
        Vector3 camRotateDir = _movement.SetAimRotation();

        float moveSpeed;
        if (_status.IsAiming.Value) moveSpeed = _status.WalkSpeed;
        else moveSpeed = _status.RunSpeed;

        Vector3 moveDir = _movement.SetMove(moveSpeed);
        _status.IsMoving.Value = (moveDir != Vector3.zero);

        // TODO: 몸체의 회전기능 구현 후 호출해야 함.

        Vector3 avatarDir;
        if (_status.IsAiming.Value) avatarDir = camRotateDir;
        else avatarDir = moveDir;

        _movement.SetAvatarRotation(avatarDir);

        // SetAnimationParamater
        // Aim 상태일 때만
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
        // 현재 체력을 감소시키지만, 체력이 0이하가 되면 플레이어 사망처리.
        _status.CurrentHP.Value -= value;
        if (_status.CurrentHP.Value <= 0) Dead();
    }
    public void RecoveryHP(int value)
    {
        // 현재 체력을 증가시키지만, MaxHP 이하까지만.
        int hp = _status.CurrentHP.Value + value;

        _status.CurrentHP.Value = Mathf.Clamp(
            hp,
            0,
            _status.MaxHP);
    }
    public void Dead()
    {
        // 주말에 혼자 시도 해 보기.
        // 사망
        Debug.Log("사망");
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


    // 현재수치 / 최대수치
    private void SetHpUIGauge(int currentHp)
    {
        float hp = currentHp / (float)_status.MaxHP; // 형변환 안 하면 값이 0만 나왔음.
        _hpUI.SetImageFillAmount(hp);
    }


}