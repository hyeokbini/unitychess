using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject chesspiece;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(chesspiece, new Vector3(0,0,-1),Quaternion.identity);
    }

}
