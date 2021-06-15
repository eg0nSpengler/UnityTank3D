using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
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
    }

    // Start is called before the first frame update
    void Start()
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
