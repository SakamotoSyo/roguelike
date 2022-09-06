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
        ShowText,
        StairPanel,

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

    [SerializeField, Header("StairPanelのObject")]
    private GameObject _stairPanel;

    [SerializeField, Header("ItemInformationのText")]
    private Text _itemInfoText;

    [SerializeField, Header("MainPanelのCanvasGroup")]
    private CanvasGroup _mainPanelCanvasGroup;

    [SerializeField, Header("ItemPanelのCanvasGroup")]
    private CanvasGroup _itemGroupCanvasGroup;

    [SerializeField, Header("ItemButtonのPrefab")]
    private GameObject _itemButtonPrefab;

    [SerializeField, Header("ItemObjectのPrefab")]
    private GameObject _itemObjectPrehab;

    [Header("吹き飛ばしの杖を振った時に生成されるObj")]
    [SerializeField] GameObject _blowObj;

    [Header("雷の石を使ったときに生成されるObj")]
    [SerializeField] GameObject _thunderStone;

    [Tooltip("PlayerStatusのスクリプト")]
    private PlayerStatus _playerStatusCs;

    [Tooltip("PlayerBaseのスクリプト")]
    private PlayerMove _playerMoveCs;


    [Tooltip("ゲームマネージャー")]
    private GameManager _gameManager;

    [Tooltip("ダンジョン生成")]
    private DgGenerator _dgGenerator;

    private float _testPosition;

    /// <summary>現在のUIType</summary>
    private UIType _uiType;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        _dgGenerator = DgGenerator.Instance;

        //最初にあらかじめ持ち物の上限分のボタンを作成
        for (int i = 0; i > 20; i++)
        {
            var a = Instantiate(_itemButtonPrefab, _itemContent.transform);
        }
        //_playerBaseCs = _gameManager.PlayerObj.GetComponent<PlayerBase>();
    }

    // Update is called once per frame
    void Update()
    {

        if (_gameManager.TurnType == GameManager.TurnManager.Player && _uiType == UIType.Normal)
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
            else if (_uiType == UIType.ItemInfomationPanel)
            {
                _itemInforPanel.SetActive(false);
                _gameManager.TurnType = GameManager.TurnManager.Player;
                _uiType = UIType.Normal;
            }
            else if (_uiType == UIType.StairPanel)
            {
                _stairPanel.SetActive(false);
                _gameManager.TurnType = GameManager.TurnManager.Player;
                _uiType = UIType.Normal;
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
        if (item.GetItemType == Item.ItemType.MagicBook)
        {
            ItemSelectButton.GetComponentInChildren<Text>().text = "読む";
            ItemSelectButton.GetComponent<Button>().onClick.AddListener(() => UseSelectItem(item));

            var ItemSelectButton2 = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
            ItemSelectButton2.GetComponentInChildren<Text>().text = "投げる";
            ItemSelectButton2.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(ThrowItem(item)));

            var ItemSelectButton3 = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
            ItemSelectButton3.GetComponentInChildren<Text>().text = "置く";
            ItemSelectButton3.GetComponent<Button>().onClick.AddListener(() => ItemPut(item));

            var ItemSelectButton4 = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
            ItemSelectButton4.GetComponentInChildren<Text>().text = "説明";
            ItemSelectButton4.GetComponent<Button>().onClick.AddListener(() => ItemExplanation(item));
        }
        else if (item.GetItemType == Item.ItemType.SpecialItem)
        {
            ItemSelectButton.GetComponentInChildren<Text>().text = "振る";
            ItemSelectButton.GetComponent<Button>().onClick.AddListener(() => UseSelectItem(item));

            var ItemSelectButton2 = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
            ItemSelectButton2.GetComponentInChildren<Text>().text = "投げる";
            ItemSelectButton2.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(ThrowItem(item)));

            var ItemSelectButton3 = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
            ItemSelectButton3.GetComponentInChildren<Text>().text = "置く";
            ItemSelectButton3.GetComponent<Button>().onClick.AddListener(() => ItemPut(item));

            var ItemSelectButton4 = Instantiate(_useItemButtonPrehab, _useItemSelectPanel.transform);
            ItemSelectButton4.GetComponentInChildren<Text>().text = "説明";
            ItemSelectButton4.GetComponent<Button>().onClick.AddListener(() => ItemExplanation(item));
        }

        _useItemSelectPanel.SetActive(true);
        Debug.Log("Select");
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
        _playerStatusCs.RemoveItem(item);
        if (item.GetEffectType == Item.ItemEffectType.Hearing)
        {
            _playerStatusCs.SetHp(item.GetItemEffect);

            ShowText($"{item.GetItemEffect}回復しました");
        }
        else if (item.GetEffectType == Item.ItemEffectType.Food)
        {

        }
        else if (item.GetEffectType == Item.ItemEffectType.Special)
        {
            if (item.GetItemName == "ワープの杖")
            {
                //プレイヤーをランダムにワープする
                _dgGenerator.PlayerRespawn();
                ResetMenu();
            }
            else if (item.GetItemName == "吹き飛ばしの杖")
            {
                //敵に当たると吹き飛ばすObjectを生成
                Instantiate(_blowObj, new Vector2(_gameManager.PlayerX, _gameManager.PlayerY * -1), transform.rotation);
                ResetMenu(true);
            }
            else if (item.GetItemName == "雷の石") 
            {
                Instantiate(_thunderStone, new Vector2(_gameManager.PlayerX, _gameManager.PlayerY * -1), transform.rotation);
                ResetMenu(true);
            }
        }

    }

    /// <summary>
    /// アイテムを投げるメソッド
    /// </summary>
    /// <param name="item"></param>
    public IEnumerator ThrowItem(Item item)
    {
        //アイテムを生成する
        var Item = Instantiate(_itemObjectPrehab, _gameManager.PlayerObj.transform.position, _gameManager.PlayerObj.transform.rotation);
        var ItemObjectCs = Item.GetComponent<ItemObjectScript>();

        //アイテムに情報を設定
        ItemObjectCs.SetItemInfor(item);
        ItemObjectCs.SetItemSprite(item.GetItemImage);
        _playerMoveCs = _gameManager.PlayerObj.GetComponent<PlayerMove>();
        //プレイヤーの動いた方向を持ってくる
        int x = (int)_playerMoveCs.PlayerDirection.x;
        int y = (int)_playerMoveCs.PlayerDirection.y;

        if (x == 0 && y == 0)
        {
            y = -1;
        }

        //アイテムがが壁の座標まで飛び続ける
        while (_dgGenerator.Layer.GetMapData(_gameManager.PlayerX + x, _gameManager.PlayerY + y * -1) == 1)
        {
            x += Math.Sign(x);
            y += Math.Sign(y);
        }

        x += Math.Sign(x) * -1;
        y += Math.Sign(y) * -1;

        //移動する次の場所
        Vector3 _nextPosition = new Vector3(_gameManager.PlayerX + x, _gameManager.PlayerY * -1 + y, 0);
        //今の場所から目的地までの距離
        //var _distance_Two = Vector3.Distance(Item.transform.position, _nextPosition);
        //配列にアイテムの場所をセットする
        _dgGenerator.Layer.SetData(_gameManager.PlayerX + x, _gameManager.PlayerY + y * -1, 3);
        Debug.Log(_dgGenerator.Layer.GetMapData(_gameManager.PlayerX + x, _gameManager.PlayerY + y * -1));
        //移動処理
        StartCoroutine(ItemThrowMove(Item, _nextPosition));

        yield return new WaitForSeconds(0.08f);
        //アイテムのオブジェクトをゲームマネージャーに渡す
        _gameManager.SetItemObjList(Item);

        _playerStatusCs.RemoveItem(item);

        ResetMenu();
    }

    /// <summary>
    /// アイテムを指定の場所まで移動させる
    /// </summary>
    /// <param name="Item"></param>
    /// <param name="_nextPosition"></param>
    private IEnumerator ItemThrowMove(GameObject Item, Vector3 _nextPosition)
    {
        while (Item.transform.position != _nextPosition)
        {
            //ここがスピード
            _testPosition += 0.05f;
            //移動処理
            Item.transform.position = Vector3.Lerp(_gameManager.PlayerObj.transform.position, _nextPosition, _testPosition);
            yield return new WaitForSeconds(0.01f);
        }

        _testPosition = 0;
    }


    /// <summary>
    /// アイテムをその場に置く
    /// </summary>
    /// <param name="item"></param>
    private void ItemPut(Item item)
    {
        //アイテムの生成
        var Item = Instantiate(_itemObjectPrehab, _gameManager.PlayerObj.transform.position, _gameManager.PlayerObj.transform.rotation);
        var ItemObjectCs = Item.GetComponent<ItemObjectScript>();

        //アイテムに情報を渡す
        ItemObjectCs.SetItemInfor(item);
        ItemObjectCs.SetItemSprite(item.GetItemImage);
        _playerMoveCs = _gameManager.PlayerObj.GetComponent<PlayerMove>();
        //アイテムのオブジェクトをゲームマネージャーに渡す
        _gameManager.SetItemObjList(Item);
        //配列にアイテムの場所をSetする
        _dgGenerator.Layer.SetData((int)_gameManager.PlayerObj.transform.position.x, (int)_gameManager.PlayerObj.transform.position.y, 3);
        //プレイヤーのアイテム欄からその場に置いたアイテムを消去する
        _playerStatusCs.RemoveItem(item);

        ResetMenu();
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
    private void ResetMenu(bool specialItem = false)
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
        if (specialItem)
        {
            
        }
        else 
        {
            _gameManager.TurnType = GameManager.TurnManager.Player;
        }
    }

    /// <summary>
    /// 引数に入れたものをTextで表示する
    /// </summary>
    private void ShowText(string st)
    {
        ResetMenu();
        _gameManager.TurnType = GameManager.TurnManager.MenuOpen;
        _uiType = UIType.ItemInfomationPanel;

        _itemInforPanel.SetActive(true);
        _itemInfoText.text = st;
    }

    /// <summary>階段に関するUIを表示させる処理</summary>
    public void StairUI()
    {

        _uiType = UIType.StairPanel;

        _stairPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_stairPanel.transform.GetChild(0).gameObject);
    }
}
