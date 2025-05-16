using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField][Range(0, 100)] private float _attackRange;
    [SerializeField] private int _shootDamage;
    [SerializeField] private float _shootDelay;
    [SerializeField] private AudioClip _shootSFX;

    private CinemachineImpulseSource _impulse;
    private Camera _camera;
    private bool _canShoot { get => _currentCount <= 0; } // 0이면 true반환
    private float _currentCount;

    private void Awake() => Init();
    private void Update() => HandleCanShoot();
    private void Init()
    {
        _camera = Camera.main;
        _impulse = GetComponent<CinemachineImpulseSource>();
    }

    public bool Shoot()
    {
        if (!_canShoot) return false;

        PlayShootSound();
        PlayCameraEffect();

        PlayShootEffect();

        _currentCount = _shootDelay;

        // TODO : Ray 발사 -> 반환받은 대상에게 데미지 부여하는 기능. 몬스터 구현 시 같이 구현
        IDamagable target = RayShoot();
        if(target == null) return true;

        target.TakeDamage(_shootDamage);

        // Debug.Log($"총에 맞음 : {target.name}");
        // ----
        return true;
    }
    private void HandleCanShoot()
    {
        if (_canShoot) return;
        _currentCount -= Time.deltaTime;
    }
    private IDamagable RayShoot()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, _attackRange, _targetLayer))
        {
            // =====
            
            // TODO : 몬스터를 어떻게 구현하는가에 따라 다르다.
            // IDamagable
            return hit.transform.GetComponent<IDamagable>(); // 이거 우회하라고? GetComponent가 지속적으로 호출되는걸 방지하는 방법!
            // =====


        }
        return null;
    }

    private void PlayShootSound()
    {
        SFXController sfx = GameManager.Instance.Audio.GetSFX();
        sfx.Play(_shootSFX);
    }
    private void PlayCameraEffect()
    {
        _impulse.GenerateImpulse();
    }
    private void PlayShootEffect()
    {
        // TODO : 총구 효과. 파티클로 구현
    }







}
