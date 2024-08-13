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
            case "blackqueen": this.GetComponent<SpriteRenderer>().sprite = blackqueen; break;
            case "blackking": this.GetComponent<SpriteRenderer>().sprite = blackking; break;
            case "blackrook": this.GetComponent<SpriteRenderer>().sprite = blackrook; break;
            case "blackknight": this.GetComponent<SpriteRenderer>().sprite = blackknight; break;
            case "blackbishop": this.GetComponent<SpriteRenderer>().sprite = blackbishop; break;
            case "blackpawn": this.GetComponent<SpriteRenderer>().sprite = blackpawn; break;

            case "whitequeen": this.GetComponent<SpriteRenderer>().sprite = whitequeen; break;
            case "whiteking": this.GetComponent<SpriteRenderer>().sprite = whiteking; break;
            case "whiterook": this.GetComponent<SpriteRenderer>().sprite = whiterook; break;
            case "whiteknight": this.GetComponent<SpriteRenderer>().sprite = whiteknight; break;
            case "whitebishop": this.GetComponent<SpriteRenderer>().sprite = whitebishop; break;
            case "whitepawn": this.GetComponent<SpriteRenderer>().sprite = whitepawn; break;
        }
    }

    // 유니티상의 좌표를 보드 좌표로 설정
    public void SetCoords()
    {
        float x = xposition;
        float y = yposition;

        // x,y 좌표를 변환해 체스판에서의 위치 재계산
        x = xposition - 3.5f;
        y = 3.5f - yposition;

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
}
