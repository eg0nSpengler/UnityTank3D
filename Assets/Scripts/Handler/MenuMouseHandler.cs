using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuMouseHandler : MonoBehaviour
{
    private Transform _sphereTransform;
    private Vector3 _mousePos;
    private void Awake()
    {
        _sphereTransform = GetComponent<Transform>();
        _mousePos = new Vector3(0.0f, 0.0f, 0.0f);

        if (!_sphereTransform)
        {
            Debug.LogError("Failed to get valid transform in " + gameObject.name.ToString() + ", creating one now");
            _sphereTransform = gameObject.AddComponent<Transform>();
        }

    }

    void Start()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 10f));
    }

    // Update is called once per frame
    void Update()
    {
        _sphereTransform.position = _mousePos;
    }

    private void OnMouseEnter()
    {
        
    }


}
