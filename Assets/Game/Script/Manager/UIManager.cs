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

    [SerializeField, Header("MainMenuのオブジェクト")]
    private GameObject _mainMenuPanel;

    [SerializeField, Header("SelectMenuのオブジェクト")] 
    private GameObject _SelectMenuPanel;

    [SerializeField, Header("ItemPanelのオブジェクト")]
    private GameObject _itemPanel;

    [SerializeField, Header("Itemを生成する場所")]
    private GameManager _itemContent;

    [SerializeField, Header("footPanelのオブジェクト")]
    private GameObject _footPanel;

    [SerializeField, Header("UseItemSelectPanelのオブジェクト")]
    private GameObject _useSelectPanel;

    [SerializeField, Header("MainPanelのCanvasGroup")]
    private CanvasGroup _mainPanelCanvasGroup;

    [SerializeField, Header("ItemPanelのCanvasGroup")]
    private CanvasGroup _itemGroupCanvasGroup;

    [SerializeField, Header("ItemButtonのPrefab")]
    private GameObject _itemButtonPrefab;

    [Tooltip("PlayerBaseのスクリプト")]
    private PlayerStatus _playerBaseCs;


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
                //持っているアイテムの削除
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
    /// プレイヤー特定のキー入力でMainUIを表示させる
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
    /// PlayerのUIのボタンイベントから呼び出される
    /// </summary>
    /// <param name="panelName">呼び出すためのパネルの名前</param>
    public void PlayerUIActive(string panelName)
    {
        if (UIType._mainMenuPanel == _uiType && panelName == "ItemPanel")
        {
            //時間が余ったらここをオブジェクトプールでアイテムを呼び出す

            //_itemPanel.SetActive(true);
            //もしnullだった場合中身を入れる
            if (_playerBaseCs == null) 
            {
                _playerBaseCs = _gameManager.PlayerObj.GetComponent<PlayerStatus>();
            }

            GameObject ItemButtonIns;
            //持っているアイテムの生成
            foreach (var item in _playerBaseCs.PlayerItemList) 
            {
                ItemButtonIns = Instantiate(_itemButtonPrefab, _itemContent.transform);

                ItemButtonIns.transform.Find("ItemName").GetComponent<Text>().text = item.GetItemName;
                ItemButtonIns.transform.Find("ItemImage").GetComponent<Image>().sprite = item.GetItemImage;
                //ボタンイベントを追加する
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
