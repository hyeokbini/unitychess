using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

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
        if(attack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        // 오브젝트를 controller에 할당
        controller = GameObject.FindGameObjectWithTag("control");

        if(attack)
        {
            GameObject cp = controller.GetComponent<Game>().getposition(matrixx, matrixy);

            Destroy(cp);
        }

        // 기물이 있던 위치를 빈 공간으로
        controller.GetComponent<Game>().setpositionempty(
            reference.GetComponent<userscript>().getxposition(),
            reference.GetComponent<userscript>().getyposition()
        );

        // 기물의 위치를 새로운 좌표로 업데이트
        reference.GetComponent<userscript>().setxposition(matrixx);
        reference.GetComponent<userscript>().setyposition(matrixy);
        reference.GetComponent<userscript>().SetCoords();

        // 새 위치에 기물 배치
        controller.GetComponent<Game>().setposition(reference);

        // 기존에 표시됐던 moveplate들 삭제(아직 미구현)
        //reference.GetComponent<userscript>().destroymoveplates();
    }

    public void SetCoords(int x,int y)
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
