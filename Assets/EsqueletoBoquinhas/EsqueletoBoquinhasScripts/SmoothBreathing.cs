using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothBreathing : MonoBehaviour
{
    //smoothly transitions between scale of 1 and 1.2 and back to 1
    public float speed = 1f;
    public float scale = 1.2f;
    private Vector3 _originalScale;
    private bool _isBreathingIn = true;
    private void Start()
    {
        _originalScale = transform.localScale;
    }
    
    private void Update()
    {
        if (_isBreathingIn)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _originalScale * scale, speed * Time.deltaTime);
            if (Vector3.Distance(transform.localScale, _originalScale * scale) < 0.01f)
            {
                _isBreathingIn = false;
            }
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _originalScale, speed * Time.deltaTime);
            if (Vector3.Distance(transform.localScale, _originalScale) < 0.01f)
            {
                _isBreathingIn = true;
            }
        }
    }
}
