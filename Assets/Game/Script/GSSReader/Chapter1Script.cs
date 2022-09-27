//using UnityEngine;
//using System;
//using UnityEngine.UI;
//using System.Collections;
//using UnityEngine.SceneManagement;

////  GSSReader を追加している GameObject にのみ AddComponent できます。
//[RequireComponent(typeof(GSSReader))]
////  クラス名はなんでもOKです。
////  読み込むデータに合わせて適切なクラス名に変更してください。
////  MonoBehaviourを継承している場合、スクリプトファイル名と
////  クラス名を合わせる必要があるので、クラス名を変更したら
////  スクリプトファイル名も変更してください。
//public class Chapter1Script : MonoBehaviour
//{
//    public bool fadefinish = true;

//    [SerializeField] private GameObject ShopingC;

//    [SerializeField] private GameObject CommandCanvas;
//    [SerializeField] private GameObject ChatPanelCanvas;
//    [SerializeField] private GameObject ChapterCanvas;
//    [SerializeField] private GameObject BackGroundCanvas;
//    [SerializeField] private GameObject Chapter0Prefab;

//    [SerializeField] private Text ChatText;
//    [SerializeField] private Text CharacterText;

//    [Header("テキストが流れる速度")]
//    [SerializeField] float TextSpeed = 0.3f;

//    [SerializeField] private Image BackGround;

//    [SerializeField] private AudioClip[] clips;


//    private GameObject ImageInstance;

//    int Name = 0;
//    int CharacterTalk = 0;
//  　int SeInt = 0;
//    int SceneChangeInt = 0;

//    float waitTime = 0.6f;
   
//    float elapsedTime = 0f;

//    [Tooltip("ターンチェンジに使う変数")]
//    string SceneChange;
//    [Tooltip("SEを鳴らすために使う変数")]
//    string SE;
//    [Tooltip("分割したメッセージを格納しておく変数")]
//    string[] splitMessage = new string[10];

//    int messageNum;
//    int nowMessage;
//    int MessageInt;
//    int intSE;

//    bool isOneMesage = false;
//    bool StoryStart = false;
//    bool nextChat = false;

//    AudioSource _audio;

//    [SerializeField] private GSSReader reader;

//    public bool Chapter0 = false;


//    private void Awake()
//    {
//        _audio = GetComponent<AudioSource>();
//    }

//    void Start()
//    {

//        //splitMessage[0] = "かみまみた";
        
//        if(!Tutorial)
//        {
//            reader.SheetID = "1o9pi7tP_KAGemLaY4qqBhx0LjkYFDR8nUFPu4c5e3Ho";
           
//        }
//    }

//    void Update()
//    {

//        //reader.SheetID = "あ";
       
//        //ストーリーのテキスト表示
//        if (StoryStart)
//        {
//            //ChatStart();  
//        }

//        //タイトルができ次第処理を変える
//        if(!Chapter0)
//        {
//            reader.SheetID = "1o9pi7tP_KAGemLaY4qqBhx0LjkYFDR8nUFPu4c5e3Ho";
//            reader.Reload();
//            Chapter0 = true;
//            //ChapterManger();
            
//        }


      
//    }

//    //  GSS の読み込み完了時にコールバックされます。
//    public void OnGSSLoadEnd()
//    {
//        var r = GetComponent<GSSReader>();
//        var d = r.Datas;
//        if (d != null)
//        {
//            //  d をゲームで使うデータに変換する処理をここに書く

//            if(Name < d.Length )
//            {
//                StoryStart = true;

//                CharacterText.text = d[Name][0];
//                isOneMesage = false;
//                //文章の入れる
//                splitMessage[0] = d[CharacterTalk][1];
//                SceneChange = d[SceneChangeInt][3];
//                SE = d[intSE][4];

//                Name++;
//                CharacterTalk++;
//                SeInt++;
//                SceneChangeInt++;
//                intSE++;
//                //SceneChanger(SceneChange);
//            }
            
//        }
//    }

//    //背景画像の切り替え
//    public void BackGroundChanger(string b)
//    {
//        if (b == "Moon")
//        {
//            Debug.Log("gami");
//        }
//        else if (b == "Chapter0")
//        {
//            Destroy(ImageInstance);
//            ImageInstance = Instantiate<GameObject>(Chapter0Prefab);


//        }
//    }

//    //ストーリー、シーンの切り替え
//    public void SceneChanger(string s)
//    {
//        if (s == "Main")
//        {
//            StoryStart = false;
//        }
//        else if (s == "Chapter")
//        {

//        }
//        else if (s == "Odekake_Kaimono")
//        {
//            StoryStart = false;
//            Init();
//            //ここで移動する場面を変える
//            reader.SheetID = "1zu6nie5fxkFZfMmuaJwEf36UPfeMrmyBPK-oSmxDtlY";
//            reader.Reload();
//        }
//        else if (s == "Odekake_GameCenter")
//        {
//            StoryStart = false;
//            Init();
//            //ここで移動する場面をゲームセンターに変える
//            reader.SheetID = "";
//            reader.Reload();
//        }
//        else if (s == "Odekake_Hanabi")
//        {
//            StoryStart = false;
//            Init();
//            //ここで移動する場面を花火大会に変える
//            reader.SheetID = "";
//            reader.Reload();
//        }

//        //選択肢
//        if (s == "ShoopingC")
//        {
//            StoryStart = false;
//            ChapterCanvas.SetActive(false);
//            ShopingC.SetActive(true);



//        }
//    }

//    private void PlaySE(string s)
//    {
//        if (s == "Keitai")
//        {
//            audios.clip = clips[0];

//            audios.Play();
//        }
//    }

//    /// <summary>
//    /// 初期化処理
//    /// </summary>
//    public void Init()
//    {
//        Name = 0;
//        CharacterTalk = 0;
//        SeInt = 0;
//        SceneChangeInt = 0;
//        intSE = 0;
//        ChapterCanvas.SetActive(false);
//        CommandCanvas.SetActive(false);
//        ChatPanelCanvas.SetActive(true);

//    }

//    //StoryStartがTrueになるとUpdateから呼ばれる
//    private void ChatStart()
//    {
//        if (fadefinish)
//        {
//            if (!isOneMesage)
//            {
//                if (elapsedTime >= TextSpeed)
//                {
//                    //Debug.Log(splitMessage[messageNum][nowMessage]);
//                    ChatText.text += splitMessage[messageNum][nowMessage];


//                    nowMessage++;
//                    elapsedTime = 0f;

//                    if (nowMessage >= splitMessage[messageNum].Length)
//                    {

//                        isOneMesage = true;


//                    }
//                }
//                elapsedTime += Time.deltaTime;

//                if (Input.GetMouseButtonDown(0))
//                {
//                    ChatText.text += splitMessage[messageNum].Substring(nowMessage);
//                    isOneMesage = true;

//                }
//            }
//            else
//            {
//                NextChat();
//            }
//        }
//    }

//    private void NextChat()
//    {
//        if (isOneMesage)
//        {
//            waitTime += Time.deltaTime;
//            if (waitTime >= 0.5f)
//            {
//                if (Input.GetMouseButtonDown(0))
//                {
//                    messageNum = 0;
//                    nowMessage = 0;
//                    elapsedTime = 0f;
//                    ChatText.text = "";
//                    Debug.Log(nextChat);
//                    waitTime = 0f;
//                    //次の文章を読み込む
//                    reader.Reload();
//                    Debug.Log("呼ばれた");
//                    nextChat = false;

//                }
//            }

//        }
//    }
//}