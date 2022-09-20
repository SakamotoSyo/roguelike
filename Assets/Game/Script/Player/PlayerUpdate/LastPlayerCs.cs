using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPlayerCs : MonoBehaviour
{
    [Header("�A�j���[�V����")]
    [SerializeField] Animator _animator;
    [Header("GSSReader�̃X�N���v�g")]
    [SerializeField] GSSReader _gssReader;

    [Tooltip("GameManager�̃C���X�^���X")]
    GameManager _gameManager;
    [Tooltip("DgGenerator�̃C���X�^���X")]
    DgGenerator _dgGenerator;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        _dgGenerator = DgGenerator.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.TurnType == GameManager.TurnManager.Story) 
        {
            _animator.SetBool("Move", true);
        }
    }

    /// <summary>
    /// �{�X����X�^�[�g����
    /// </summary>
    public void BossStart() 
    {
        _gameManager.SetPlayerPosition((int)transform.position.x, (int)transform.position.y * -1);
        _gameManager.PlayerObj.transform.position = transform.position;
        _gameManager.PlayerObj.SetActive(true);
        gameObject.SetActive(false);
       
        _gameManager.TurnType = GameManager.TurnManager.Player;
    }
}
