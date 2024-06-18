using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    /// <summary>
    /// 최대 줌 아웃 크기
    /// </summary>
    public float zoomMax = 15;

    /// <summary>
    /// 최소 줌 인 크기
    /// </summary>
    public float zoomMin = 7;

    float zoomTarget = 7.0f;

    public float smooth = 2.0f;
    Vector3 offset;
    Transform target;
    Camera minimapCamera;

    PlayerInputActions uiActions;

    private void Awake()
    {
        minimapCamera = GetComponent<Camera>();
        uiActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        uiActions.UI.Enable();
        uiActions.UI.MinimapZoomIn.performed += OnZoomIn;
        uiActions.UI.MinimapZoomOut.performed += OnZoomOut;
    }

    private void OnDisable()
    {
        uiActions.UI.MinimapZoomIn.performed -= OnZoomIn;
        uiActions.UI.MinimapZoomOut.performed -= OnZoomOut;
        uiActions.UI.Disable();
    }

    private void Start()
    {
        zoomTarget = zoomMin;

        Player player = GameManager.Instance.Player;
        offset = transform.position;    // 플레이어가 0,0,0이어서 별다른 계산 안함
        target = player.transform;
        player.onSpawn += () =>
        {
            transform.position = target.position + offset;
            transform.rotation = Quaternion.Euler(90, target.eulerAngles.y, 0);
        };
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * smooth);
        transform.rotation = Quaternion.Euler(90, target.eulerAngles.y, 0);

        minimapCamera.orthographicSize = Mathf.Lerp(minimapCamera.orthographicSize, zoomTarget, Time.deltaTime);
    }

    private void OnZoomIn(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //minimapCamera.orthographicSize -= 1.0f;
        zoomTarget -= 1.0f;
        zoomTarget = Mathf.Clamp(zoomTarget, zoomMin, zoomMax);
    }

    private void OnZoomOut(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //minimapCamera.orthographicSize += 1.0f;
        zoomTarget += 1.0f;
        zoomTarget = Mathf.Clamp(zoomTarget, zoomMin, zoomMax);
    }
}
