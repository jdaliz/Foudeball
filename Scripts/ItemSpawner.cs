using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{

  /// <summary>
  /// 
  /// </summary>
  public List<GameObject> iArrivingItem;

  private int NbItem;
  private float SenderCounter;

  public float reducetimer;



  // Use this for initialization
  void Start()
  {
    if (iArrivingItem != null)
      NbItem = iArrivingItem.Count;
    reducetimer = (1f - Time.realtimeSinceStartup * 0.001f)-1;
    SenderCounter = Random.Range(4f, 7f);
  }

  // Update is called once per frame
  void Update()
  {
    if (GameManager.Instance.GameState != GameManager.StateType.PLAYING)
      return;

    reducetimer = (1f - Time.realtimeSinceStartup * 0.001f);

    if (SenderCounter > 0)
    {
      SenderCounter -= Time.deltaTime;
    }
    else
    {
      SenderCounter = Random.Range(0.75f, 2f) * reducetimer;

      int nbItem = Random.Range(1, 3);
      for (int i=0;i<nbItem;i++)
      SendMobs();
    }
  }

  public void SendMobs()
  {
    float angle = Random.Range(160f, 380f);
    Vector2 StarPoint = new Vector2((Screen.width / 2f) + Mathf.Cos((3.14f / 180f) * angle) * Screen.width * 2f,
      (Screen.height / 2f) + Mathf.Sin((3.14f / 180f) * angle) * Screen.width * 2f);

    Vector3 v3 = Camera.main.ScreenToWorldPoint(StarPoint);
    GameObject obj = iArrivingItem[Random.Range(0, NbItem)];
    if ((iArrivingItem != null)&& (iArrivingItem.Count>0) )
      Instantiate(obj, v3, Quaternion.identity);
  }
}
