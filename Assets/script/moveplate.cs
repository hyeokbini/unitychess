using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveplate : MonoBehaviour
{
    public GameObject controller;

    // 기물을 참조하기 위한 변수
    GameObject reference = null;

    // 보드 포지션, 좌표 포지션 X
    int matrixx;
    int matrixy;

    // false : 이동, true : 기물 공격
    public bool attack = false;

    public void Start()
    {
        // 공격 가능한 상태라면 빨간색
        if (attack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        // 오브젝트를 controller에 할당
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().getposition(matrixx, matrixy);
            Destroy(cp);
        }

        // 기물이 있던 위치를 빈 공간으로
        controller.GetComponent<Game>().setpositionempty
        (
            reference.GetComponent<userscript>().getxposition(),
            reference.GetComponent<userscript>().getyposition()
        );

        if (reference.name.Contains("rook"))
        {
            reference.GetComponent<userscript>().firstmove = false;
        }
        // 캐슬링 로직 추가 (킹의 이동일 때만 적용)
        if (reference.name.Contains("king"))
        {
            if (reference.GetComponent<userscript>().firstmove && Mathf.Abs(reference.GetComponent<userscript>().getxposition() - matrixx) == 2)
            {
                Game sc = controller.GetComponent<Game>();

                // 킹사이드 캐슬링 (오른쪽 캐슬링)
                if (matrixx > reference.GetComponent<userscript>().getxposition())
                {
                    GameObject rook = sc.getposition(reference.GetComponent<userscript>().getxposition() + 3, reference.GetComponent<userscript>().getyposition());
                    rook.GetComponent<userscript>().setxposition(reference.GetComponent<userscript>().getxposition() + 1);
                    rook.GetComponent<userscript>().SetCoords();

                    // 기존에 있던 위치를 빈 공간으로 설정
                    controller.GetComponent<Game>().setpositionempty(rook.GetComponent<userscript>().getxposition() + 2, rook.GetComponent<userscript>().getyposition());

                    // 새 위치에 룩 배치
                    controller.GetComponent<Game>().setposition(rook);
                }
                // 퀸사이드 캐슬링 (왼쪽 캐슬링)
                else if (matrixx < reference.GetComponent<userscript>().getxposition())
                {
                    GameObject rook = sc.getposition(reference.GetComponent<userscript>().getxposition() - 4, reference.GetComponent<userscript>().getyposition());
                    rook.GetComponent<userscript>().setxposition(reference.GetComponent<userscript>().getxposition() - 1);
                    rook.GetComponent<userscript>().SetCoords();

                    // 기존에 있던 위치를 빈 공간으로 설정
                    controller.GetComponent<Game>().setpositionempty(rook.GetComponent<userscript>().getxposition() - 3, rook.GetComponent<userscript>().getyposition());

                    // 새 위치에 룩 배치
                    controller.GetComponent<Game>().setposition(rook);
                }
            }
            reference.GetComponent<userscript>().firstmove = false;
        }


        // 기물의 위치를 새로운 좌표로 업데이트
        reference.GetComponent<userscript>().setxposition(matrixx);
        reference.GetComponent<userscript>().setyposition(matrixy);
        reference.GetComponent<userscript>().SetCoords();

        // 새 위치에 기물 배치
        controller.GetComponent<Game>().setposition(reference);

        // 폰의 첫 이동인지 체크하여 firstmove 비활성화
        if (reference.name.Contains("pawn") && reference.GetComponent<userscript>().firstmove)
        {
            reference.GetComponent<userscript>().firstmove = false; // 첫 이동 이후 firstmove 비활성화
        }

        // 턴 전환 로직 추가
        controller.GetComponent<Game>().nextturn();

        // 기존에 표시됐던 moveplate들 삭제
        reference.GetComponent<userscript>().destroymoveplates();
    }

    public void SetCoords(int x, int y)
    {
        matrixx = x;
        matrixy = y;
    }

    public void setreference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject getreference()
    {
        return reference;
    }
}
