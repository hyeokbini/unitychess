using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject chesspiece;
    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] blackplayer = new GameObject[16];
    private GameObject[] whiteplayer = new GameObject[16];

    private string currentplayer = "white";

    private bool checkmate = false;
    // Start is called before the first frame update
    void Start()
    {
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

        for (int i = 0; i < blackplayer.Length; i++)
        {
            setposition(blackplayer[i]);
            setposition(whiteplayer[i]);
        }
    }

    public GameObject create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        userscript user = obj.GetComponent<userscript>();
        user.name = name;
        user.setxposition(x);
        user.setyposition(y);
        user.activate();
        return obj;
    }

    public void setposition(GameObject obj)
    {
        userscript user = obj.GetComponent<userscript>();

        positions[user.getxposition(), user.getyposition()] = obj;
    }
}
