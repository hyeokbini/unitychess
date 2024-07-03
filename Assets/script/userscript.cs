using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userscript : MonoBehaviour
{
    public GameObject control;
    public GameObject movepiece;

    private int xposition = -1;
    private int yposition = -1;

    private string player;

    public Sprite whiteking, whitequeen, whiteknight, whitebishop, whiterook, whitepawn;
    public Sprite blackking, blackqueen, blackknight, blackbishop, blackrook, blackpawn;

    public void activate()
    {
        control = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

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

    public void SetCoords()
    {
        float x = xposition;
        float y = yposition;

        x = xposition - 3.5f;
        y = 3.5f - yposition;

        this.transform.position = new Vector3(x, y, -1.0f);
    }


    public int getxposition()
    {
        return xposition;
    }
    public int getyposition()
    {
        return yposition;
    }

    public void setxposition(int x)
    {
        xposition = x;
    }
    public void setyposition(int y)
    {
        yposition = y;
    }
}
