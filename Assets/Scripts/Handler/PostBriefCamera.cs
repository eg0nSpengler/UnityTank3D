using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostBriefCamera : MonoBehaviour
{
    [Header("References")]
    public Transform _transPoint1;
    public Transform _transPoint2;

    private Camera _camera;
    private Vector3 _smoothVel;
    
    private float _smoothTime;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        _smoothTime = 2.0f;
        _smoothVel = Vector3.zero;

        if (!_camera)
        {
            Debug.LogError("Failed to get Camera on " + gameObject.name.ToString() + ", creating one now");
            _camera = gameObject.AddComponent<Camera>();
        }

        if (!_transPoint1)
        {
            Debug.LogError("Failed to get Transform on " + gameObject.name.ToString() + ", using parent Transform as fallback");
            _transPoint1 = GetComponent<Transform>();
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _transPoint1.position, Time.deltaTime * 2.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name.ToString());
    }
}
