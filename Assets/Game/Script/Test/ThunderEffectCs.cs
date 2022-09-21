using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderEffectCs : MonoBehaviour
{
    [SerializeField] Animator _animator;

    PlayerMove _playerMove;
    // Start is called before the first frame update
    void Start()
    {
        _playerMove = GameManager.Instance.PlayerObj.GetComponent<PlayerMove>();

        _animator.SetFloat("x", _playerMove.GetDirection().x);
        _animator.SetFloat("y", _playerMove.GetDirection().y);
        Destroy(gameObject, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
