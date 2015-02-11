using UnityEngine;
using System.Collections;

public class DestroyItems : MonoBehaviour
{

  float fDestroyTimes;

  // Use this for initialization
  void Start()
  {
    fDestroyTimes = 8f;
  }

  // Update is called once per frame
  void Update()
  {
    if (GameManager.Instance.GameState != GameManager.StateType.PLAYING)
      return;

    if (fDestroyTimes > 0)
    {
      fDestroyTimes -= Time.deltaTime;
    }
    else
      Destroy(gameObject,0.1f);
  }
}
