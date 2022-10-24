using UnityEngine;

// 왼쪽 끝으로 이동한 배경을 오른쪽 끝으로 재배치하는 스크립트
public class BackgroundLoop : MonoBehaviour {
    private float width; // 배경의 가로 길이

    private void Awake() {
        // 가로 길이를 측정하는 처리
        BoxCollider2D backgroundCollider=GetComponent<BoxCollider2D>();
        width = backgroundCollider.size.x;
    }

    private void Update() {
        // 현재 위치가 원점에서 왼쪽으로 width 이상 이동했을때 위치를 리셋
        if(transform.position.x<=-width)
        {
            Reposition();
        }
    }

    // 위치를 리셋하는 메서드
    private void Reposition() { //얼만큼 오른쪽으로 밀어낼것인가?
        Vector2 offset = new Vector2(width * 2f, 0); //-width 만큼 이동했기때문에  (-width  0  width ) 으로 이동한다 생각하면 width X 2 해주면 다음칸으로 보낼수 있음
        transform.position = (Vector2)transform.position + offset; //현재 위치에서 2배 곱한값을 넣어줌
    }

}