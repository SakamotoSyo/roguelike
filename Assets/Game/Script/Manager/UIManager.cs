using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{

    public enum UIType
    {
        _normal,
        _mainMenuPanel,
        _itemPanel,
        _footPanel,
        _useSelectPanel,

    }

    [SerializeField, Header("MainMenu�̃I�u�W�F�N�g")]
    private GameObject _mainMenuPanel;

    [SerializeField, Header("SelectMenu�̃I�u�W�F�N�g")] 
    private GameObject _SelectMenuPanel;

    [SerializeField, Header("ItemPanel�̃I�u�W�F�N�g")]
    private GameObject _itemPanel;

    [SerializeField, Header("Item�𐶐�����ꏊ")]
    private GameManager _itemContent;

    [SerializeField, Header("footPanel�̃I�u�W�F�N�g")]
    private GameObject _footPanel;

    [SerializeField, Header("UseItemSelectPanel�̃I�u�W�F�N�g")]
    private GameObject _useSelectPanel;

    [SerializeField, Header("MainPanel��CanvasGroup")]
    private CanvasGroup _mainPanelCanvasGroup;

    [SerializeField, Header("ItemPanel��CanvasGroup")]
    private CanvasGroup _itemGroupCanvasGroup;

    [SerializeField, Header("ItemButton��Prefab")]
    private GameObject _itemButtonPrefab;

    [Tooltip("PlayerBase�̃X�N���v�g")]
    private PlayerStatus _playerBaseCs;


    [Tooltip("�Q�[���}�l�[�W���[")] private GameManager _gameManager;

    /// <summary>���݂�UIType</summary>
    private UIType _uiType;


    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        //_playerBaseCs = _gameManager.PlayerObj.GetComponent<PlayerBase>();
    }

    // Update is called once per frame
    void Update()
    {

        if (_gameManager.TurnType == GameManager.TurnManager.Player)
        {
            PlayerMainUI();
        }
        else
        {
            CancelUI();
        }
    }


    /// <summary>
    /// UI���J����Cancel���������Ƃ�
    /// </summary>
    public void CancelUI()
    {
        if (Input.GetButtonDown("Cancel"))
        {

            if (_uiType == UIType._mainMenuPanel)
            {
                _mainMenuPanel.SetActive(false);

                _uiType = UIType._normal;
                _gameManager.TurnType = GameManager.TurnManager.Player;
            }
            else if (_uiType == UIType._itemPanel)
            {
                _itemPanel.SetActive(false);
                _mainPanelCanvasGroup.interactable = true;
                //�����Ă���A�C�e���̍폜
                EventSystem.current.SetSelectedGameObject(_SelectMenuPanel.transform.GetChild(0).gameObject);
                _uiType = UIType._mainMenuPanel;
            }
            else if (_uiType == UIType._footPanel)
            {
                _footPanel.SetActive(false);
                _uiType = UIType._mainMenuPanel;
            }
            else if (_uiType == UIType._useSelectPanel)
            {
                _useSelectPanel.SetActive(false);
                _uiType = UIType._itemPanel;
            }

        }
    }


    /// <summary>
    /// �v���C���[����̃L�[���͂�MainUI��\��������
    /// </summary>
    private void PlayerMainUI()
    {
        if (_gameManager.TurnType == GameManager.TurnManager.Player && Input.GetButtonDown("Cancel"))
        {
            _gameManager.TurnType = GameManager.TurnManager.MenuOpen;
            _mainMenuPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_SelectMenuPanel.transform.GetChild(0).gameObject);
            _uiType = UIType._mainMenuPanel;

        }
    }

    /// <summary>
    /// Player��UI�̃{�^���C�x���g����Ăяo�����
    /// </summary>
    /// <param name="panelName">�Ăяo�����߂̃p�l���̖��O</param>
    public void PlayerUIActive(string panelName)
    {
        if (UIType._mainMenuPanel == _uiType && panelName == "ItemPanel")
        {
            //���Ԃ��]�����炱�����I�u�W�F�N�g�v�[���ŃA�C�e�����Ăяo��

            //_itemPanel.SetActive(true);
            //����null�������ꍇ���g������
            if (_playerBaseCs == null) 
            {
                _playerBaseCs = _gameManager.PlayerObj.GetComponent<PlayerStatus>();
            }

            GameObject ItemButtonIns;
            //�����Ă���A�C�e���̐���
            foreach (var item in _playerBaseCs.PlayerItemList) 
            {
                ItemButtonIns = Instantiate(_itemButtonPrefab, _itemContent.transform);

                ItemButtonIns.transform.Find("ItemName").GetComponent<Text>().text = item.GetItemName;
                ItemButtonIns.transform.Find("ItemImage").GetComponent<Image>().sprite = item.GetItemImage;
                //�{�^���C�x���g��ǉ�����
                ItemButtonIns.GetComponent<Button>().onClick.AddListener(() => SelectItem(item));

                if (_playerBaseCs.WeaponEquip == item)
                {
                    ItemButtonIns.transform.Find("Equip").GetComponent<Text>().text = "E";
                }
                else if (_playerBaseCs.ShieldEquip == item) 
                {
                    ItemButtonIns.transform.Find("Equip").GetComponent<Text>().text = "E";
                }



            }

            if (_itemContent.transform.childCount != 0) 
            {

                _uiType = UIType._itemPanel;
                _itemPanel.SetActive(true);
                _itemGroupCanvasGroup.interactable = true;
                _mainPanelCanvasGroup.interactable = false;
                EventSystem.current.SetSelectedGameObject(_itemContent.transform.GetChild(0).gameObject);
            }
           
           
        }
        else if (UIType._mainMenuPanel == _uiType && panelName == "footPanel")
        {
            _footPanel.SetActive(true);
            _mainPanelCanvasGroup.interactable = false;

            _uiType = UIType._footPanel;
        }
        else if (UIType._mainMenuPanel == _uiType && panelName == "useSelectPanel")
        {
            _useSelectPanel.SetActive(true);
            _itemGroupCanvasGroup.interactable = false;

            _uiType = UIType._useSelectPanel;
        }
    }

    private void SelectItem(Item item) 
    {

    }

}
