using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform target;
    public float smoothing;
    public float maxY;

    Vector3 offset;
    AudioSource audioSource;
    
    void Start()
    {
        offset = new Vector3(3, 2, -14);
        audioSource = GetComponent<AudioSource>();
    }
    
    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);    
    }

    public void Stop()
    {
        audioSource.Stop();
        Destroy(this);
    }
}
