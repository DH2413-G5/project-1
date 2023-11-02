using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HandShadowRepositioner : MonoBehaviour
{

    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    [SerializeField] private float zOffset;
    [SerializeField] private float scaleX = 1;
    [SerializeField] private float scaleY = 1;
    [SerializeField] private float scaleZ = 1;
    [SerializeField] private Transform handAnchor;
    
    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(handAnchor);
    }

    // Update is called once per frame
    void Update()
    {
        var pos = handAnchor.localPosition;
        transform.localPosition = new Vector3(xOffset + pos.x * scaleX, yOffset + pos.y * scaleY, zOffset + pos.z * scaleZ);
    }
}
