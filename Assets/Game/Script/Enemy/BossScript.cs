using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : EnemyBase
{
    [Header("�͈͍U�����o���m��")]
    [SerializeField] int _rangeAttack;

    [Header("������΂��U�����o���m��")]
    [SerializeField] int _blowOffAttack;

    [Header("�͈͍U���̃G�t�F�N�g")]
    [SerializeField] GameObject _rangeEffect;

    [Header("ItemEffect")]
    [SerializeField] ItemEffect _effectCs;

    protected override async void AddAttack()
    {
        //���͂R�}�X�ȓ��Ɏ�l���������Ƃ������_���ɌĂ΂��
        if (Mathf.Abs(_gameManager.PlayerX - transform.position.x) < 4 && Mathf.Abs((_gameManager.PlayerY * -1) - transform.position.y) < 4)
        {
            var ram = Random.Range(0, 101);

            if (ram < _rangeAttack)
            {
                //_isAttack = true;
                await EnemyAttack(2);
                if (_gameManager.PlayerY * -1 == transform.position.y)
                {
                    //�G�t�F�N�g�𐶐�����
                    Instantiate(_rangeEffect, _gameManager.PlayerObj.transform.position, new Quaternion(0, 0, 90, 90));
                }
                else 
                {
                    //�G�t�F�N�g�𐶐�����
                    Instantiate(_rangeEffect, _gameManager.PlayerObj.transform.position, transform.rotation);
                }                
            }
        }
        //���͂P�}�X�ȓ��Ɏ�l���������Ƃ������_���ɌĂ΂��
        if (Mathf.Abs(_gameManager.PlayerX - transform.position.x) < 2 && Mathf.Abs((_gameManager.PlayerY * -1) - transform.position.y) < 2)
        {
            var ram = Random.Range(0, 101);

            if (ram < _blowOffAttack)
            {
                //_isAttack = true;
                await EnemyAttack(3);
            }
        }

        else if ((_gameManager.PlayerX == transform.position.x || _gameManager.PlayerY * -1 == transform.position.y)
                 && Mathf.Abs(_gameManager.PlayerX - transform.position.x) < 4 && Mathf.Abs((_gameManager.PlayerY * -1) - transform.position.y) < 4) 
        {
            
        }

        
    }

    protected override async UniTask EnemyAttack(int count = 1)
    {
        await base.EnemyAttack(count);

        if (count == 2)
        {
            //�U�����s�O�̃X�e�[�g���擾���Ȃ��悤�ɂP�t���[���҂�
            await UniTask.DelayFrame(1);

            _stateInfo = default;
            _dir = new Vector2(_gameManager.PlayerX - (int)transform.position.x, _gameManager.PlayerY * -1 - (int)transform.position.y);

            await UniTask.WaitUntil(() => 0.5f <= _stateInfo.normalizedTime);

            //�U��
            _playerBase.AddDamage(_enemyStatus.GetPower, this.gameObject);

            await UniTask.WaitUntil(() => 1f <= _stateInfo.normalizedTime);
        }
        else if (count == 3)
        {
            //Player�ɂ��Ă�ItemEffect���珈�����Ă�
            _gameManager.PlayerObj.GetComponent<ItemEffect>().BlowAwayEffect(_dir);
            Debug.Log("������΂��U��");
        }
        else if (count == 4) 
        {
           
        }
    }
}
