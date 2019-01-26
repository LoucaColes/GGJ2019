using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    // Start is called before the first frame update

    private Light thisLight;

    [SerializeField] private float lightBurst = 0.5f;
    [SerializeField] private int frameBetweenBurst = 3;
    [SerializeField] private int frameDelay = 15;           //Length of burst from frameBetweenBurst before reset

    private int frameCount = 0;    
    private bool burstCheck = true;
    

    void Awake()
    {
        thisLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;

        if (frameCount == frameBetweenBurst && burstCheck)
        {
            thisLight.intensity += lightBurst;
            burstCheck = false;
        }       

        if (frameCount == frameDelay)
        {
            thisLight.intensity -= lightBurst;
            burstCheck = true;
            frameCount = 0;
        }
    }
}
