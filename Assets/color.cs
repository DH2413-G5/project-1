using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class color : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeRedEvent()
    {
        GetComponent<MeshRenderer>().material.color = Color.red; // 更改为红色
    }
    
    public void ChangeGreenEvent()
    {
        GetComponent<MeshRenderer>().material.color = Color.green; // 更改为红色
    }
}
