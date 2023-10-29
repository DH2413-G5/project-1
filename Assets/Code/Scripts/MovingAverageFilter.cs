using System.Collections.Generic;
using UnityEngine;

public class MovingAverageFilter
{
    private Queue<Vector3> values = new Queue<Vector3>();
    private Vector3 sum = Vector3.zero;
    private int windowSize;

    public MovingAverageFilter(int windowSize)
    {
        this.windowSize = windowSize;
    }

    public Vector3 Process(Vector3 newValue)
    {
        values.Enqueue(newValue);
        sum += newValue;

        if (values.Count > windowSize)
        {
            sum -= values.Dequeue();
        }

        return sum / values.Count;
    }
}