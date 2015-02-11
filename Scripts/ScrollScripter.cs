using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollScripter : MonoBehaviour
{

  public Scrollbar scrbar;

  // Use this for initialization
  void Start()
  {
    scrbar.value = 1;
  }

  // Update is called once per frame
  void Update()
  {
    scrbar.value -= Time.deltaTime*0.01f;

    if (scrbar.value == 0)
      scrbar.value = 1;
  }

  public void OpenURL(string text)
  {
    Application.OpenURL(text);
  }
}
