using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LimitColision : MonoBehaviour {

  public Image FadeImg;
  float fadervalue;

  bool ExitGame;

  bool FadeIn;
  bool FadeOut;
  bool RequestExit;
  Texture2D tex2D;

  public Image OptionImage;

  //public GameObject can;

  public int UserAction;

  public bool MenuCanBeDisplayed;

  bool MaskAll = false;

  void Start ()
  {

    fadervalue = 2f;
    StartFadeOut();
    ExitGame = false;
    MenuCanBeDisplayed = false;

    OptionImage.enabled = false;
    //can.se = true;
    UserAction = 0;

  }


	// Update is called once per frame
	void Update () 
  {
	  if (FadeOut)
    {
      if (fadervalue>0)
        fadervalue -= Time.deltaTime;
      else
        FadeOut = false;

      FadeImg.color = new Color(1f,1f,1f,fadervalue);
      AudioListener.volume = 1f-fadervalue;
    }
    else if (FadeIn)
    {
      if (fadervalue<1f)
        fadervalue += Time.deltaTime;
      else
        FadeIn = false;

      FadeImg.color = new Color(1f,1f,1f,fadervalue);
      AudioListener.volume = 1f-fadervalue;
    }

    if (!MaskAll)
    {
      if (OptionImage.enabled)
        OptionImage.material.SetFloat("_InvFade", fadervalue * 0.95F);
    }


	}

  IEnumerator UploadPNG()
  {
    
    yield return new WaitForEndOfFrame();
    int width = Screen.width;
    int height = Screen.height;
    tex2D = new Texture2D(width, height, TextureFormat.RGB24, false);
    tex2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
    tex2D.Apply();
    OptionImage.sprite = Sprite.Create(tex2D, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
    OptionImage.material.SetFloat("_Flou", 1F);
    OptionImage.enabled = true;
  }

  void OnGUI()
  {
    if ( (GameManager.Instance.GameState == GameManager.StateType.MENU) && (RequestExit == false) )
    {
      
      //StopAllCoroutines();
      StartCoroutine("UploadPNG");
      StartCoroutine("ExitApplication");
      FadeImg.enabled = false;
      RequestExit = true;
    }



    if (RequestExit)
    {

      if (UserAction == 2)
      {
        UserAction = 0;
        StartCoroutine("ResumeApplication");

      }
      else if (UserAction == 1)
      {
        Application.Quit();
      }
    }

  }
  

  IEnumerator WaitEnding ()
  {
    StartFadeIn();
    for (float i = 0.25f; i > 0; i -= Time.deltaTime)
    {
      yield return null;
    }
    Application.LoadLevel(0);
  }

  IEnumerator ExitApplication ()
  {
    StartFadeIn();
    for (float i = 1f; i > 0; i -= Time.deltaTime)
    {
      yield return null;
    }
    MenuCanBeDisplayed = true;
    //can.active = true;
  }

  IEnumerator ResumeApplication()
  {
    StartFadeOut();
    for (float i = 1f; i > 0; i -= Time.fixedDeltaTime)
    {
      yield return null;
    }
    RequestExit = false;
    OptionImage.enabled = false;
    MenuCanBeDisplayed = false;
    //can.active = false;
  }


  public void StartFadeIn()
  {
    FadeImg.enabled = true;
    FadeImg.color = new Color(1f, 1f, 1f, 0);
    FadeIn = true;
    FadeOut = false;
    fadervalue = 0f;
    //MaskAll = true;
  }

  public void StartFadeOut()
  {
    FadeIn = false;
    FadeOut = true;
    fadervalue = 1f;
    
  }

  public void QuitApplication()
  {
    UserAction = 1;
  }

  public void ReturnToPlay()
  {
    UserAction = 2;
  }

}
