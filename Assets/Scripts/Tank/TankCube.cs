using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCube : MonoBehaviour
{
    [Header("References")]
    public Material mouseOverMaterial;

    private MeshRenderer _meshRen;
    private Material _defaultMaterial;

    private void Awake()
    {
        _meshRen = GetComponentInParent<MeshRenderer>();
        _defaultMaterial = GetComponentInParent<MeshRenderer>().material;

        if (!_meshRen)
        {
            Debug.LogError("Failed to get MeshRenderer on " + gameObject.name.ToString() + ", creating one now...");
            _meshRen = gameObject.AddComponent<MeshRenderer>();
        }

        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        _meshRen.material = mouseOverMaterial;
    }

    private void OnMouseExit()
    {
        _meshRen.material = _defaultMaterial;
    }

}
