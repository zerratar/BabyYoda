using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    void FixedUpdate()
    {
        this.transform.LookAt(Camera.main.transform);
    }
    void LateUpdate()
    {
        this.transform.LookAt(Camera.main.transform);
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(Camera.main.transform);
    }
}
