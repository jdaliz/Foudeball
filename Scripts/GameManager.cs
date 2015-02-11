using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Advertisements;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;


public class GameManager : MonoBehaviour
{

  public enum StateType
  {
    DEFAULT = 0,
    WAITING,
    STARTTURN,
    PLAYING,
    RETURNTOMAIN,
    MAINMENU,
    RESTART,
    GAMEOVER,
    GAMESTART,
    MENU,
    RETURNTOPLAY,
    OPTIONS
  };

  public enum ArchivementNum
  {
    Archivement_1 = 0,
    Archivement_2,
    Archivement_3,
    Archivement_4,
    Archivement_5,
  };

  public float fMusicVolume = 1f;
  public float fEffectVolume = 1f;

  public AudioSource MusicSource;
  public AudioSource EffectSource;
  public AudioSource IntroSource;


  public float fDelayBeforeStart = 2f;

  private static GameManager instance;


  public StateType GameState;
  public StateType NewGameState;

  private float StateTransitionCounter;

  public GameObject MenuPanel;

  public string Name;
  public int score;
  public int id;

  ScorePlayer scp;

  public GameObject PanelFlashInfos;

  public Text txtScore;

  public int PlayerLive = 3;

  public GameObject LiveObject;
  private Image LiveImg;

  public Text FlashInfoTxt;

  float AdsDisplayCounter;

  public GameObject GoogSignIn;
  public GameObject GoogSignOut;

  List<string> mArchiveList = new List<string>() 
  {
    "CgkIgNa-jLgaEAIQAQ",
    "CgkIgNa-jLgaEAIQAg",
    "CgkIgNa-jLgaEAIQAw",
    "CgkIgNa-jLgaEAIQBA",
    "CgkIgNa-jLgaEAIQBQ"
  };

  public bool bEnableLeaderboard = true;

#if UNITY_EDITOR
  string gameId = @"131625197";
#elif UNITY_ANDROID
    string gameId = @"131625197";
#elif UNITY_IOS
    string gameId = @"131625197";
#else 
    string gameId = @"131625197";
#endif

  /// <summary>
  /// 
  /// </summary>
  private GameManager()
  {
    // initialize your game manager here. Do not reference to GameObjects here (i.e. GameObject.Find etc.)
    // because the game manager will be created before the objects
  }

  void Awake()
  {
    if (Application.loadedLevelName == "MainMenu")
    {
      if (Advertisement.isSupported)
      {
        Advertisement.allowPrecache = true;
        Advertisement.Initialize(gameId);
      }
      else
      {
        Debug.Log("Platform not supported");
      }
    }
  }

  /// <summary>
  /// 
  /// </summary>
  public static GameManager Instance
  {
    get
    {
      if (instance == null)
      {
        instance = FindObjectOfType<GameManager>();
      }

      return instance;
    }
  }

  // Use this for initialization
  void Start()
  {
    if (PlayerPrefs.HasKey("Foudeball_GlobalVolume"))
      fMusicVolume = PlayerPrefs.GetFloat("Foudeball_GlobalVolume");
    else
      fMusicVolume = 1f;

    GetComponent<AudioSource>().mute = false;
    AudioListener.volume = fMusicVolume;

    id = PlayerPrefs.GetInt("CurrentPlayer");

    scp = new ScorePlayer(id, 0, PlayerPrefs.GetString((id + 2).ToString()));
    //GameState = StateType.RETURNTOPLAY;
    Name = scp.NamePlayer;
    id = scp.IDPlayer;
    if (PanelFlashInfos != null)
      PanelFlashInfos.SetActive(true);

    PlayerLive = 3;
    if (LiveObject != null)
      LiveImg = LiveObject.GetComponent<Image>();


    AdsDisplayCounter = 15f;

#if UNITY_EDITOR
    // recommended for debugging:
    PlayGamesPlatform.DebugLogEnabled = true;
#endif
    // Activate the Google Play Games platform
    PlayGamesPlatform.Activate();

    if ((GoogSignOut != null) && (GoogSignIn != null))
    {
      if (Social.localUser.authenticated)
      {

        GoogSignIn.SetActive(false);
        //GoogSignOut.SetActive(true);
      }
      else
      {
        GoogSignIn.SetActive(true);
        //GoogSignOut.SetActive(false);

      }
    }
    else
    {

    }
  }

  /// <summary>
  /// 
  /// </summary>
  void FixedUpdate()
  {

  }

  // Update is called once per frame
  void Update()
  {

    scp.Score = score;
    if (txtScore != null)
      txtScore.text = score.ToString();
    if (Application.loadedLevelName == "MainMenu")
    {
      if (AdsDisplayCounter > 0)
      {
        AdsDisplayCounter -= Time.deltaTime;
      }
      else
      {
        if (Advertisement.isReady())
        {
          AdsDisplayCounter = 30f;
          // Show with default zone, pause engine and print result to debug log
          Advertisement.Show(null, new ShowOptions
          {
            pause = true,
            resultCallback = result =>
            {
              Debug.Log(result.ToString());
            }
          });
        }
      }
    }

    switch (GameState)
    {

      case StateType.PLAYING:
      {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
          GameState = StateType.MENU;
          MenuPanel.SetActive(true);

        }
      }
      break;

      case StateType.MENU:
      {
        if (GetComponent<LimitColision>().MenuCanBeDisplayed)
          MenuPanel.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
          GetComponent<LimitColision>().ReturnToPlay();
          GameState = StateType.RETURNTOPLAY;
          MenuPanel.SetActive(false);
        }
      }
      break;

