using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBase : MonoBehaviour
{

    [Header("PlayerStatus")]
    [SerializeField] private PlayerStatus _playerStatus;

    [Tooltip("�Q�[���}�l�[�W���[")]
    private GameManager _gameManagerIns;

    [Tooltip("�G�l�~�[�}�l�[�W���[")]
    private EnemyManager _enemyManagerIns;

    [Tooltip("�v���C���[�����Ɉړ�����ꏊ")]
    private Vector3 _nextPosition;

    [Tooltip("�v���C���[������������")]
    private Vector3 _playerDirection;
    public Vector3 PlayerDirection => _playerDirection;

    [Tooltip("���쒆���ǂ���")]
    private bool _isMoving;


    //Test�p
    private float _waitTime = 0.1f;
    private float _countTime = 0;

    private void Start()
    {
        _gameManagerIns = GameManager.Instance;
        _enemyManagerIns = EnemyManager.Instance;
        _gameManagerIns.SetPlayerObj(this.gameObject);

        //�Q�[���}�l�[�W���[�Ƀv���C���[�̏ꏊ��n��i
        // _gameManagerIns.SetPlayerRoomNum((int)(transform.position.x), (int)transform.position.y * -1);

    }
    private void Update()
    {
        if (_gameManagerIns.TurnType == GameManager.TurnManager.Player)
        {
            Attack();
        }
        if (_gameManagerIns.TurnType == GameManager.TurnManager.Player && _waitTime < _countTime)
        {
            MoveInputKey();
        }
        if (_gameManagerIns.TurnType == GameManager.TurnManager.Result) 
        {
            
        }
        _countTime += Time.deltaTime;
    }

    /// <summary>
    /// �ړ��̓��͏���
    /// </summary>
    private void MoveInputKey()
    {
        _countTime = 0;
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        //�V�t�g�������Ă���Ƃ��͈ړ��ł��Ȃ�����
        if (!Input.GetButton("Lock"))
        {
            _isMoving = judgeMove((int)x, (int)y);
            //�ړ���ɏ�Q�����Ȃ����ǂ���
            if (_isMoving && (x != 0 || y != 0))
            {
                //�v���C���[�̕�����ۑ�����
                _playerDirection = new Vector3(x, y, 0);

                //�Q�[���}�l�[�W���[�Ńv���C���[���ǂ̕����ɂ��邩���肷��i
                // _gameManagerIns.SetPlayerRoomNum((int)(transform.position.x + x)�@, (int)(transform.position.y + y) * -1);

                //�A�C�e���������ɗ����Ă��Ȃ����ǂ���
                ItemJudge(x, y);

                //�����Ɋ��G�����������ړ��ł��Ȃ��Ƃ���������ǉ�����
                _nextPosition = transform.position + new Vector3(x, y, 0);

                //�Q�[���}�l�[�W���[�Ƀv���C���[�̏ꏊ��n��
                _gameManagerIns.SetPlayerPosition((int)x, (int)y * -1);

                //�ړ�����
                transform.position = Vector3.Lerp(transform.position, _nextPosition, 1);

                //�s�����I������̂Ń^�[���t�F�[�Y��ς���
                _gameManagerIns.TurnType = GameManager.TurnManager.Enemy;
                Debug.Log("�����̍s�����I���܂���");
            }
            else
            {

            }
        }
        else
        {
            if (x != 0 || y != 0)
            {
                _playerDirection = new Vector2(x, y);
                Debug.Log($"{_playerDirection}�v���C���[�̕��������߂܂���");
            }
        }



        _isMoving = false;
    }


    /// <summary>
    ///�v���C���[�̍U������
    /// </summary>
    private void Attack()
    {
        if (Input.GetButtonDown("Submit"))
        {
            Debug.Log("�U������΂ꂽ");
            //�v���C���[�������Ă�������ɓG�������ꍇ
            foreach (var i in _enemyManagerIns.EnemyBaseList)
            {
                if (transform.position.x + PlayerDirection.x == i.transform.position.x && transform.position.y + PlayerDirection.y == i.transform.position.y)
                {
                     LogScript.Instance.OutPutLog("�U���̏�������������");
                    //�A�j���[�V�����̏���������
                    i.GetComponent<IDamageble>().AddDamage(_playerStatus.Power, this.gameObject);
                    
                }
            }

            //�o���l���Q�b�g�������ǂ����m�F����
            _gameManagerIns.TurnType = GameManager.TurnManager.Result;

           //���̐ݒ�
           //_gameManagerIns.TurnType = GameManager.TurnManager.Enemy;
        }
    }

    private void UIInputKey()
    {
        if (Input.GetButtonDown("Cancel"))
        {

        }

    }

    /// <summary>
    /// �_���W�����̋��ɃA�N�Z�X���Ď��̏ꏊ���ړ��\�����ׂ�
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool judgeMove(int x, int y)
    {
        //�}�b�v�f�[�^�ɃA�N�Z�X
        int value = DgGenerator.Instance.Layer.GetMapData(_gameManagerIns.PlayerX + x, _gameManagerIns.PlayerY + y * -1);

        // Debug.Log(value);

        //0�͕ǁA1�͓�
        if (value == 0)
        {
            return false;

        }
        else if (value == 1 || value == 3)
        {
            return true;
        }

        return false;

    }

    /// <summary>
    /// �����ɃA�C�e�����Ȃ����ǂ������肷��
    /// </summary>
    public void ItemJudge(float x, float y)
    {
        foreach (var i in _gameManagerIns.ItemObjList)
        {
            //�v���C���[�ƃA�C�e���̍��W���d�Ȃ��Ă�����
            if (i.transform.position == this.gameObject.transform.position + new Vector3(x, y, 0))
            {
                var objCs = i.GetComponent<ItemObjectScript>();
                var PlayerStatus = gameObject.GetComponent<PlayerStatus>();

                //�A�C�e�����C���x���g���ɃZ�b�g����
                PlayerStatus.SetItem(objCs.ItemInfomation);
                //�����ɒu�����A�C�e�������X�g����Remove
                _gameManagerIns.RemoveItemObjList(i);
                //�����Ă���A�C�e�����폜
                objCs.DestroyObj();

                return;

            }
        }
    }

    /// <summary>
    /// �o���l���Ȃǃ��U���g�̏���
    /// </summary>
    private void ResultProcess() 
    {
        
    }

}