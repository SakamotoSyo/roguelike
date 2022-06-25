using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

//  GSSReader を追加している GameObject にのみ AddComponent できます。
[RequireComponent(typeof(GSSReader))]
//  クラス名はなんでもOKです。
//  読み込むデータに合わせて適切なクラス名に変更してください。
//  MonoBehaviourを継承している場合、スクリプトファイル名と
//  クラス名を合わせる必要があるので、クラス名を変更したら
//  スクリプトファイル名も変更してください。
public class Chapter1Script : MonoBehaviour
{
    [SerializeField] private GameObject ShopingC;

    [SerializeField] private GameObject CommandCanvas;
    [SerializeField] private GameObject ChatPanelCanvas;
    [SerializeField] private GameObject ChapterCanvas;
    [SerializeField] private GameObject BackGroundCanvas;
    [SerializeField] private GameObject Chapter0Prefab;

    [SerializeField] private Text ChatText;
    [SerializeField] private Text CharacterText;

    [SerializeField] private Image BackGround;

    [SerializeField] private AudioClip[] clips;


    private GameObject ImageInstance;

    private int Name = 0;
    private int CharacterTalk = 0;
    private int BackInt = 0;
    private int SceneChangeInt = 0;

    private float waitTime = 0.6f;
    [SerializeField]private float TextSpeed = 0.3f;
    private float elapsedTime = 0f;

    private string Back;
    private string SceneChange;
    private string SE;
    private string[] splitMessage = new string[10];

    private int messageNum;
    private int nowMessage;
    private int MessageInt;
    private int intSE;

    private bool Tutorial = false;
    private bool isOneMesage = false;
    private bool StoryStart = false;
    private bool nextChat = false;
    public bool fadefinish = true;

    private AudioSource audios;

    [SerializeField] private GSSReader reader;

    public bool Chapter0 = false;


    private void Awake()
    {
        audios = GetComponent<AudioSource>();
    }

    void Start()
    {

        //splitMessage[0] = "かみまみた";
        
        if(!Tutorial)
        {
            reader.SheetID = "1o9pi7tP_KAGemLaY4qqBhx0LjkYFDR8nUFPu4c5e3Ho";
           
        }
    }

    void Update()
    {

        //reader.SheetID = "あ";
       
        //ストーリーのテキスト表示
        if (StoryStart)
        {
            //ChatStart();  
        }

        //タイトルができ次第処理を変える
        if(!Chapter0)
        {
            reader.SheetID = "1o9pi7tP_KAGemLaY4qqBhx0LjkYFDR8nUFPu4c5e3Ho";
            reader.Reload();
            Chapter0 = true;
            //ChapterManger();
            
        }


      
    }


    //  メソッド名もなんでもOKです。
    //  GSSReader の OnLoadEnd に追加することで
    //  GSS の読み込み完了時にコールバックされます。
    public void OnGSSLoadEnd()
    {
        var r = GetComponent<GSSReader>();
        var d = r.Datas;
        if (d != null)
        {
            //  d をゲームで使うデータに変換する処理をここに書きます。
            //  以下は
            /*for (var row = 0; row < d.Length; row++)
            {
                for (var col = 0; col < d[row].Length; col++)
                {
                    Debug.Log("[" + row + "][" + col + "]=" + d[row][col]);
                    Debug.Log(d);

                }
            }*/

            if(Name < d.Length )
            {
                StoryStart = true;

                CharacterText.text = d[Name][0];
                //ChatText.text = d[CharacterTalk][1];
                isOneMesage = false;
                //文章の内容をリセットする
                //ChatText.text = " ";
                //文章の入れる
                splitMessage[0] = d[CharacterTalk][1];
                Back = d[BackInt][2];
                SceneChange = d[SceneChangeInt][3];
                SE = d[intSE][4];
                //BackGroundChanger(Back);
                //PlaySE(SE);

                Name++;
                CharacterTalk++;
                BackInt++;
                SceneChangeInt++;
                intSE++;
                //SceneChanger(SceneChange);
            }
            
        }
    }

