using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonBehaviour<EnemyManager>
{
    //�G�̃��X�g
    public List<GameObject> EnemyList = new List<GameObject>();

    //�G�̑����̂ǂ��܂œG���s��������
    public int EnemyActionCountNum = 0;

    //�G��̂̍U�����I��������ǂ���
    public bool EnemyActionEnd = false;

    private bool _isMoveing = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        EnemyActionMgr();
        
    }

    /// <summary>
    /// �^�[����Enemy�Ɉڂ������ɊeEnemy�̍s�����n�߂�
    /// </summary>
    private void EnemyActionMgr() 
    {
        if (GameManager.Instance.TurnType == GameManager.TurnManager.Enemy && !EnemyActionEnd && EnemyList.Count > EnemyActionCountNum)
        {
            if (EnemyList[EnemyActionCountNum].TryGetComponent(out IEnemyMove IM))
            {
                 IM.Move();
                Debug.Log("�G���s������");
                EnemyActionCountNum++;
            }
        }
        //Enemy�̍s�������ׂďI�������v���C���[�̃^�[���Ɉڂ�
        else if(EnemyList.Count <= EnemyActionCountNum && !EnemyActionEnd)
        {
            EnemyActionCountNum = 0;
            Debug.Log("�G�̍s�����I�����");
            GameManager.Instance.TurnType = GameManager.TurnManager.Player;
        }
    } 


}
