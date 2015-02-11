using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class ScorePlayer
{
  public int IDPlayer;
  public int Score;
  public string NamePlayer;

  public ScorePlayer(int id, int score, string player)
  {
    IDPlayer = id;
    NamePlayer = player;
    Score = score;
  }

  public int GetIdScorePlayer()
  {
    return IDPlayer + 2;
  }
  public int GetIdNamePlayer()
  {
    return IDPlayer + 1;
  }


}


