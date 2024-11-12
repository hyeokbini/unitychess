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
    public string player;
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

    public bool linemoveplate(int xincrease, int yincrease, int kingX = -1, int kingY = -1, bool checkMode = false)
{
    Game sc = control.GetComponent<Game>();
    int x = xposition + xincrease;
    int y = yposition + yincrease;

    // 빈 칸이거나 공격할 수 있는 칸까지 이동 경로를 처리
    while (sc.positiononboard(x, y))
    {
        GameObject targetPiece = sc.getposition(x, y);

        // checkMode가 활성화된 경우
        if (checkMode)
        {
            if (x == kingX && y == kingY)
                return true; // 킹의 위치에 도달할 수 있음을 반환
        }
        else
        {
            if (targetPiece == null)
            {
                // 빈 칸이면 moveplate 생성
                moveplatespawn(x, y);
            }
            else if (targetPiece.GetComponent<userscript>().player != player)
            {
                // 적 기물이 있는 경우 공격 가능한 moveplate 생성
                moveplateattackspawn(x, y);
                break; // 공격 위치 이후로는 이동 불가하므로 루프 종료
            }
            else
            {
                break; // 아군 기물이 있는 경우 이동 중단
            }
        }

        // 다음 칸으로 진행
        x += xincrease;
        y += yincrease;
    }

    // 킹의 위치에 도달할 수 없는 경우 false 반환
    return false;
}




    // 나이트 움직임 구현
    public bool Lmoveplate(int kingX = -1, int kingY = -1, bool checkMode = false)
    {
        return pointmoveplate(xposition + 1, yposition + 2, kingX, kingY, checkMode) ||
               pointmoveplate(xposition - 1, yposition + 2, kingX, kingY, checkMode) ||
               pointmoveplate(xposition + 2, yposition + 1, kingX, kingY, checkMode) ||
               pointmoveplate(xposition + 2, yposition - 1, kingX, kingY, checkMode) ||
               pointmoveplate(xposition - 2, yposition + 1, kingX, kingY, checkMode) ||
               pointmoveplate(xposition - 2, yposition - 1, kingX, kingY, checkMode) ||
               pointmoveplate(xposition + 1, yposition - 2, kingX, kingY, checkMode) ||
               pointmoveplate(xposition - 1, yposition - 2, kingX, kingY, checkMode);
    }


    // 킹의 움직임 구현
    public bool surroundmoveplate(int kingX = -1, int kingY = -1, bool checkMode = false)
    {
        // 각 위치에 대해 pointmoveplate를 호출하고, 체크 모드일 때는 킹의 위치만 확인
        bool isThreatened = pointmoveplate(xposition, yposition + 1, kingX, kingY, checkMode) ||
                            pointmoveplate(xposition, yposition - 1, kingX, kingY, checkMode) ||
                            pointmoveplate(xposition + 1, yposition, kingX, kingY, checkMode) ||
                            pointmoveplate(xposition - 1, yposition, kingX, kingY, checkMode) ||
                            pointmoveplate(xposition + 1, yposition + 1, kingX, kingY, checkMode) ||
                            pointmoveplate(xposition + 1, yposition - 1, kingX, kingY, checkMode) ||
                            pointmoveplate(xposition - 1, yposition + 1, kingX, kingY, checkMode) ||
                            pointmoveplate(xposition - 1, yposition - 1, kingX, kingY, checkMode);

        // 캐슬링 moveplate 생성은 체크 모드일 때 무시
        if (!checkMode)
        {
            castlingmoveplate();
        }

        return isThreatened;
    }


    public bool pointmoveplate(int x, int y, int kingX = -1, int kingY = -1, bool checkMode = false)
    {
        Game sc = control.GetComponent<Game>();

        // 위치가 보드 내에 있는지 확인
        if (sc.positiononboard(x, y))
        {
            GameObject cp = sc.getposition(x, y);

            if (checkMode)
            {
                // 체크 모드: kingX와 kingY와의 일치 여부 확인만 수행
                return x == kingX && y == kingY;
            }
            else
            {
                // 실제 moveplate 생성 모드
                if (IsMoveSafeForKing(x, y))
                {
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
        }
        return false;
    }



public bool pawnmoveplate(int x, int y, int kingX = -1, int kingY = -1, bool checkMode = false)
{
    Game sc = control.GetComponent<Game>();

    if (player == "white")
    {
        if (checkMode)
        {
            // 킹 위치 확인을 위한 대각선 이동 체크
            return (x + 1 == kingX && y == kingY) || (x - 1 == kingX && y == kingY);
        }
        else
        {
            Debug.Log("화이트 폰 클릭, 현재 상태는 firstmove ="+(firstmove ? "true" : "false"));
            // 1칸 전진 (전방에 기물이 없어야 함)
            if (sc.positiononboard(x, y) && sc.getposition(x, y) == null)
            {
                if (IsMoveSafeForKing(x, y))
                {
                    moveplatespawn(x, y);
                }
            }

            // 첫 이동에서 2칸 전진 가능 (경로의 각 칸이 안전해야 함)
            if (firstmove)
            {
                Debug.Log("첫 괄호 통과");
                Debug.Log(sc.positiononboard(x, y + 1) ? "true" : "false");
                Debug.Log(sc.getposition(x, y + 1) == null ? "true" : "false");
                Debug.Log(sc.positiononboard(x, y) ? "true" : "false");
                Debug.Log(sc.getposition(x, y) == null ? "true1" : "false1");
                if (sc.positiononboard(x, y + 1) && sc.getposition(x, y + 1) == null &&
                    sc.positiononboard(x, y) && sc.getposition(x, y) == null)
                {
                    Debug.Log("두번째 괄호 통과");
                    if (IsMoveSafeForKing(x, y + 1))
                    {
                        Debug.Log("세번째 괄호 통과");
                        moveplatespawn(x, y + 1);
                    }
                }
            }

            // 오른쪽 대각선 공격
            if (sc.positiononboard(x + 1, y) && sc.getposition(x + 1, y) != null &&
                sc.getposition(x + 1, y).GetComponent<userscript>().player != player)
            {
                if (IsMoveSafeForKing(x + 1, y))
                {
                    moveplateattackspawn(x + 1, y);
                }
            }

            // 왼쪽 대각선 공격
            if (sc.positiononboard(x - 1, y) && sc.getposition(x - 1, y) != null &&
                sc.getposition(x - 1, y).GetComponent<userscript>().player != player)
            {
                if (IsMoveSafeForKing(x - 1, y))
                {
                    moveplateattackspawn(x - 1, y);
                }
            }
        }
    }
    else if (player == "black")
    {
        if (checkMode)
        {
            // 킹 위치 확인을 위한 대각선 이동 체크
            return (x + 1 == kingX && y == kingY) || (x - 1 == kingX && y == kingY);
        }
        else
        {
            // 1칸 전진 (전방에 기물이 없어야 함)
            if (sc.positiononboard(x, y) && sc.getposition(x, y) == null)
            {
                if (IsMoveSafeForKing(x, y))
                {
                    moveplatespawn(x, y);
                }
            }

            // 첫 이동에서 2칸 전진 가능 (경로의 각 칸이 안전해야 함)
            if (firstmove)
            {
                if (sc.positiononboard(x, y - 1) && sc.getposition(x, y - 1) == null &&
                    sc.positiononboard(x, y) && sc.getposition(x, y) == null)
                {
                    if (IsMoveSafeForKing(x, y - 1))
                    {
                        moveplatespawn(x, y - 1);
                    }
                }
            }

            // 오른쪽 대각선 공격
            if (sc.positiononboard(x + 1, y) && sc.getposition(x + 1, y) != null &&
                sc.getposition(x + 1, y).GetComponent<userscript>().player != player)
            {
                if (IsMoveSafeForKing(x + 1, y))
                {
                    moveplateattackspawn(x + 1, y);
                }
            }

            // 왼쪽 대각선 공격
            if (sc.positiononboard(x - 1, y) && sc.getposition(x - 1, y) != null &&
                sc.getposition(x - 1, y).GetComponent<userscript>().player != player)
            {
                if (IsMoveSafeForKing(x - 1, y))
                {
                    moveplateattackspawn(x - 1, y);
                }
            }
        }
    }

    return false;
}




    public void castlingmoveplate()
    {
        Game sc = control.GetComponent<Game>();

        // 킹사이드 캐슬링
        if (firstmove)
        {
            // 오른쪽 캐슬링: 킹과 룩 사이에 기물이 없어야 하고, 마지막 위치가 안전해야 함
            if (sc.positiononboard(xposition + 1, yposition) && sc.getposition(xposition + 1, yposition) == null &&
                sc.positiononboard(xposition + 2, yposition) && sc.getposition(xposition + 2, yposition) == null)
            {
                // 룩이 첫 번째로 움직인 적이 없어야 함
                GameObject rook = sc.getposition(xposition + 3, yposition);
                if (rook != null && rook.name.Contains("rook") && rook.GetComponent<userscript>().firstmove)
                {
                    // 킹의 마지막 위치가 안전한지 확인
                    if (IsMoveSafeForKing(xposition + 2, yposition))
                    {
                        moveplatespawn(xposition + 2, yposition); // 킹사이드 캐슬링 moveplate 생성
                    }
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
                    // 킹의 마지막 위치가 안전한지 확인
                    if (IsMoveSafeForKing(xposition - 2, yposition))
                    {
                        moveplatespawn(xposition - 2, yposition); // 퀸사이드 캐슬링 moveplate 생성
                    }
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

    private bool IsMoveSafeForKing(int targetX, int targetY)
    {
        Game sc = control.GetComponent<Game>();

        // 현재 위치와 firstmove 상태를 저장
        int originalX = xposition;
        int originalY = yposition;
        bool originalFirstMove = firstmove; // 첫 이동 상태 저장

        // 현재 위치를 비우고, 기물을 임시로 새로운 위치에 배치
        sc.setpositionempty(originalX, originalY);
        xposition = targetX;
        yposition = targetY;
        sc.setposition(gameObject);

        // 첫 이동 상태를 변경하지 않도록 firstmove를 복원
        bool isKingInCheck = sc.IsKingInCheck(player);

        sc.setpositionempty(targetX, targetY);
        // 원래 위치와 firstmove 상태로 복원
        xposition = originalX;
        yposition = originalY;
        firstmove = originalFirstMove; // 첫 이동 상태 복원
        sc.setposition(gameObject);
        Debug.Log("화이트 일리걸 무브 체크 완료, 현재 상태는 firstmove ="+(firstmove ? "true" : "false"));
        return !isKingInCheck;  // 킹이 안전한 경우 true 반환
    }


    public bool checkForKingThreat(int kingX, int kingY)
    {
        switch (name)
        {
            case "blackqueen":
            case "whitequeen":
                return linemoveplate(1, 0, kingX, kingY, true) || linemoveplate(0, 1, kingX, kingY, true) ||
                       linemoveplate(1, 1, kingX, kingY, true) || linemoveplate(-1, 0, kingX, kingY, true) ||
                       linemoveplate(0, -1, kingX, kingY, true) || linemoveplate(-1, -1, kingX, kingY, true) ||
                       linemoveplate(-1, 1, kingX, kingY, true) || linemoveplate(1, -1, kingX, kingY, true);

            case "blackknight":
            case "whiteknight":
                return Lmoveplate(kingX, kingY, true);

            case "blackbishop":
            case "whitebishop":
                return linemoveplate(1, 1, kingX, kingY, true) || linemoveplate(1, -1, kingX, kingY, true) ||
                       linemoveplate(-1, 1, kingX, kingY, true) || linemoveplate(-1, -1, kingX, kingY, true);

            case "blackking":
            case "whiteking":
                return surroundmoveplate(kingX, kingY, true);

            case "blackrook":
            case "whiterook":
                return linemoveplate(1, 0, kingX, kingY, true) || linemoveplate(0, 1, kingX, kingY, true) ||
                       linemoveplate(-1, 0, kingX, kingY, true) || linemoveplate(0, -1, kingX, kingY, true);

            case "blackpawn":
                return pawnmoveplate(xposition, yposition - 1, kingX, kingY, true);

            case "whitepawn":
                return pawnmoveplate(xposition, yposition + 1, kingX, kingY, true);

            default:
                return false;
        }
    }

}

