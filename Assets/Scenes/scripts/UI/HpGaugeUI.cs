using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HpGaugeUI : MonoBehaviour
{
    // 1. LookAt(mainCamera) => 카메라 위치에 따라서 HP게이지가 기울어짐
    // 2. 현재 카메라의 방향으로 회전. 카메라의 방향 벡터를 적용
    // 3. 카메라의 반대 방향 벡터 적용 -> Text의 경우 뒤집혔음.

    // Test 3
    // private void SetUIRotate3()
    // {
    //     transform.forward = -Camera.main.transform.forward;
    // }

    [SerializeField] private Image _image;
    private Transform _cameraTransform;

    private void Awake() => Init();
    private void LateUpdate() => SetUIForwardVector(_cameraTransform.forward);
    private void Init()
    {
        // _image = GetComponentInChildren<Image>();
        _cameraTransform = Camera.main.transform;
    }

    // 현재 수치 / 최대 수치
    public void SetImageFillAmount(float value)
    {
        _image.fillAmount = value;
    }
    private void SetUIForwardVector(Vector3 target)
    {
        // 1. transform.rotation = Camera.main.transform.rotation;
        // transform.forward = Camera.main.transform.forward;
        transform.forward = target;
    }
    






}
