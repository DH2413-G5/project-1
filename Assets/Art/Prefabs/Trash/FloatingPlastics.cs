using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlastics : MonoBehaviour
{
    [SerializeField] public float speed = 1.0f;   // A slower speed
    [SerializeField] public float distance = 5.0f; // The distance the object will move
    public float speedDown = 1.0f;
    private Vector3 startPos;
    private Vector3 endPos;
    private bool movingRight = true;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        //endPos = startPos + new Vector3(distance, 0, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position - Vector3.up * speed * Time.deltaTime;
        transform.position = newPosition;

        //if (movingRight)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);

        //    if (transform.position == endPos)
        //    {
        //        movingRight = false;
        //    }
        //}
        //else
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);

        //    if (transform.position == startPos)
        //    {
        //        movingRight = true;
        //    }
        //}

    }
}
