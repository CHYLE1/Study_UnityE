using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameConfigSO config;
    [SerializeField] private Camera playerCamera;

    private CharacterController characterController;
    private float xRotation = 0f;
    private float verticalVelocity;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // 게임 오버시 마우스 보이기
        if (GameManager.Instance == null || !GameManager.Instance.IsGameActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        // ESC로 마우스 보이기/숨기기
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (Cursor.lockState == CursorLockMode.Locked) { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
            else { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
        }

        HandleMovement();
        HandleLook();
        HandleShooting();
    }

    private void HandleMovement()
    {
        float x = 0; float z = 0;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) z += 1;
            if (Keyboard.current.sKey.isPressed) z -= 1;
            if (Keyboard.current.dKey.isPressed) x += 1;
            if (Keyboard.current.aKey.isPressed) x -= 1;
        }

        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move * config.MoveSpeed * Time.deltaTime);

        if (characterController.isGrounded && verticalVelocity < 0) verticalVelocity = -2f;

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && characterController.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(config.JumpForce * -2f * Physics.gravity.y);
        }

        verticalVelocity += Physics.gravity.y * Time.deltaTime;
        characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    private void HandleLook()
    {
        Vector2 mouseDelta = Vector2.zero;
        if (Mouse.current != null) mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * config.LookSensitivity;
        float mouseY = mouseDelta.y * config.LookSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleShooting()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            // [디버깅] 화면에 초록색 선을 1초간 그려서 어디 쏘는지 보여줌 (Scene 탭에서 보임)
            Debug.DrawRay(ray.origin, ray.direction * config.ShootRange, Color.green, 1f);

            // [중요 수정] QueryTriggerInteraction.Collide 추가!
            // 이걸 넣어야 'Trigger' 상태인 타겟을 맞출 수 있습니다.
            if (Physics.Raycast(ray, out RaycastHit hit, config.ShootRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
            {
                // Target 스크립트 찾기
                Target targetScript = hit.collider.GetComponent<Target>();
                if (targetScript != null)
                {
                    targetScript.TakeHit();
                    Debug.Log("타겟 명중!"); // 콘솔창 확인용
                }
            }
        }
    }
}