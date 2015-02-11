using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

  public List<ScorePlayer> ListDesScores;

  // Use this for initialization
  void Start()
  {
    ListDesScores = new List<ScorePlayer>();


    int cpt = 1;
    while (PlayerPrefs.HasKey(cpt.ToString()))
    {
      string name = PlayerPrefs.GetString((cpt + 2).ToString());

      ListDesScores.Add(new ScorePlayer(PlayerPrefs.GetInt((cpt).ToString()), PlayerPrefs.GetInt((cpt + 1).ToString()), PlayerPrefs.GetString((cpt + 2).ToString())));
      cpt = cpt + 3;
    }
    if (ListDesScores.Count <= 0)
      return;
    cpt = 1;
    var ScoreTrier = ListDesScores.OrderByDescending(v => v.Score);
    
    foreach (ScorePlayer unScore in ScoreTrier)
    {
      if (cpt > 8)
        break;
      if (GameObject.Find("Player" + cpt) != null)
      {
        var GUILevel = GameObject.Find("Player" + cpt).GetComponent<Text>();
        if (GUILevel != null)
          GUILevel.text = unScore.NamePlayer + "".PadLeft(16 - unScore.NamePlayer.Length, '-') + unScore.Score;
      }
      cpt++;
    }

  }

  // Update is called once per frame
  void Update()
  {

  }


  public void SaveScore(ScorePlayer sp)
  {


    PlayerPrefs.SetInt(sp.IDPlayer.ToString(), sp.IDPlayer);
    PlayerPrefs.SetInt((sp.IDPlayer + 1).ToString(), sp.Score);
    PlayerPrefs.SetString((sp.IDPlayer + 2).ToString(), sp.NamePlayer);


  }
}
