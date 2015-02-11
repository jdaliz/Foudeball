using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using System.Collections;

public class PlayerManager : MonoBehaviour
{

  public Text DebugText;

  public float fTimeBeforeAdvertissing = 5f;

  float AdvertissingCounter;

  public GameObject HandSpriteL;
  public GameObject HandSpriteR;

  public AudioSource AudSrc;
  public AudioSource AudSrcCrie;

  bool bPlaySound = false;

  float bSoundTimer = 0.5f;

  float fHitTimer = 0;

  int fAdsCounter = 3;

  Animator mAnim;

#if UNITY_EDITOR
  string gameId = @"131625197";
#elif UNITY_ANDROID
    string gameId = @"131625197";
#elif UNITY_IOS
    string gameId = @"131625197";
#else 
    string gameId = @"131625197";
#endif

  void Awake()
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

  void OnGUI()
  {



  }

  // Use this for initialization
  void Start()
  {
    AdvertissingCounter = 0.5f;

    mAnim = GetComponent<Animator>();
    if (!PlayerPrefs.HasKey("AdsCounter"))
      PlayerPrefs.SetInt("AdsCounter", 5);
    fAdsCounter = PlayerPrefs.GetInt("AdsCounter");
  }

  // Update is called once per frame
  void Update()
  {

    if ((GameManager.Instance.GameState == GameManager.StateType.MENU) ||
     (GameManager.Instance.GameState == GameManager.StateType.GAMEOVER))
    {
      
      if (AdvertissingCounter > 0)
      {
        AdvertissingCounter -= Time.deltaTime;
      }
      else
      {

        AdvertissingCounter = fTimeBeforeAdvertissing;

        if (fAdsCounter > 0)
        {
          fAdsCounter--;
          PlayerPrefs.SetInt("AdsCounter", fAdsCounter);
        }
        else
        {
          if (Advertisement.isReady())
          {
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

          fAdsCounter = 5;
          PlayerPrefs.SetInt("AdsCounter", fAdsCounter);
        }
      }
    }
    else if (AdvertissingCounter != 0)
    {
      AdvertissingCounter = 3f;
    }

    if (GameManager.Instance.GameState == GameManager.StateType.RETURNTOMAIN)
    {
      if (AdvertissingCounter != 1f)
        AdvertissingCounter = 1f;
    }

    if (bSoundTimer > 0)
    {
      bSoundTimer -= Time.deltaTime;
    }
    else
    {
      bPlaySound = false;
    }

    if (fHitTimer > 0)
    {
      fHitTimer -= Time.deltaTime;
    }
#if UNITY_EDITOR



    if (Input.GetMouseButton(0) && (GameManager.Instance.GameState == GameManager.StateType.PLAYING))
    {

      if (fHitTimer <= 0)
      {
        fHitTimer = 0.25f;
        if (bPlaySound == false)
        {
          AudSrcCrie.Play();
          bSoundTimer = 0.25f;
          bPlaySound = true;
          mAnim.SetTrigger("Hit");
        }


        Vector2 v2 = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
        Vector2 v4 = Input.mousePosition;

        Vector3 vHand = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vHand.z = 0;




        Collider2D coll = NoWall(Input.mousePosition);

        if (coll != null)
        {
          string name = coll.name.ToLower();

          if ((name.IndexOf("bad") >= 0) || (name.IndexOf("good") >= 0))
          {

            if (name.IndexOf("good") >= 0)
            {
              GameManager.Instance.score += 10;

            }
            else
              GameManager.Instance.HitPlayer();

            coll.gameObject.GetComponent<Rigidbody2D>().gravityScale = 8f;
            Destroy(coll.gameObject, 2f);
            if (vHand.x>=0)
              Destroy(Instantiate(HandSpriteL, vHand, TurnFromVector(v2, v4)),0.5f);
            else
              Destroy(Instantiate(HandSpriteR, vHand, TurnFromVector(v2, v4)), 0.5f);

            if (fHitTimer == 0.25f)
            {
              fHitTimer -= Time.deltaTime;
              AudSrc.Play();
              
            }

          }
          else if (name.IndexOf("touch") < 0)
          {
            if (vHand.x >= 0)
              Destroy(Instantiate(HandSpriteL, vHand, TurnFromVector(v2, v4)), 0.5f);
            else
              Destroy(Instantiate(HandSpriteR, vHand, TurnFromVector(v2, v4)), 0.5f);

          }
          else
          {
            AudSrcCrie.Stop();
          }

        }
        else
        {
          if (vHand.x >= 0)
            Destroy(Instantiate(HandSpriteL, vHand, TurnFromVector(v2, v4)), 0.5f);
          else
            Destroy(Instantiate(HandSpriteR, vHand, TurnFromVector(v2, v4)), 0.5f);
        }
      }
      else
      {
        fHitTimer -= Time.deltaTime;
      }
    }
#else

    if ((Input.touchCount>0) && (GameManager.Instance.GameState == GameManager.StateType.PLAYING))
    {

      if (fHitTimer <= 0)
      {
        fHitTimer = 0.25f;
        if (bPlaySound == false)
        {
          AudSrcCrie.Play();
          bSoundTimer = 0.25f;
          bPlaySound = true;
          mAnim.SetTrigger("Hit");
        }


        Vector2 v2 = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
        Vector2 v4 = Input.touches[0].position;

        Vector3 vHand = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
        vHand.z = 0;




        Collider2D coll = NoWall(Input.touches[0].position);

        if (coll != null)
        {
          string name = coll.name.ToLower();

          if ((name.IndexOf("bad") >= 0) || (name.IndexOf("good") >= 0))
          {

            if (name.IndexOf("good") >= 0)
              GameManager.Instance.score += 10;
            else
              GameManager.Instance.HitPlayer();

            coll.gameObject.GetComponent<Rigidbody2D>().gravityScale = 8f;
            Destroy(coll.gameObject, 2f);
            if (vHand.x>=0)
              Destroy(Instantiate(HandSpriteL, vHand, TurnFromVector(v2, v4)),0.5f);
            else
              Destroy(Instantiate(HandSpriteR, vHand, TurnFromVector(v2, v4)), 0.5f);

            if (fHitTimer == 0.25f)
            {
              fHitTimer -= Time.deltaTime;
              AudSrc.Play();
              
            }

          }
          else if (name.IndexOf("touch") < 0)
          {
            if (vHand.x >= 0)
              Destroy(Instantiate(HandSpriteL, vHand, TurnFromVector(v2, v4)), 0.5f);
            else
              Destroy(Instantiate(HandSpriteR, vHand, TurnFromVector(v2, v4)), 0.5f);

          }
          else
          {
            AudSrcCrie.Stop();
          }

        }
        else
        {
          if (vHand.x >= 0)
            Destroy(Instantiate(HandSpriteL, vHand, TurnFromVector(v2, v4)), 0.5f);
          else
            Destroy(Instantiate(HandSpriteR, vHand, TurnFromVector(v2, v4)), 0.5f);
        }
      }
      else
      {
        fHitTimer -= Time.deltaTime;
      }
    }

#endif
  }






  /// <summary>
  /// 
  /// </summary>
  /// <param name="coll"></param>
  void OnTriggerEnter2D(Collider2D coll)
  {

  }


  Collider2D NoWall(Vector2 TargetPos)
  {

    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(TargetPos);
    Collider2D lColid = Physics2D.OverlapPoint(mousePosition, 1 << 9);


    return lColid;

  }


  /// <summary>
  /// 
  /// </summary>
  /// <param name="a"></param>
  /// <param name="b"></param>
  /// <returns></returns>
  public Quaternion TurnFromVector(Vector2 v2, Vector2 v4)
  {
    Vector3 v10 = new Vector3(v2.x, v2.y, 0f);

    Vector3 v20 = new Vector3(v4.x, v4.y, 0f);

    Vector3 dir = v20 - v10;
    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    return Quaternion.AngleAxis(angle - 90f, Vector3.forward);
  }

}
