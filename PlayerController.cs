using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour {
   public AudioClip deathClip; // 사망시 재생할 오디오 클립
   public float jumpForce = 700f; // 점프 힘

   private int jumpCount = 0; // 누적 점프 횟수
   private bool isGrounded = false; // 바닥에 닿았는지 나타냄
   private bool isDead = false; // 사망 상태

   private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
   private Animator animator; // 사용할 애니메이터 컴포넌트
   private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트

   private void Start() 
    {
       // 초기화
       playerRigidbody=GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
   }

   private void Update() {
        // 사용자 입력을 감지하고 점프하는 처리
        if (isDead) //사망상태면 Update를 실행하지않음
        {
            return;
        }
       if(Input.GetMouseButtonDown(0)&&jumpCount<2) //마우스 왼쪽 버튼을 누르는 순간 그리고  점프카운트가 2이하일때 
        {
            jumpCount++; //점프 카운트를 1올림
            playerRigidbody.velocity = Vector2.zero; //점프직전 속도를 0,0으로 
            playerRigidbody.AddForce(new Vector2(0, jumpForce)); //점프포스 만큼 y축으로 힘을줌 
            playerAudio.Play(); //오디오 실행 

        }
       else if(Input.GetMouseButtonUp(0)&&playerRigidbody.position.y>0) //마우스 왼쪽버튼을 때는 순간 그리고 플레이어 리지드 바디 y축 포지션이 0이상일대 (공중에 뜬상태)
        {
            playerRigidbody.velocity = playerRigidbody.velocity * 0.5f; //현재의 속도를 절반으로 변경 최고점에서의 속도는 0에 가까워 절반으로 줄여도 효과가 미미함
            //그러나 높이가 낮은 상태에서 절반으로 줄이면 그만큼 낮게 점프함 , 사용자의 입력에 따라 점프 높이를 조절할수있음 
        }
        animator.SetBool("Grounded", isGrounded); //isGrounded 그라운드는 땅에 닿았는지 상태를 확인함 그후 애니메이터 상태를 바꿔줌
    }

   private void Die() {
        // 사망 처리
        animator.SetTrigger("Die");
        playerAudio.clip = deathClip; //사망 오디오를 들고옴 
        playerAudio.Play();
        playerRigidbody.velocity = Vector2.zero;
        isDead = true;
        GameManager.instance.OnPlayerDead(); 
   }

   private void OnTriggerEnter2D(Collider2D other) { //2D는 2D라고 뒤에 이름 따로 붙음 
       // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
       if(other.tag =="Dead"&&!isDead) //장애물의 태그가 Dead 이거나 플레이어가 사망상태가 아니라면 
        {
            Die(); 
        }
   }

   private void OnCollisionEnter2D(Collision2D collision) {//땅은 트리거로 설정하면 플레이어가 못밟아서 아래로떨어짐 
        // 바닥에 닿았음을 감지하는 처리
        if (collision.contacts[0].normal.y > 0.7f)
        {//collision.contacts[0] 은 두 물체 사이의 첫 번째 충돌 지점의 정보를 가져옴 y가 0.7인경우 대략 45도 각도를 의미함
            //조건은 0.7보다 커야 하기때문에 충돌 표면이 위쪽이고 , 경사면이 너무 급하지 않은지 검사 하는것 
            isGrounded = true;
            jumpCount = 0;
        }
   }

   private void OnCollisionExit2D(Collision2D collision) {
       // 바닥에서 벗어났음을 감지하는 처리
       isGrounded=false;
   }
}