    ////背景画像の切り替え
    //public void BackGroundChanger(string b)
    //{
    //    if(b == "Moon")
    //    {
    //        Debug.Log("gami");
    //    }
    //    else if(b == "Chapter0")
    //    {
    //        Destroy(ImageInstance);
    //        ImageInstance = Instantiate<GameObject>(Chapter0Prefab);


    //    }
    //}

    ////ストーリー、シーンの切り替え
    //public void SceneChanger(string s)
    //{
    //    if(s == "Main")
    //    {
    //        StoryStart = false;
    //    }
    //    else if(s == "Chapter")
    //    {

    //    }
    //    else if (s == "Odekake_Kaimono")
    //    {
    //        StoryStart = false;
    //        ChapterManger();
    //        //ここで移動する場面を変える
    //        reader.SheetID = "1zu6nie5fxkFZfMmuaJwEf36UPfeMrmyBPK-oSmxDtlY";
    //        reader.Reload();
    //    }
    //    else if (s == "Odekake_GameCenter")
    //    {
    //        StoryStart = false;
    //        ChapterManger();
    //        //ここで移動する場面をゲームセンターに変える
    //        reader.SheetID = "";
    //        reader.Reload();
    //    }
    //    else if(s == "Odekake_Hanabi")
    //    {
    //        StoryStart = false;
    //        ChapterManger();
    //        //ここで移動する場面を花火大会に変える
    //        reader.SheetID = "";
    //        reader.Reload();
    //    }

    //    //選択肢
    //    if(s == "ShoopingC")
    //    {
    //        StoryStart = false;
    //        ChapterCanvas.SetActive(false);
    //        ShopingC.SetActive(true);



    //    }
    //}

    //private void PlaySE(string s)
    //{
    //    if(s == "Keitai")
    //    {
    //        audios.clip = clips[0];
            
    //        audios.Play();
    //    }
    //}

    //public void ChapterManger()
    //{
    //    Name = 0;
    //    CharacterTalk = 0;
    //    BackInt = 0;
    //    SceneChangeInt = 0;
    //    intSE = 0;
    //    ChapterCanvas.SetActive(false);
    //        CommandCanvas.SetActive(false);
    //        ChatPanelCanvas.SetActive(true);
        
    //}

    ////StoryStartがTrueになるとUpdateから呼ばれる
    //private void ChatStart()
    //{
    //    if (fadefinish)
    //    {
    //        if (!isOneMesage)
    //        {
    //            if (elapsedTime >= TextSpeed)
    //            {
    //                //Debug.Log(splitMessage[messageNum][nowMessage]);
    //                ChatText.text += splitMessage[messageNum][nowMessage];


    //                nowMessage++;
    //                elapsedTime = 0f;

    //                if (nowMessage >= splitMessage[messageNum].Length)
    //                {

    //                    isOneMesage = true;


    //                }
    //            }
    //            elapsedTime += Time.deltaTime;

    //            if (Input.GetMouseButtonDown(0))
    //            {
    //                ChatText.text += splitMessage[messageNum].Substring(nowMessage);
    //                isOneMesage = true;

    //            }
    //        }
    //        else
    //        {
    //            NextChat();
    //        }
    //    }
    //}

    //private void NextChat()
    //{
    //    if (isOneMesage)
    //    {
    //        waitTime += Time.deltaTime;
    //        if (waitTime >= 0.5f)
    //        {
    //            if (Input.GetMouseButtonDown(0))
    //            {
    //                messageNum = 0;
    //                nowMessage = 0;
    //                elapsedTime = 0f;
    //                ChatText.text = "";
    //                Debug.Log(nextChat);
    //                waitTime = 0f;
    //                //次の文章を読み込む
    //                reader.Reload();
    //                Debug.Log("呼ばれた");
    //                nextChat = false;

    //            }
    //        }

    //    }
    //}
}