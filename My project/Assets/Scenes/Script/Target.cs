using UnityEngine;

public class Target : MonoBehaviour
{
    private GameConfigSO config;

    // [중요] 스포너가 부르는 함수
    public void Initialize(GameConfigSO gameConfig)
    {
        config = gameConfig;

        // 초기화될 때 확실하게 파괴 시간 설정
        if (config != null) Destroy(gameObject, config.TargetLifeTime);
    }

    // [핵심] 스포너와 상관없이 태어나자마자 무조건 실행되는 함수
    private void Start()
    {
        // 1. 물리 엔진(Rigidbody)이 있다면 강제로 끄기
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;  // "움직이지 마!"
            rb.useGravity = false;  // "중력 받지 마!"
            rb.linearVelocity = Vector3.zero; // "속도 0!"
            rb.angularVelocity = Vector3.zero; // "회전 0!"
        }

        // 2. 만약 Animator가 있다면 끄기 (애니메이션이 범인일 수 있음)
        if (TryGetComponent<Animator>(out Animator anim))
        {
            anim.enabled = false;
        }

        // 3. 트리거 켜기 (총알 맞기용)
        if (TryGetComponent<Collider>(out Collider col))
        {
            col.isTrigger = true;
        }
    }

    // 매 프레임마다 강제로 정지 (혹시라도 누가 밀까봐)
    private void Update()
    {
        // 아무것도 안 함 (위치 고정)
    }

    public void TakeHit()
    {
        GameEvents.OnTargetHit?.Invoke();
        Destroy(gameObject);
    }
}