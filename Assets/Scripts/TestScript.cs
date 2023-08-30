using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        string a =  "a1".GetEntity<Deneme>().Alias;

        Debug.Log(a);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
