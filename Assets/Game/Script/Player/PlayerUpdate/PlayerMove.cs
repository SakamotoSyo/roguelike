using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class PlayerMove : MonoBehaviour,IDirection
{
    [Header("PlayerMoveMent")]
    [SerializeField] PlayerMovement _playerMovement;

    [Header("���������WaitTime")]
    [SerializeField] float _afterWalkActionTime;

    [Header("���������WaitTime")]
    [SerializeField] float _afterRunActionTime;

    [Header("�����X�s�[�h")]
    [SerializeField] float _walkSpeed;

    [Header("����X�s�[�h")]
    [SerializeField] float _runSpeed;

    [Header("Animator�R���|�[�l���g")]
    [SerializeField] Animator _anim;

    [Header("Playable�R���|�[�l���g")]
    [SerializeField] PlayableDirector _moveArrowDirector;

    [Header("�ړ��������i�r�Q�[�V��������Obj")]
    [SerializeField] GameObject _moveArrowObj;

    [Header("�ʘH�ɓ��������Ɏ�����Ƃ炷��")]
    [SerializeField] GameObject _playerLight;

    [Header("PlayerStatus��Script")]
    [SerializeField] PlayerStatus _playerStatus;

    [Tooltip("GameManeger�̃C���X�^���X")]
    private GameManager _gameManager;

    [Tooltip("MInimap��Update���邽�߂�Action")]
    public Action MiniMapUpdate;

    [Tooltip("���쒆���ǂ���")]
    private bool _isMoving;

    [Tooltip("�v���C���[������������")]
    private Vector2 _playerDirection;

    [Tooltip("�v���C���[�����Ɉړ�����ꏊ")]
    private Vector3 _nextPosition;

    [Tooltip("���͒l��ۑ����Ă����ϐ�")]
    float x, y;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    void Update()
    {
        _anim.SetFloat("x", _playerDirection.x);
        _anim.SetFloat("y", _playerDirection.y);
    }

    public void InputKey()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
    }


    /// <summary>
    /// �ړ��̓��͏���
    /// </summary>
    public async void Move()
    {
        //_countTime = 0;
        //�V�t�g�������Ă���Ƃ��͈ړ��ł��Ȃ�����
        if (!Input.GetButton("Lock"))
        {
            _moveArrowDirector.Stop();
            _moveArrowObj.SetActive(false);

            _isMoving = judgeMove((int)x, (int)y);
            //�ړ���ɏ�Q�����Ȃ����ǂ���
            if (_isMoving && (x != 0 || y != 0))
            {
                //�v���C���[�̕�����ۑ�����
                _playerDirection = new Vector2(x, y);

                //�Q�[���}�l�[�W���[�Ńv���C���[���ǂ̕����ɂ��邩���肷��i
                //_gameManagerIns.SetPlayerRoomNum((int)(transform.position.x + x)�@, (int)(transform.position.y + y) * -1);

                //�A�C�e���������ɗ����Ă��Ȃ����ǂ���
                ItemJudge(x, y);

                _nextPosition = transform.position + new Vector3(x, y, 0);

                PlayerLightSet();

                //�Q�[���}�l�[�W���[�Ƀv���C���[�̏ꏊ��n��
                _gameManager.SetPlayerPosition((int)transform.position.x + (int)x, ((int)transform.position.y + (int)y) * -1);

                //1��������1�񕜂���
                _playerStatus.SetHp(1);

                //�ړ�����
                StartCoroutine(TestMove(_nextPosition, x, y));
                //transform.position = Vector3.Lerp(transform.position, _nextPosition, 1);

                _gameManager.TurnType = GameManager.TurnManager.WaitTurn;

                await TestWait();

                //�s�����I������̂Ń^�[���t�F�[�Y��ς���
                _gameManager.TurnType = GameManager.TurnManager.Enemy;
            }
            else
            {

            }
        }
        else
        {
            //�ړ����s�̃i�r�Q�[�V�������o��
            _moveArrowObj.SetActive(true);
            _moveArrowDirector.Play();
            if (x != 0 || y != 0)
            {
                _playerDirection = new Vector2(x, y);
                Debug.Log($"{_playerDirection}�v���C���[�̕��������߂܂���");
            }
        }



        _isMoving = false;
    }

    /// <summary>
    /// �_���W�����̋��ɃA�N�Z�X���Ď��̏ꏊ���ړ��\�����ׂ�
    /// </summary>
    /// <param name="x">�ړ����邘���W</param>
    /// <param name="y">�ړ�����y���W</param>
    /// <returns></returns>
    private bool judgeMove(int x, int y)
    {
        if (x == 0 && y == 0) return false;
        //�}�b�v�f�[�^�ɃA�N�Z�X
        int value = DgGenerator.Instance.Layer.GetMapData(_gameManager.PlayerX + x, _gameManager.PlayerY + y * -1);
        //Debug.Log(value);
        //0�͕ǁA1�͓�
        if (value == MapNum.WallNum)
        {
            return false;

        }
        else if (value == MapNum.LoadNum || value == MapNum.ItemNum)
        {
            return true;
        }
        else if (value == MapNum.StairNum)
        {
            _playerMovement.StairCheck(true);
            return true;
        }

        return false;

    }

    /// <summary>
    /// �����ɃA�C�e�����Ȃ����ǂ������肷��
    /// </summary>
    public void ItemJudge(float x, float y)
    {
        foreach (var i in _gameManager.ItemObjList)
        {
            //�v���C���[�ƃA�C�e���̍��W���d�Ȃ��Ă�����
            if (i.transform.position == this.gameObject.transform.position + new Vector3(x, y, 0))
            {
                var objCs = i.GetComponent<ItemObjectScript>();
                var PlayerStatus = gameObject.GetComponent<PlayerStatus>();

                //�A�C�e�����C���x���g���ɃZ�b�g����
                PlayerStatus.SetItem(objCs.ItemInfomation);
                //�����ɒu�����A�C�e�������X�g����Remove
                _gameManager.RemoveItemObjList(i);
                //�����Ă���A�C�e�����폜
                objCs.DestroyObj();

                return;

            }
        }
    }

    /// <summary>
    /// �����I�ɃA�j���[�V��������ꂽ�ړ������̒ǉ�
    /// </summary>
    /// <param name="next">���ɖڎw���ꏊ</param>
    /// <param name="inputX">x���̓��͒l</param>
    /// <param name="inputY">�����̓��͒l</param>
    /// <returns></returns>
    IEnumerator TestMove(Vector3 next, float inputX, float inputY)
    {
        float t = 0;
        float runSpeed = 1;
        //Debug.Log($"���̖ړI�n��{next}");
        if (Input.GetButton("Dash"))
        {
            runSpeed = _runSpeed;
        }
        _anim.SetBool("Move", true);
        while (true)
        {
            yield return null;
            t += _walkSpeed * runSpeed;
            //�ړ�����
            transform.position = Vector3.Lerp(transform.position, _nextPosition, t);
            if (t >= 1)
            {
                _anim.SetBool("Move", false);
                //�Y���������
                transform.position = Vector3.Lerp(transform.position, _nextPosition, 1);
                break;
            }
        }
        yield return null;
    }

    /// <summary>
    /// Player���ʘH�ɓ���������Light��OnOff������
    /// </summary>
    void PlayerLightSet() 
    {
        if (DgGenerator.Instance.GetDivNum((int)_nextPosition.x, (int)_nextPosition.y) == -1)
        {
            _gameManager.SetLight(false);
            _playerLight.SetActive(true);
        }
        else 
        {
            _gameManager.SetLight(true);
            _playerLight.SetActive(false);
        }
    }

    async UniTask TestWait()
    {
        var t = _afterWalkActionTime;
        if (Input.GetButton("Dash"))
        {
            t = _afterRunActionTime;
        }
        await UniTask.Delay(TimeSpan.FromSeconds(t));
    }

    public Vector2 GetDirection() => _playerDirection;
}
