using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    // 기물 변수
    public GameObject chesspiece;
    // 기물의 위치를 저장할 2차원 배열
    private GameObject[,] positions = new GameObject[8, 8];
    // 흑백 플레이어들의 기물 배열
    private GameObject[] blackplayer = new GameObject[16];
    private GameObject[] whiteplayer = new GameObject[16];

    // 현재 플레이어 차례(초기값 white)
    private string currentplayer = "white";

    // 체크메이트 상태
    private bool checkmate = false;
    void Start()
    {
        // 백색 기물 생성
        whiteplayer = new GameObject[]
        {
            create("whiterook", 0, 0), create("whiteknight", 1, 0),
            create("whitebishop", 2, 0), create("whitequeen", 3, 0),
            create("whiteking", 4, 0), create("whitebishop", 5, 0),
            create("whiteknight", 6, 0), create("whiterook", 7, 0),
            create("whitepawn", 0, 1), create("whitepawn", 1, 1),
            create("whitepawn", 2, 1), create("whitepawn", 3, 1),
            create("whitepawn", 4, 1), create("whitepawn", 5, 1),
            create("whitepawn", 6, 1), create("whitepawn", 7, 1)
        };

        // 흑색 기물 생성
        blackplayer = new GameObject[]
        {
            create("blackrook", 0, 7), create("blackknight",1,7),
            create("blackbishop",2,7), create("blackqueen",3,7),
            create("blackking",4,7), create("blackbishop",5,7),
            create("blackknight",6,7), create("blackrook",7,7),
            create("blackpawn", 0, 6), create("blackpawn", 1, 6),
            create("blackpawn", 2, 6), create("blackpawn", 3, 6),
            create("blackpawn", 4, 6), create("blackpawn", 5, 6),
            create("blackpawn", 6, 6), create("blackpawn", 7, 6)
        };

        // 생성된 기물들 보드에 배치
        for (int i = 0; i < blackplayer.Length; i++)
        {
            setposition(blackplayer[i]);
            setposition(whiteplayer[i]);
        }
    }

    // 이름, 위치값을 받아 기물을 생성
    public GameObject create(string name, int x, int y)
    {
        // 기물 오브젝트 인스턴스화
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        userscript user = obj.GetComponent<userscript>();

        // 인스턴스화된 기물 이름, 좌표 설정, 활성화
        user.name = name;
        user.setxposition(x);
        user.setyposition(y);
        user.activate();

        // 생성된 기물 오브젝트 반환
        return obj;
    }

    // 기물을 보드 위의 위치 배열에 배치
    public void setposition(GameObject obj)
    {
        userscript user = obj.GetComponent<userscript>();

        // 기물의 현재 좌표를 가져와 해당 위치에 기물 배치
        positions[user.getxposition(), user.getyposition()] = obj;
    }

    // 특정 좌표를 빈칸으로
    public void setpositionempty(int x, int y)
    {
        positions[x,y] = null;
    }

    // 특정 좌표에 있는 기물 반환
    public GameObject getposition(int x, int y)
    {
        return positions[x,y];
    }

    // 보드를 벗어났는지 확인
    public bool positiononboard(int x, int y)
    {
        if(x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1))
        {
            return false;
        }
        return true;
    }
}
