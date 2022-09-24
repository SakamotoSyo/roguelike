using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;

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
        ShowText,
        StairPanel,
        HelpPanel,

    }

    [Header("AudioSouse")]
    [SerializeField] AudioSource _audioSource;

    [Header("���j���[�̑I����")]
    [SerializeField] AudioClip _selectSound;

    [Header("���j���[�̃L�����Z����")]
    [SerializeField] AudioClip _cancelSound;

    [SerializeField, Header("MainMenu�̃I�u�W�F�N�g")]
    private GameObject _mainMenuPanel;

    [SerializeField, Header("SelectMenu�̃I�u�W�F�N�g")]
    private GameObject _SelectMenuPanel;

    [SerializeField, Header("ItemPanel�̃I�u�W�F�N�g")]
    private GameObject _itemPanel;

    [SerializeField, Header("HelpPanel�̃I�u�W�F�N�g")]
    private GameObject _helpPanel;

    [SerializeField, Header("Item�𐶐�����ꏊ")]
    private GameObject _itemContent;

    [SerializeField, Header("footPanel�̃I�u�W�F�N�g")]
    private GameObject _footPanel;

    [SerializeField, Header("�A�C�e�����ǂ��g�������߂�p�l��")]
    private GameObject _useItemSelectPanel;

    [SerializeField, Header("�A�C�e�����ǂ��g�������߂�p�l����Sprite")]
    private GameObject _useItemSprite;

    [SerializeField, Header("�A�C�e�����ǂ��g�����\������{�^��")]
    private GameObject _useItemButtonPrehab;

    [SerializeField, Header("ItemInfomationPanel�̃I�u�W�F�N�g")]
    private GameObject _itemInforPanel;

    [SerializeField, Header("StairPanel��Object")]
    private GameObject _stairPanel;

    [SerializeField, Header("ItemInformation��Text")]
    private Text _itemInfoText;

    [SerializeField, Header("MainPanel��CanvasGroup")]
    private CanvasGroup _mainPanelCanvasGroup;

    [SerializeField, Header("ItemPanel��CanvasGroup")]
    private CanvasGroup _itemGroupCanvasGroup;

    [SerializeField, Header("ItemButton��Prefab")]
    private GameObject _itemButtonPrefab;

    [SerializeField, Header("ItemObject��Prefab")]
    private GameObject _itemObjectPrehab;

    [Header("������΂��̏��U�������ɐ��������Obj")]
    [SerializeField] GameObject _blowObj;

    [Header("���̐΂��g�����Ƃ��ɐ��������Obj")]
    [SerializeField] GameObject _thunderStone;

    [Header("�ǂ̃{�^�������ɃX�N���[�����邩")]
    [SerializeField] int _scrollDownButtonNum = 8;

    [Header("�ǂ̃{�^�����牺�ɃX�N���[�����邩")]
    [SerializeField] int _scrollUpButtonNum = 10;

    [Header("ScrollBar")]
    [SerializeField] ScrollManager _scrollManager;

    [Tooltip("PlayerStatus�̃X�N���v�g")]
    private PlayerStatus _playerStatusCs;

    [Tooltip("PlayerBase�̃X�N���v�g")]
    private PlayerMove _playerMoveCs;


    [Tooltip("�Q�[���}�l�[�W���[")]
    private GameManager _gameManager;

    [Tooltip("�_���W��������")]
    private DgGenerator _dgGenerator;

    private float _testPosition;

    /// <summary>���݂�UIType</summary>
    private UIType _uiType;

    // Start is called before the first frame update
    async void Start()
    {
        _gameManager = GameManager.Instance;
        _dgGenerator = DgGenerator.Instance;

        _gameManager.TurnType = GameManager.TurnManager.WaitTurn;
        //�ŏ��ɂ��炩���ߎ������̏�����̃{�^�����쐬
        for (int i = 0; i > 20; i++)
        {
            var a = Instantiate(_itemButtonPrefab, _itemContent.transform);
        }
        //_playerBaseCs = _gameManager.PlayerObj.GetComponent<PlayerBase>();

        await TestWait();
        _uiType = UIType.HelpPanel;
        _helpPanel.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {

        if (_gameManager.TurnType == GameManager.TurnManager.Player && _uiType == UIType.Normal)
        {
            PlayerMainUI();
        }
        else if (_gameManager.TurnType == GameManager.TurnManager.Story) 
        {
           
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
                _audioSource.PlayOneShot(_cancelSound);
                _uiType = UIType.Normal;
                _gameManager.TurnType = GameManager.TurnManager.Player;
            }
            else if (_uiType == UIType.ItemPanel)
            {
                _itemPanel.SetActive(false);
                _audioSource.PlayOneShot(_cancelSound);
                _mainPanelCanvasGroup.interactable = true;
                //�������ꂽ�{�^���̍폜
                for (int i = _itemContent.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(_itemContent.transform.GetChild(i).gameObject);
                }
                _scrollManager.Reset();
                EventSystem.current.SetSelectedGameObject(_SelectMenuPanel.transform.GetChild(0).gameObject);
                _uiType = UIType.MainMenuPanel;
            }
            else if (_uiType == UIType.FootPanel)
            {
                _footPanel.SetActive(false);
                _audioSource.PlayOneShot(_cancelSound);
                _uiType = UIType.MainMenuPanel;
            }
            else if (_uiType == UIType.UseItemSelect)
            {
                _useItemSprite.SetActive(false);
                _audioSource.PlayOneShot(_cancelSound);
                //�������ꂽ�{�^���̍폜
                for (int i = _useItemSelectPanel.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(_useItemSelectPanel.transform.GetChild(i).gameObject);
                }
                _uiType = UIType.ItemPanel;
            }
            else if (_uiType == UIType.StairPanel)
            {
                _stairPanel.SetActive(false);
                _audioSource.PlayOneShot(_cancelSound);
                _gameManager.TurnType = GameManager.TurnManager.Player;
                _uiType = UIType.Normal;
            }
            else if (_uiType == UIType.ItemInfomationPanel)
            {
                _itemInforPanel.SetActive(false);
                _audioSource.PlayOneShot(_cancelSound);
                EventSystem.current.SetSelectedGameObject(_useItemSelectPanel.transform.GetChild(0).gameObject);
                _uiType = UIType.UseItemSelect;

            }
            else if (_uiType == UIType.HelpPanel) 
            {
                _helpPanel.SetActive(false);
                _audioSource.PlayOneShot(_cancelSound);
                _uiType = UIType.Normal;
                _gameManager.TurnType = GameManager.TurnManager.Player;
            }

        }

        if (Input.GetButtonDown("Submit")) 
        {
            if (_uiType == UIType.ItemInfomationPanel)
            {
                _itemInforPanel.SetActive(false);
                _audioSource.PlayOneShot(_cancelSound);
                _gameManager.TurnType = GameManager.TurnManager.Player;
                _uiType = UIType.Normal;
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
            _audioSource.PlayOneShot(_selectSound);
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
            int itemPanelButtonNum = 0;
            GameObject ItemButtonIns;
            //�����Ă���A�C�e���̐���
            foreach (var item in _playerStatusCs.PlayerItemList)
            {
                ItemButtonIns = Instantiate(_itemButtonPrefab, _itemContent.transform);

                ItemButtonIns.transform.Find("ItemName").GetComponent<Text>().text = item.GetItemName;
                ItemButtonIns.transform.Find("ItemImage").GetComponent<Image>().sprite = item.GetItemImage;
                //�{�^���C�x���g��ǉ�����
                ItemButtonIns.GetComponent<Button>().onClick.AddListener(() => SelectItem(item));

                if (itemPanelButtonNum != 0
                    && (itemPanelButtonNum % _scrollDownButtonNum == 0))
                {
                    ItemButtonIns.AddComponent<ScrollDownScript>();
                    Debug.Log("�Ă΂ꂽ");
                }
                else if (itemPanelButtonNum != 0
                    && itemPanelButtonNum % _scrollUpButtonNum == 0) 
                {
                    ItemButtonIns.AddComponent<ScrollUpScript>();
                }

                if (_playerStatusCs.WeaponEquip == item)
                {
                    ItemButtonIns.transform.Find("Equip").GetComponent<Text>().text = "E";
                }
                else if (_playerStatusCs.ShieldEquip == item)
                {
                    ItemButtonIns.transform.Find("Equip").GetComponent<Text>().text = "E";
                }

                itemPanelButtonNum++;

                if (itemPanelButtonNum == _scrollUpButtonNum + 2)
                {
                    Debug.Log(itemPanelButtonNum);
                    itemPanelButtonNum = 2;
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
        if (item.GetItemType == Item.ItemType.MagicBook)
        {
            ItemSelectButton.GetComponentInChildren<Text>().text = "�ǂ�";
            ItemSelectButton.GetComponent<Button>().onClick.AddListener(() => UseSelectItem(item));

            var ItemSelectButton2 = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
            ItemSelectButton2.GetComponentInChildren<Text>().text = "������";
            ItemSelectButton2.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(ThrowItem(item)));

            var ItemSelectButton3 = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
            ItemSelectButton3.GetComponentInChildren<Text>().text = "�u��";
            ItemSelectButton3.GetComponent<Button>().onClick.AddListener(() => ItemPut(item));

            var ItemSelectButton4 = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
            ItemSelectButton4.GetComponentInChildren<Text>().text = "����";
            ItemSelectButton4.GetComponent<Button>().onClick.AddListener(() => ItemExplanation(item));
        }
        else if (item.GetItemType == Item.ItemType.SpecialItem)
        {
            ItemSelectButton.GetComponentInChildren<Text>().text = "�U��";
            ItemSelectButton.GetComponent<Button>().onClick.AddListener(() => UseSelectItem(item));

            var ItemSelectButton2 = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
            ItemSelectButton2.GetComponentInChildren<Text>().text = "������";
            ItemSelectButton2.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(ThrowItem(item)));

            var ItemSelectButton3 = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
            ItemSelectButton3.GetComponentInChildren<Text>().text = "�u��";
            ItemSelectButton3.GetComponent<Button>().onClick.AddListener(() => ItemPut(item));

            var ItemSelectButton4 = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
            ItemSelectButton4.GetComponentInChildren<Text>().text = "����";
            ItemSelectButton4.GetComponent<Button>().onClick.AddListener(() => ItemExplanation(item));
        }

        _useItemSprite.SetActive(true);
        Debug.Log("Select");
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
        _playerStatusCs.RemoveItem(item);
        if (item.GetEffectType == Item.ItemEffectType.Hearing)
        {
            _playerStatusCs.SetHp(item.GetItemEffect);

            ShowText($"{item.GetItemEffect}�񕜂��܂���");
            Input.ResetInputAxes();
        }
        else if (item.GetEffectType == Item.ItemEffectType.Food)
        {

        }
        else if (item.GetEffectType == Item.ItemEffectType.Special)
        {
            if (item.GetItemName == "���[�v�̏�")
            {
                //�v���C���[�������_���Ƀ��[�v����
                _dgGenerator.PlayerRespawn();
                ResetMenu();
            }
            else if (item.GetItemName == "������΂��̏�")
            {
                //�G�ɓ�����Ɛ�����΂�Object�𐶐�
                Instantiate(_blowObj, new Vector2(_gameManager.PlayerX, _gameManager.PlayerY * -1), transform.rotation);
                ResetMenu(true);
            }
            else if (item.GetItemName == "���̐�") 
            {
                Instantiate(_thunderStone, new Vector2(_gameManager.PlayerX, _gameManager.PlayerY * -1), transform.rotation);
                ResetMenu();
            }
        }

    }

    /// <summary>
    /// �A�C�e���𓊂��郁�\�b�h
    /// </summary>
    /// <param name="item"></param>
    public IEnumerator ThrowItem(Item item)
    {
        //�A�C�e���𐶐�����
        var Item = Instantiate(_itemObjectPrehab, _gameManager.PlayerObj.transform.position, _gameManager.PlayerObj.transform.rotation);
        var ItemObjectCs = Item.GetComponent<ItemObjectScript>();

        //�A�C�e���ɏ���ݒ�
        ItemObjectCs.SetItemInfor(item);
        ItemObjectCs.SetItemSprite(item.GetItemImage);
        var _playerDir = _gameManager.PlayerObj.GetComponent<IDirection>();
        //�v���C���[�̓����������������Ă���
        int x = (int)_playerDir.GetDirection().x;
        int y = (int)_playerDir.GetDirection().y;

        if (x == 0 && y == 0)
        {
            y = -1;
        }

        //�A�C�e�������ǂ̍��W�܂Ŕ�ё�����
        while (_dgGenerator.Layer.GetMapData(_gameManager.PlayerX + x, _gameManager.PlayerY + y * -1) == 1)
        {
            x += Math.Sign(x);
            y += Math.Sign(y);
        }

        x += Math.Sign(x) * -1;
        y += Math.Sign(y) * -1;

        //�ړ����鎟�̏ꏊ
        Vector3 _nextPosition = new Vector3(_gameManager.PlayerX + x, _gameManager.PlayerY * -1 + y, 0);
        //���̏ꏊ����ړI�n�܂ł̋���
        //var _distance_Two = Vector3.Distance(Item.transform.position, _nextPosition);
        //�z��ɃA�C�e���̏ꏊ���Z�b�g����
        _dgGenerator.Layer.SetData(_gameManager.PlayerX + x, _gameManager.PlayerY + y * -1, 3);
        Debug.Log(_dgGenerator.Layer.GetMapData(_gameManager.PlayerX + x, _gameManager.PlayerY + y * -1));
        //�ړ�����
        StartCoroutine(ItemThrowMove(Item, _nextPosition));

        yield return new WaitForSeconds(0.08f);
        //�A�C�e���̃I�u�W�F�N�g���Q�[���}�l�[�W���[�ɓn��
        _gameManager.SetItemObjList(Item);

        _playerStatusCs.RemoveItem(item);

        ResetMenu();
    }

    /// <summary>
    /// �A�C�e�����w��̏ꏊ�܂ňړ�������
    /// </summary>
    /// <param name="Item"></param>
    /// <param name="_nextPosition"></param>
    private IEnumerator ItemThrowMove(GameObject Item, Vector3 _nextPosition)
    {
        while (Item.transform.position != _nextPosition)
        {
            //�������X�s�[�h
            _testPosition += 0.05f;
            //�ړ�����
            Item.transform.position = Vector3.Lerp(_gameManager.PlayerObj.transform.position, _nextPosition, _testPosition);
            yield return new WaitForSeconds(0.01f);
        }

        _testPosition = 0;
    }


    /// <summary>
    /// �A�C�e�������̏�ɒu��
    /// </summary>
    /// <param name="item"></param>
    private void ItemPut(Item item)
    {
        //�A�C�e���̐���
        var Item = Instantiate(_itemObjectPrehab, _gameManager.PlayerObj.transform.position, _gameManager.PlayerObj.transform.rotation);
        var ItemObjectCs = Item.GetComponent<ItemObjectScript>();

        //�A�C�e���ɏ���n��
        ItemObjectCs.SetItemInfor(item);
        ItemObjectCs.SetItemSprite(item.GetItemImage);
        _playerMoveCs = _gameManager.PlayerObj.GetComponent<PlayerMove>();
        //�A�C�e���̃I�u�W�F�N�g���Q�[���}�l�[�W���[�ɓn��
        _gameManager.SetItemObjList(Item);
        //�z��ɃA�C�e���̏ꏊ��Set����
        _dgGenerator.Layer.SetData((int)_gameManager.PlayerObj.transform.position.x, (int)_gameManager.PlayerObj.transform.position.y, 3);
        //�v���C���[�̃A�C�e�������炻�̏�ɒu�����A�C�e������������
        _playerStatusCs.RemoveItem(item);

        ResetMenu();
    }

    /// <summary>
    /// �A�C�e���̐�����\������
    /// </summary>
    /// <param name="item"></param>
    private void ItemExplanation(Item item)
    {
        Input.ResetInputAxes();
        Debug.Log("�����傠�����ǂ��킊");
        _itemInfoText.text = item.GetItemInformationText;
        _itemInforPanel.SetActive(true);
        _uiType = UIType.ItemInfomationPanel;
    }

    /// <summary>
    /// ���j���[��ʂ����ׂĕ��郁�\�b�h
    /// �A�C�e�����g������ȂǂɎg��
    /// </summary>
    private void ResetMenu(bool specialItem = false)
    {
        _mainMenuPanel.SetActive(false);
        _itemPanel.SetActive(false);
        _useItemSprite.SetActive(false);

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
        if (specialItem)
        {
            
        }
        else 
        {
            _gameManager.TurnType = GameManager.TurnManager.Player;
        }
    }

    /// <summary>
    /// �����ɓ��ꂽ���̂�Text�ŕ\������
    /// </summary>
    private void ShowText(string st)
    {
        ResetMenu();
        _gameManager.TurnType = GameManager.TurnManager.MenuOpen;
        _uiType = UIType.ItemInfomationPanel;

        _itemInforPanel.SetActive(true);
        _itemInfoText.text = st;
    }

    /// <summary>�K�i�Ɋւ���UI��\�������鏈��</summary>
    public void StairUI()
    {

        _uiType = UIType.StairPanel;

        _stairPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_stairPanel.transform.GetChild(0).gameObject);
    }

    public void PlayerStory() 
    {

    }

    async UniTask TestWait()
    {
        var t = 4.5f;
        await UniTask.Delay(TimeSpan.FromSeconds(t));
    }
}
