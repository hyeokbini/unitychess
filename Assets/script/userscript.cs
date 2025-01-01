using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userscript : MonoBehaviour
{
    public GameObject control;
    public GameObject movepiece;

    // x,y 좌표 변수, 초기값 -1
    private int xposition = -1;
    private int yposition = -1;
    private string player;
    // 폰 두칸 움직인 구현을 위한 변수
    public bool firstmove = true;

    // 스프라이트 이미지 변수
    public Sprite whiteking, whitequeen, whiteknight, whitebishop, whiterook, whitepawn;
    public Sprite blackking, blackqueen, blackknight, blackbishop, blackrook, blackpawn;

    public void activate()
    {
        // 오브젝트를 control에 할당
        control = GameObject.FindGameObjectWithTag("GameController");
        // 기물 위치 설정
        SetCoords();
        // 이름에 따른 스프라이트 이미지 설정
        switch (name)
        {
            case "blackqueen": GetComponent<SpriteRenderer>().sprite = blackqueen; player = "black"; break;
            case "blackking": GetComponent<SpriteRenderer>().sprite = blackking; player = "black"; firstmove = true; break;
            case "blackrook": GetComponent<SpriteRenderer>().sprite = blackrook; player = "black"; firstmove = true; break;
            case "blackknight": GetComponent<SpriteRenderer>().sprite = blackknight; player = "black"; break;
            case "blackbishop": GetComponent<SpriteRenderer>().sprite = blackbishop; player = "black"; break;
            case "blackpawn": GetComponent<SpriteRenderer>().sprite = blackpawn; player = "black"; firstmove = true; break;

            case "whitequeen": GetComponent<SpriteRenderer>().sprite = whitequeen; player = "white"; break;
            case "whiteking": GetComponent<SpriteRenderer>().sprite = whiteking; player = "white"; firstmove = true; break;
            case "whiterook": GetComponent<SpriteRenderer>().sprite = whiterook; player = "white"; firstmove = true; break;
            case "whiteknight": GetComponent<SpriteRenderer>().sprite = whiteknight; player = "white"; break;
            case "whitebishop": GetComponent<SpriteRenderer>().sprite = whitebishop; player = "white"; break;
            case "whitepawn": GetComponent<SpriteRenderer>().sprite = whitepawn; player = "white"; firstmove = true; break;
        }
    }

    // 유니티상의 좌표를 보드 좌표로 설정
    public void SetCoords()
    {
        float x = xposition;
        float y = yposition;

        // x,y 좌표를 변환해 체스판에서의 위치 재계산

        x -= 3.5f;
        y -= 3.5f;

        // 위치 재설정
        transform.position = new Vector3(x, y, -1.0f);
    }

    // x,y 좌표 리턴
    public int getxposition()
    {
        return xposition;
    }
    public int getyposition()
    {
        return yposition;
    }

    // x,y 좌표 설정
    public void setxposition(int x)
    {
        xposition = x;
    }
    public void setyposition(int y)
    {
        yposition = y;
    }

    private void OnMouseUp()
    {
        // 게임오버 되지 않았을 때 현재 플레이어가 수를 둘 수 있게 만들기
        if (!control.GetComponent<Game>().gameover() && control.GetComponent<Game>().getcurrentplayer() == player)
        {

            // 기존 이동가능 moveplate 삭제
            destroymoveplates();
            // 새로운 moveplate 생성
            initiatemoveplates();
        }

    }

    // 존재했던 moveplate 삭제
    public void destroymoveplates()
    {
        // moveplate를 찾아와 배열로 반환
        GameObject[] moveplates = GameObject.FindGameObjectsWithTag("moveplate");
        // 모두 삭제
        for (int i = 0; i < moveplates.Length; i++)
        {
            Destroy(moveplates[i]);
        }
    }

    // 기물 움직이기 로직
    public void initiatemoveplates()
    {
        switch (name)
        {
            case "blackqueen":
            case "whitequeen":
                linemoveplate(1, 0);
                linemoveplate(0, 1);
                linemoveplate(1, 1);
                linemoveplate(-1, 0);
                linemoveplate(0, -1);
                linemoveplate(-1, -1);
                linemoveplate(-1, 1);
                linemoveplate(1, -1);
                break;
            case "blackknight":
            case "whiteknight":
                Lmoveplate();
                break;
            case "blackbishop":
            case "whitebishop":
                linemoveplate(1, 1);
                linemoveplate(1, -1);
                linemoveplate(-1, 1);
                linemoveplate(-1, -1);
                break;
            case "blackking":
            case "whiteking":
                surroundmoveplate();
                break;
            case "blackrook":
            case "whiterook":
                linemoveplate(1, 0);
                linemoveplate(0, 1);
                linemoveplate(-1, 0);
                linemoveplate(0, -1);
                break;
            case "blackpawn":
                pawnmoveplate(xposition, yposition - 1);
                break;
            case "whitepawn":
                pawnmoveplate(xposition, yposition + 1);
                break;
        }
    }

    // 일직선형으로 움직이는 기물들의 로직 처리
    public void linemoveplate(int xincrease, int yincrease)
    {
        Game sc = control.GetComponent<Game>();

        int x = xposition + xincrease;
        int y = yposition + yincrease;

        while (sc.positiononboard(x, y) && sc.getposition(x, y) == null)
        {
            moveplatespawn(x, y);
            x += xincrease;
            y += yincrease;
        }
        // 공격 가능 기물이 경로에 있으면 빨간색 위치표시
        if (sc.positiononboard(x, y) && sc.getposition(x, y).GetComponent<userscript>().player != player)
        {
            moveplateattackspawn(x, y);
        }
    }

    // 나이트의 움직임 구현
    public void Lmoveplate()
    {
        pointmoveplate(xposition + 1, yposition + 2);
        pointmoveplate(xposition - 1, yposition + 2);
        pointmoveplate(xposition + 2, yposition + 1);
        pointmoveplate(xposition + 2, yposition - 1);
        pointmoveplate(xposition - 2, yposition + 1);
        pointmoveplate(xposition - 2, yposition - 1);
        pointmoveplate(xposition + 1, yposition - 2);
        pointmoveplate(xposition - 1, yposition - 2);
    }

    // 킹의 움직임 구현
    public void surroundmoveplate()
    {
        pointmoveplate(xposition, yposition + 1);
        pointmoveplate(xposition, yposition - 1);
        pointmoveplate(xposition + 1, yposition);
        pointmoveplate(xposition - 1, yposition);
        pointmoveplate(xposition + 1, yposition + 1);
        pointmoveplate(xposition + 1, yposition - 1);
        pointmoveplate(xposition - 1, yposition + 1);
        pointmoveplate(xposition - 1, yposition - 1);

        castlingmoveplate();
    }

    // 일직선형이 아닌 특정 점으로 이동하는 로직 구현
    public void pointmoveplate(int x, int y)
    {
        Game sc = control.GetComponent<Game>();
        if (sc.positiononboard(x, y))
        {
            GameObject cp = sc.getposition(x, y);
            
            if (cp == null)
            {
                moveplatespawn(x, y);
            }
            else if (cp.GetComponent<userscript>().player != player)
            {
                moveplateattackspawn(x, y);
            }
        }
    }

    // 폰의 이동 구현
    public void pawnmoveplate(int x, int y)
    {
        Game sc = control.GetComponent<Game>();

        // 흰색 폰,검정 폰 로직 분리.
        if (player == "white")
        {
            // 1칸 전진 (전방에 기물이 없어야 함)
            if (sc.positiononboard(x, y) && sc.getposition(x, y) == null)
            {
                moveplatespawn(x, y); // 폰의 1칸 직진 이동
            }

            // 첫 번째 이동인 경우 2칸 이동 가능 (전방 두 칸 모두 비어있어야 함)
            if (firstmove)
            {
                // 두 칸 전진 가능 여부 확인
                if (sc.positiononboard(x, y + 1) && sc.getposition(x, y + 1) == null &&
                    sc.positiononboard(x, y) && sc.getposition(x, y) == null)
                {
                    moveplatespawn(x, y + 1); // 2칸 전진
                }
            }

            // 오른쪽 대각선 공격
            if (sc.positiononboard(x + 1, y) && sc.getposition(x + 1, y) != null &&
                sc.getposition(x + 1, y).GetComponent<userscript>().player != player)
            {
                moveplateattackspawn(x + 1, y);
            }

            // 왼쪽 대각선 공격
            if (sc.positiononboard(x - 1, y) && sc.getposition(x - 1, y) != null &&
                sc.getposition(x - 1, y).GetComponent<userscript>().player != player)
            {
                moveplateattackspawn(x - 1, y);
            }
        }
        else if (player == "black")
        {
            // 1칸 전진 (전방에 기물이 없어야 함)
            if (sc.positiononboard(x, y) && sc.getposition(x, y) == null)
            {
                moveplatespawn(x, y); // 폰의 1칸 직진 이동
            }

            // 첫 번째 이동인 경우 2칸 이동 가능 (전방 두 칸 모두 비어있어야 함)
            if (firstmove)
            {
                // 두 칸 전진 가능 여부 확인
                if (sc.positiononboard(x, y - 1) && sc.getposition(x, y - 1) == null &&
                    sc.positiononboard(x, y) && sc.getposition(x, y) == null)
                {
                    moveplatespawn(x, y - 1); // 2칸 전진
                }
            }

            // 오른쪽 대각선 공격
            if (sc.positiononboard(x + 1, y) && sc.getposition(x + 1, y) != null &&
                sc.getposition(x + 1, y).GetComponent<userscript>().player != player)
            {
                moveplateattackspawn(x + 1, y);
            }

            // 왼쪽 대각선 공격
            if (sc.positiononboard(x - 1, y) && sc.getposition(x - 1, y) != null &&
                sc.getposition(x - 1, y).GetComponent<userscript>().player != player)
            {
                moveplateattackspawn(x - 1, y);
            }
        }
    }

    public void castlingmoveplate()
{
    Game sc = control.GetComponent<Game>();

    // 킹사이드 캐슬링
    if (firstmove)
    {
        // 오른쪽 캐슬링: 킹과 룩 사이에 기물이 없어야 함
        if (sc.positiononboard(xposition + 1, yposition) && sc.getposition(xposition + 1, yposition) == null &&
            sc.positiononboard(xposition + 2, yposition) && sc.getposition(xposition + 2, yposition) == null)
        {
            // 룩이 첫 번째로 움직인 적이 없어야 함
            GameObject rook = sc.getposition(xposition + 3, yposition);
            if (rook != null && rook.name.Contains("rook") && rook.GetComponent<userscript>().firstmove)
            {
                // 룩 위치에 moveplate 생성
                moveplatespawn(xposition + 2, yposition);
            }
        }

        // 퀸사이드 캐슬링
        if (sc.positiononboard(xposition - 1, yposition) && sc.getposition(xposition - 1, yposition) == null &&
            sc.positiononboard(xposition - 2, yposition) && sc.getposition(xposition - 2, yposition) == null &&
            sc.positiononboard(xposition - 3, yposition) && sc.getposition(xposition - 3, yposition) == null)
        {
            // 룩이 첫 번째로 움직인 적이 없어야 함
            GameObject rook = sc.getposition(xposition - 4, yposition);
            if (rook != null && rook.name.Contains("rook") && rook.GetComponent<userscript>().firstmove)
            {
                // 룩 위치에 moveplate 생성
                moveplatespawn(xposition - 2, yposition);
            }
        }
    }
}

    public void moveplatespawn(int inputx, int inputy)
    {
        float x = inputx;
        float y = inputy;

        // x,y 좌표를 변환해 체스판에서의 위치 재계산
        x -= 3.5f;
        y -= 3.5f;

        // 위치 재설정
        GameObject mp = Instantiate(movepiece, new Vector3(x, y, -3.0f), Quaternion.identity);

        moveplate mpscript = mp.GetComponent<moveplate>();
        mpscript.setreference(gameObject);
        mpscript.SetCoords(inputx, inputy);
    }

    public void moveplateattackspawn(int inputx, int inputy)
    {
        float x = inputx;
        float y = inputy;

        // x,y 좌표를 변환해 체스판에서의 위치 재계산
        x -= 3.5f;
        y -= 3.5f;

        // 위치 재설정
        GameObject mp = Instantiate(movepiece, new Vector3(x, y, -3.0f), Quaternion.identity);

        moveplate mpscript = mp.GetComponent<moveplate>();
        mpscript.attack = true;
        mpscript.setreference(gameObject);
        mpscript.SetCoords(inputx, inputy);
    }
}
