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
        switch (this.name)
        {
            case "blackqueen": this.GetComponent<SpriteRenderer>().sprite = blackqueen; player = "black"; break;
            case "blackking": this.GetComponent<SpriteRenderer>().sprite = blackking; player = "black"; break;
            case "blackrook": this.GetComponent<SpriteRenderer>().sprite = blackrook; player = "black"; break;
            case "blackknight": this.GetComponent<SpriteRenderer>().sprite = blackknight; player = "black"; break;
            case "blackbishop": this.GetComponent<SpriteRenderer>().sprite = blackbishop; player = "black"; break;
            case "blackpawn": this.GetComponent<SpriteRenderer>().sprite = blackpawn; player = "black"; break;

            case "whitequeen": this.GetComponent<SpriteRenderer>().sprite = whitequeen; player = "white"; break;
            case "whiteking": this.GetComponent<SpriteRenderer>().sprite = whiteking; player = "white"; break;
            case "whiterook": this.GetComponent<SpriteRenderer>().sprite = whiterook; player = "white"; break;
            case "whiteknight": this.GetComponent<SpriteRenderer>().sprite = whiteknight; player = "white"; break;
            case "whitebishop": this.GetComponent<SpriteRenderer>().sprite = whitebishop; player = "white"; break;
            case "whitepawn": this.GetComponent<SpriteRenderer>().sprite = whitepawn; player = "white"; break;
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
        this.transform.position = new Vector3(x, y, -1.0f);
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
        switch (this.name)
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

        if (sc.positiononboard(x, y))
        {
            if (sc.getposition(x, y) == null)
            {
                moveplatespawn(x, y);
            }

            if (sc.positiononboard(x + 1, y) && sc.getposition(x + 1, y) != null &&
                sc.getposition(x + 1, y).GetComponent<userscript>().player != player)
            {
                moveplateattackspawn(x + 1, y);
            }

            if (sc.positiononboard(x - 1, y) && sc.getposition(x - 1, y) != null &&
                sc.getposition(x - 1, y).GetComponent<userscript>().player != player)
            {
                moveplateattackspawn(x - 1, y);
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
