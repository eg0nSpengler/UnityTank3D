using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    /// <summary>
    /// thanks 10yr+ old Unity thread
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        // This just makes it to whereas the sprites always face forward towards the player camera
        gameObject.GetComponent<Transform>().LookAt(Camera.main.transform.position, Vector3.up);
    }
}
