using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for handling the Demon Monster Animation states
/// </summary>
public class DemonAnimationHandler : MonoBehaviour
{

    private Sprite _currentSprite;
    private SpriteRenderer _spriteRen;
    private List<Sprite> _spriteList;
    private NavComponent _navComponent;

    private void Awake()
    {
        _currentSprite = GetComponent<SpriteRenderer>().sprite;
        _spriteRen = GetComponent<SpriteRenderer>();
        _spriteList = new List<Sprite>();
        _navComponent = GetComponent<NavComponent>();


        if (!_navComponent)
        {
            Debug.LogError("Failed to get Nav Component on" + gameObject.name.ToString() + " creating one now");
            _navComponent = gameObject.AddComponent<NavComponent>();
        }

        if (!_spriteRen)
        {
            Debug.LogError("Failed to get Sprite Renderer on " + gameObject.name.ToString() + " creating one now");
            _spriteRen = gameObject.AddComponent<SpriteRenderer>();
        }

        _navComponent.OnDestinationBegin += PlayWalkingAnim;
        _navComponent.OnDestinationSuccess += StopAnim;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayWalkingAnim()
    {
        StartCoroutine(WalkingAnim());
    }


    IEnumerator WalkingAnim()
    {
        _spriteRen.flipX = true;
        yield return new WaitForSeconds(1f);
        _spriteRen.flipX = false;
    }

    void StopAnim()
    {
        
    }

}
