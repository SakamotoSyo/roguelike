using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPlayerCs : MonoBehaviour
{
    [Header("アニメーション")]
    [SerializeField] Animator _animator;

    [SerializeField] GSSReader _gssReader;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.TurnType == GameManager.TurnManager.Player) 
        {
            _animator.SetBool("Move", true);
        }
    }
}