      case StateType.RETURNTOPLAY:
      {
        if (PlayerLive > 0)
        {

          StateTransitionCounter = 2f;
          GameState = StateType.WAITING;
          NewGameState = StateType.PLAYING;
        }
        else
        {
          Application.LoadLevel(Application.loadedLevel);
        }
      }
      break;

      case StateType.GAMEOVER:
      {
        if (StateTransitionCounter > 0)
        {
          StateTransitionCounter -= Time.deltaTime;
        }
        else
        {
          StateTransitionCounter = 0;
          FindObjectOfType<LimitColision>().StartFadeIn();
          GameState = StateType.MENU;
        }

      }
      break;

      case StateType.RETURNTOMAIN:
      {
        StateTransitionCounter = 0.5f;
        GameState = StateType.WAITING;
        NewGameState = StateType.MAINMENU;
        MenuPanel.SetActive(false);
      }
      break;


      case StateType.MAINMENU:
      {



        Application.LoadLevel("MainMenu");
      }
      break;


      case StateType.GAMESTART:
      {
        StateTransitionCounter = 1f;
        GameState = StateType.WAITING;
        NewGameState = StateType.RESTART;
        MenuPanel.SetActive(false);
      }
      break;

      case StateType.RESTART:
      {
        RestartGame();
      }
      break;


      case StateType.WAITING:
      {
        if (StateTransitionCounter > 0)
        {
          StateTransitionCounter -= Time.deltaTime;
        }
        else
        {
          StateTransitionCounter = 0;
          GameState = NewGameState;

          if (NewGameState == StateType.PLAYING)
          {
            MenuPanel.SetActive(false);
            PanelFlashInfos.SetActive(false);
          }
        }
      }
      break;
    }
  }


  public void SetGameState(string state)
  {
    GameState = (StateType)Enum.Parse(typeof(StateType), state);
  }

  public void RestartGame()
  {
    Application.LoadLevel(Application.loadedLevel);
  }

  public void HitPlayer()
  {
    if (PlayerLive > 0)
    {
      PlayerLive--;
      LiveImg.fillAmount = PlayerLive / 3f;
    }
    else
    {
      GameState = StateType.GAMEOVER;
      FlashInfoTxt.text = "Game Over..";
      if (PanelFlashInfos != null)
        PanelFlashInfos.SetActive(true);

      if (score > 0)
      {

        GetComponent<ScoreManager>().SaveScore(scp);

        IncrementArchivement((int)GameManager.ArchivementNum.Archivement_1, score);
        IncrementArchivement((int)GameManager.ArchivementNum.Archivement_2, score);
        IncrementArchivement((int)GameManager.ArchivementNum.Archivement_3, score);
        IncrementArchivement((int)GameManager.ArchivementNum.Archivement_4, score);
        IncrementArchivement((int)GameManager.ArchivementNum.Archivement_5, score);

        Social.ReportScore(score, "CgkIgNa-jLgaEAIQBg", (bool success) =>
        {
          // handle success or failure
        });
      }


      StateTransitionCounter = 2f;

    }
  }

  public void GooglePlusLogIn()
  {

      Social.localUser.Authenticate((bool success) =>
      {
        // handle success or failure
        if (success)
        {
          bEnableLeaderboard = true;
          PlayerPrefs.SetString("FoudeballGoogle", Social.localUser.userName.Substring(0, 6));
        }
        else
        {
          bEnableLeaderboard = false;
          PlayerPrefs.DeleteKey("FoudeballGoogle");
        }
      });


  }

  public void ShowHighscores()
  {
    if (bEnableLeaderboard)
    {
      Social.ShowLeaderboardUI();
    }
    else
    {
      Application.LoadLevel("Highscores");
    }
  }

  public void IncrementArchivement(int Number, int i)
  {
    int iIndex = (int)(Enum.Parse(typeof(ArchivementNum), Number.ToString()));
    if (Social.localUser.authenticated)
    {
      Social.ReportProgress(mArchiveList[iIndex], 1, (bool success) =>
      {

      });
    }

  }

  public void GooglePlusLogOut()
  {
    PlayerPrefs.DeleteKey("FoudeballGoogle");
    if (bEnableLeaderboard)
    {
      if ((GoogSignOut != null) && (GoogSignIn != null))
      {
        GoogSignIn.SetActive(true);
        //GoogSignOut.SetActive(false);
        bEnableLeaderboard = false;
        
      }
      ((PlayGamesPlatform)Social.Active).SignOut();
    }
  }

  public void ShowAchivement()
  {
    if (bEnableLeaderboard)
    {
      Social.ShowAchievementsUI();
    }
  }
}
