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
        Normal,
        MainMenuPanel,
        ItemPanel,
        FootPanel,
        UseItemSelect,
        ItemInfomationPanel,

    }

    [SerializeField, Header("MainMenu�̃I�u�W�F�N�g")]
    private GameObject _mainMenuPanel;

    [SerializeField, Header("SelectMenu�̃I�u�W�F�N�g")] 
    private GameObject _SelectMenuPanel;

    [SerializeField, Header("ItemPanel�̃I�u�W�F�N�g")]
    private GameObject _itemPanel;

    [SerializeField, Header("Item�𐶐�����ꏊ")]
    private GameObject _itemContent;

    [SerializeField, Header("footPanel�̃I�u�W�F�N�g")]
    private GameObject _footPanel;

    [SerializeField, Header("�A�C�e�����ǂ��g�������߂�p�l��")]
    private GameObject _useItemSelectPanel;

    [SerializeField, Header("�A�C�e�����ǂ��g�����\������{�^��")]
    private GameObject _useItemButtonPrehab;

    [SerializeField, Header("ItemInfomationPanel�̃I�u�W�F�N�g")]
    private GameObject _itemInforPanel;

    [SerializeField, Header("ItemInformation��Text")]
    private Text _itemInfoText;

    [SerializeField, Header("MainPanel��CanvasGroup")]
    private CanvasGroup _mainPanelCanvasGroup;

    [SerializeField, Header("ItemPanel��CanvasGroup")]
    private CanvasGroup _itemGroupCanvasGroup;

    [SerializeField, Header("ItemButton��Prefab")]
    private GameObject _itemButtonPrefab;


    [Tooltip("PlayerStatus�̃X�N���v�g")]
    private PlayerStatus _playerStatusCs;


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

            if (_uiType == UIType.MainMenuPanel)
            {
                _mainMenuPanel.SetActive(false);

                _uiType = UIType.Normal;
                _gameManager.TurnType = GameManager.TurnManager.Player;
            }
            else if (_uiType == UIType.ItemPanel)
            {
                _itemPanel.SetActive(false);
                _mainPanelCanvasGroup.interactable = true;
                //�������ꂽ�{�^���̍폜
                for (int i = _itemContent.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(_itemContent.transform.GetChild(i).gameObject);
                }

                EventSystem.current.SetSelectedGameObject(_SelectMenuPanel.transform.GetChild(0).gameObject);
                _uiType = UIType.MainMenuPanel;
            }
            else if (_uiType == UIType.FootPanel)
            {
                _footPanel.SetActive(false);
                _uiType = UIType.MainMenuPanel;
            }
            else if (_uiType == UIType.UseItemSelect)
            {
                _useItemSelectPanel.SetActive(false);

                //�������ꂽ�{�^���̍폜
                for (int i = _useItemSelectPanel.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(_useItemSelectPanel.transform.GetChild(i).gameObject);
                }
                _uiType = UIType.ItemPanel;
            }
            else if(_uiType == UIType.ItemInfomationPanel) 
            {
                _itemInforPanel.SetActive(false);

              
                _uiType = UIType.MainMenuPanel;
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
            _uiType = UIType.MainMenuPanel;

        }
    }

    /// <summary>
    /// Player��UI�̃{�^���C�x���g����Ăяo�����
    /// </summary>
    /// <param name="panelName">�Ăяo�����߂̃p�l���̖��O</param>
    public void PlayerUIActive(string panelName)
    {
        if (UIType.MainMenuPanel == _uiType && panelName == "ItemPanel")
        {
            //���Ԃ��]�����炱�����I�u�W�F�N�g�v�[���ŃA�C�e�����Ăяo��

            //_itemPanel.SetActive(true);
            //����null�������ꍇ���g������
            if (_playerStatusCs == null) 
            {
                _playerStatusCs = _gameManager.PlayerObj.GetComponent<PlayerStatus>();
            }

            GameObject ItemButtonIns;
            //�����Ă���A�C�e���̐���
            foreach (var item in _playerStatusCs.PlayerItemList) 
            {
                ItemButtonIns = Instantiate(_itemButtonPrefab, _itemContent.transform);

                ItemButtonIns.transform.Find("ItemName").GetComponent<Text>().text = item.GetItemName;
                ItemButtonIns.transform.Find("ItemImage").GetComponent<Image>().sprite = item.GetItemImage;
                //�{�^���C�x���g��ǉ�����
                ItemButtonIns.GetComponent<Button>().onClick.AddListener(() => SelectItem(item));

                if (_playerStatusCs.WeaponEquip == item)
                {
                    ItemButtonIns.transform.Find("Equip").GetComponent<Text>().text = "E";
                }
                else if (_playerStatusCs.ShieldEquip == item) 
                {
                    ItemButtonIns.transform.Find("Equip").GetComponent<Text>().text = "E";
                }

              

            }

            //�A�C�e���������Ă��邩�ǂ���
            if (_itemContent.transform.childCount != 0) 
            {

                _uiType = UIType.ItemPanel;
                _itemPanel.SetActive(true);
                _itemGroupCanvasGroup.interactable = true;
                _mainPanelCanvasGroup.interactable = false;
                EventSystem.current.SetSelectedGameObject(_itemContent.transform.GetChild(0).gameObject);
            }
            else 
            {
                _itemInforPanel.SetActive(true);
                _itemInfoText.text = "�A�C�e���������Ă��܂���";

                _uiType = UIType.ItemInfomationPanel;
            }
           
           
        }
        else if (UIType.MainMenuPanel == _uiType && panelName == "footPanel")
        {
            _footPanel.SetActive(true);
            _mainPanelCanvasGroup.interactable = false;

            _uiType = UIType.FootPanel;
        }
       
    }

    /// <summary>
    /// �A�C�e����I�񂾎��ɌĂ΂�郁�\�b�h
    /// </summary>
    /// <param name="item">Item�̏��</param>
    public void SelectItem(Item item) 
    {
        var ItemSelectButton = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
        if (item.GetItemType == Item.ItemType.MagicBook ) 
        {
            ItemSelectButton.GetComponentInChildren<Text>().text = "�ǂ�";
            ItemSelectButton.GetComponent<Button>().onClick.AddListener(() => UseSelectItem(item));

            var ItemSelectButton2 = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
            ItemSelectButton2.GetComponentInChildren<Text>().text = "������";
            ItemSelectButton2.GetComponent<Button>().onClick.AddListener(() => ThrowItem(item));

            var ItemSelectButton3 = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
            ItemSelectButton3.GetComponentInChildren<Text>().text = "�u��";
            ItemSelectButton3.GetComponent<Button>().onClick.AddListener(() => ItemPut(item));

            var ItemSelectButton4 = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
            ItemSelectButton4.GetComponentInChildren<Text>().text = "����";
            ItemSelectButton4.GetComponent<Button>().onClick.AddListener(() => ItemExplanation(item));
        }

        _useItemSelectPanel.SetActive(true);
       _itemGroupCanvasGroup.interactable = false;
        EventSystem.current.SetSelectedGameObject(_useItemSelectPanel.transform.GetChild(0).gameObject);
        _uiType = UIType.UseItemSelect;
    }


    /// <summary>
    /// �A�C�e�����g�����Ƃ�
    /// </summary>
    /// <param name="item"></param>
    private void UseSelectItem(Item item) 
    {
        if (item.GetItemType == Item.ItemType.MagicBook) 
        {
            if (item.GetItemName == "Kaihukunosyo") 
            {
                _playerStatusCs.RemoveItem(item);
                _playerStatusCs.SetHp(item.GetItemEffect);
                ResetMenu();

                Debug.Log($"{item.GetItemEffect}�񕜂��܂���");   
            }
        }
    }

    /// <summary>
    /// �A�C�e���𓊂��郁�\�b�h
    /// </summary>
    /// <param name="item"></param>
    private void ThrowItem(Item item) 
    {

    }

    /// <summary>
    /// �A�C�e�������̏�ɒu��
    /// </summary>
    /// <param name="item"></param>
    private void ItemPut(Item item) 
    {

    }

    /// <summary>
    /// �A�C�e���̐�����\������
    /// </summary>
    /// <param name="item"></param>
    private void ItemExplanation(Item item) 
    {

    }

    /// <summary>
    /// ���j���[��ʂ����ׂĕ��郁�\�b�h
    /// �A�C�e�����g������ȂǂɎg��
    /// </summary>
    private void ResetMenu()
    {
        _mainMenuPanel.SetActive(false);
        _itemPanel.SetActive(false);
        _useItemSelectPanel.SetActive(false);

        _mainPanelCanvasGroup.interactable = true;
        _itemGroupCanvasGroup.interactable = true;

        //�������ꂽ�{�^���̍폜
        for (int i = _itemContent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_itemContent.transform.GetChild(i).gameObject);
        }

        for (int i = _useItemSelectPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_useItemSelectPanel.transform.GetChild(i).gameObject);
        }

        _uiType = UIType.Normal;
        _gameManager.TurnType = GameManager.TurnManager.Player;
    }
}
