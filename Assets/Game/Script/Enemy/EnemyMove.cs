using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{

    [SerializeField] private AsterTest _aster;
    [Tooltip("�Q�[���}�l�[�W���[�̃C���X�^���X")]
    private GameManager _gameManagerIns;
    [Tooltip("�G�l�~�[�}�l�[�W���[�̃C���X�^���X")]
    private EnemyManager _enemyManagerIns;
    // Start is called before the first frame update
    void Start()
    {
        _gameManagerIns = GameManager.Instance;
        _enemyManagerIns = EnemyManager.Instance;
        //�G�l�~�[�}�l�[�W���[�Ɏ������g�̃I�u�W�F�N�g��n��
        _enemyManagerIns.EnemyList.Add(this.gameObject);
        //_aster.Aster();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Move()
    {
        //int value = DgGenerator.Instance.Layer.GetMapData();

    }
}
