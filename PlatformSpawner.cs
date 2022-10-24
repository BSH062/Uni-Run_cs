using UnityEngine;

// 발판을 생성하고 주기적으로 재배치하는 스크립트
public class PlatformSpawner : MonoBehaviour {
    public GameObject platformPrefab; // 생성할 발판의 원본 프리팹
    public int count = 3; // 생성할 발판의 개수

    public float timeBetSpawnMin = 1.25f; // 다음 배치까지의 시간 간격 최솟값
    public float timeBetSpawnMax = 2.25f; // 다음 배치까지의 시간 간격 최댓값
    private float timeBetSpawn; // 다음 배치까지의 시간 간격

    public float yMin = -3.5f; // 배치할 위치의 최소 y값
    public float yMax = 1.5f; // 배치할 위치의 최대 y값
    private float xPos = 20f; // 배치할 위치의 x 값
    private float yPos; // 배치할 위치의 y 값

    private GameObject[] platforms; // 미리 생성한 발판들
    private int currentIndex = 0; // 사용할 현재 순번의 발판

    private Vector2 poolPosition = new Vector2(0, -25); // 초반에 생성된 발판들을 화면 밖에 숨겨둘 위치
    private float lastSpawnTime; // 마지막 배치 시점


    void Start() {
        // 변수들을 초기화하고 사용할 발판들을 미리 생성
        platforms=new GameObject[count];

        for (int i = 0; i < count; i++)//count만큼 루프하면서 발판 생성
        {
            //platformPrefab을 원본으로 새발판을 poolPosition위치에 복제 하고 생성
            //생성된 발판을 platform 배열에 넣어줌
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);
        }
        //마지막 배치시점 초기화
        lastSpawnTime = 0f;
        //다음번 배치시점 초기화 
        timeBetSpawn = 0f;


    }

    void Update() {
        // 순서를 돌아가며 주기적으로 발판을 배치
        if(GameManager.instance.isGameover)//게임 오버 상태는 동작하지 않음
        {
            return;
        }
        //마지막 배치 시점에서 timeBetSpawn 만큼 시간이 흘렀다면
        if(Time.time>=lastSpawnTime+ timeBetSpawn)
        {
            //마지막 배치시점을 현재시간으로 저장
            lastSpawnTime = Time.time;
            //다음 배치시간을 랜덤하게 설정 
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            //배치할 위치의 높이를 랜덤하게설정
            yPos = Random.Range(yMin, yMax);
            //사용할 현재 순번의 발판 오브젝트를 비활성화 하고 즉시 활성화
            //이렇게 하면 Platform컴포넌트의 OnEnable 메서드가 실행됨 
            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(true);
            //현재 순번의 발판을 화면 오른쪽에 재배치 
            platforms[currentIndex].transform.position = new Vector2(xPos, yPos);
            //순번 넘기기 
            currentIndex++;

            //마지막 순번에 도달했다면 순번을 리셋
            if (currentIndex >= count)
            {
                currentIndex = 0;
            }

        }
    }
}