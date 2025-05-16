using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HpGaugeUI : MonoBehaviour
{
    // 1. LookAt(mainCamera) => ī�޶� ��ġ�� ���� HP�������� ������
    // 2. ���� ī�޶��� �������� ȸ��. ī�޶��� ���� ���͸� ����
    // 3. ī�޶��� �ݴ� ���� ���� ���� -> Text�� ��� ��������.

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

    // ���� ��ġ / �ִ� ��ġ
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
