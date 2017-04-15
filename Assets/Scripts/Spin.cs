using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour
{
    public float Speed = 10f;


    void Update()
    {
        transform.Rotate(0,Speed * Time.deltaTime,0);
    }
}
