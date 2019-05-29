using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateServer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetServer.Instance.Start();   
    }
}
