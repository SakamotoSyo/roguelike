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

    [SerializeField, Header("MainMenuのオブジェクト")]
    private GameObject _mainMenuPanel;

    [SerializeField, Header("SelectMenuのオブジェクト")] 
    private GameObject _SelectMenuPanel;

    [SerializeField, Header("ItemPanelのオブジェクト")]
    private GameObject _itemPanel;

    [SerializeField, Header("Itemを生成する場所")]
    private GameObject _itemContent;

    [SerializeField, Header("footPanelのオブジェクト")]
    private GameObject _footPanel;

    [SerializeField, Header("アイテムをどう使うか決めるパネル")]
    private GameObject _useItemSelectPanel;

    [SerializeField, Header("アイテムをどう使うか表示するボタン")]
    private GameObject _useItemButtonPrehab;

    [SerializeField, Header("ItemInfomationPanelのオブジェクト")]
    private GameObject _itemInforPanel;

    [SerializeField, Header("ItemInformationのText")]
    private Text _itemInfoText;

    [SerializeField, Header("MainPanelのCanvasGroup")]
    private CanvasGroup _mainPanelCanvasGroup;

    [SerializeField, Header("ItemPanelのCanvasGroup")]
    private CanvasGroup _itemGroupCanvasGroup;

    [SerializeField, Header("ItemButtonのPrefab")]
    private GameObject _itemButtonPrefab;


    [Tooltip("PlayerStatusのスクリプト")]
    private PlayerStatus _playerStatusCs;


    [Tooltip("ゲームマネージャー")] private GameManager _gameManager;

    /// <summary>現在のUIType</summary>
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
    /// UIを開いてCancelを押したとき
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
                //生成されたボタンの削除
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

                //生成されたボタンの削除
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
    /// プレイヤー特定のキー入力でMainUIを表示させる
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
    /// PlayerのUIのボタンイベントから呼び出される
    /// </summary>
    /// <param name="panelName">呼び出すためのパネルの名前</param>
    public void PlayerUIActive(string panelName)
    {
        if (UIType.MainMenuPanel == _uiType && panelName == "ItemPanel")
        {
            //時間が余ったらここをオブジェクトプールでアイテムを呼び出す

            //_itemPanel.SetActive(true);
            //もしnullだった場合中身を入れる
            if (_playerStatusCs == null) 
            {
                _playerStatusCs = _gameManager.PlayerObj.GetComponent<PlayerStatus>();
            }

            GameObject ItemButtonIns;
            //持っているアイテムの生成
            foreach (var item in _playerStatusCs.PlayerItemList) 
            {
                ItemButtonIns = Instantiate(_itemButtonPrefab, _itemContent.transform);

                ItemButtonIns.transform.Find("ItemName").GetComponent<Text>().text = item.GetItemName;
                ItemButtonIns.transform.Find("ItemImage").GetComponent<Image>().sprite = item.GetItemImage;
                //ボタンイベントを追加する
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

            //アイテムを持っているかどうか
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
                _itemInfoText.text = "アイテムを持っていません";

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
    /// アイテムを選んだ時に呼ばれるメソッド
    /// </summary>
    /// <param name="item">Itemの情報</param>
    public void SelectItem(Item item) 
    {
        var ItemSelectButton = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
        if (item.GetItemType == Item.ItemType.MagicBook ) 
        {
            ItemSelectButton.GetComponentInChildren<Text>().text = "読む";
            ItemSelectButton.GetComponent<Button>().onClick.AddListener(() => UseSelectItem(item));

            var ItemSelectButton2 = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
            ItemSelectButton2.GetComponentInChildren<Text>().text = "投げる";
            ItemSelectButton2.GetComponent<Button>().onClick.AddListener(() => ThrowItem(item));

            var ItemSelectButton3 = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
            ItemSelectButton3.GetComponentInChildren<Text>().text = "置く";
            ItemSelectButton3.GetComponent<Button>().onClick.AddListener(() => ItemPut(item));

            var ItemSelectButton4 = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
            ItemSelectButton4.GetComponentInChildren<Text>().text = "説明";
            ItemSelectButton4.GetComponent<Button>().onClick.AddListener(() => ItemExplanation(item));
        }

        _useItemSelectPanel.SetActive(true);
       _itemGroupCanvasGroup.interactable = false;
        EventSystem.current.SetSelectedGameObject(_useItemSelectPanel.transform.GetChild(0).gameObject);
        _uiType = UIType.UseItemSelect;
    }


    /// <summary>
    /// アイテムを使ったとき
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

                Debug.Log($"{item.GetItemEffect}回復しました");   
            }
        }
    }

    /// <summary>
    /// アイテムを投げるメソッド
    /// </summary>
    /// <param name="item"></param>
    private void ThrowItem(Item item) 
    {

    }

    /// <summary>
    /// アイテムをその場に置く
    /// </summary>
    /// <param name="item"></param>
    private void ItemPut(Item item) 
    {

    }

    /// <summary>
    /// アイテムの説明を表示する
    /// </summary>
    /// <param name="item"></param>
    private void ItemExplanation(Item item) 
    {

    }

    /// <summary>
    /// メニュー画面をすべて閉じるメソッド
    /// アイテムを使った後などに使う
    /// </summary>
    private void ResetMenu()
    {
        _mainMenuPanel.SetActive(false);
        _itemPanel.SetActive(false);
        _useItemSelectPanel.SetActive(false);

        _mainPanelCanvasGroup.interactable = true;
        _itemGroupCanvasGroup.interactable = true;

        //生成されたボタンの削除
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
