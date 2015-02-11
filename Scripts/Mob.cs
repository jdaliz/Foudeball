using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Mob : MonoBehaviour
{
  public GameObject Ghost;
  public GameObject Explode;
  public Transform Sprite;

  public int GhostNumbers = 10;

  private List<GameObject> mSprite;

  private float fAngleSpeed = 12f;

  public float fGhostEffectTimeInMillisec = 10;
  private float fEffectTimeCounter = 0f;

  private Vector3 target;
  float time = 1f;

  // Use this for initialization
  void Start()
  {
    mSprite = new List<GameObject>();
    target = new Vector3(0,0,0);
    fGhostEffectTimeInMillisec /= 1000;

    fAngleSpeed = Random.Range(500f, 700f);

    for (int i = 0; i < GhostNumbers; i++)
    {
      mSprite.Add((GameObject)Instantiate(Ghost, transform.position, Quaternion.identity));
      mSprite[i].transform.localScale = new Vector2(((i + 1) / ((float)GhostNumbers + 1f)) * Sprite.localScale.x, (i + 1) / ((float)GhostNumbers + 1f) * Sprite.localScale.y);
    }
    time = Random.Range(2.25f,5f);
	  

  }

  // Update is called once per frame
  void Update()
  {
    if (GameManager.Instance.GameState == GameManager.StateType.PLAYING)
    {
      if (fEffectTimeCounter > 0)
      {
        fEffectTimeCounter -= Time.deltaTime;
      }
      else
      {
        if (mSprite.Count > 0)
        {
          for (int i = 0; i < (GhostNumbers - 1); i++)
          {
            mSprite[i].transform.position = mSprite[i + 1].transform.position;
            mSprite[i].transform.localRotation = mSprite[i + 1].transform.localRotation;
          }
          mSprite[GhostNumbers - 1].transform.position = transform.position;
          mSprite[GhostNumbers - 1].transform.localRotation = transform.localRotation;
        }

        fEffectTimeCounter = fGhostEffectTimeInMillisec;
      }

      transform.Rotate(0, 0, -fAngleSpeed * Time.deltaTime);
      iTween.MoveUpdate(gameObject, iTween.Hash("position", target, "time", time));

    }
  }

  void OnTriggerEnter2D(Collider2D coll)
  {
    if (coll.gameObject.name.ToLower().IndexOf("target") >= 0)
    {
      Destroy(gameObject,2f);
      rigidbody2D.gravityScale = 8f;
      rigidbody2D.AddForce(Vector3.up * 16f, ForceMode2D.Impulse);
      GetComponent<Collider2D>().enabled = false;

      if (gameObject.name.ToLower().IndexOf("good") >= 0)
        GameManager.Instance.HitPlayer();
    }
  }

  void OnTriggerExit2D(Collider2D coll)
  {

  }

  void OnDestroy()
  {
    ClearGhost();
  }

  void ClearGhost()
  {
    if (mSprite.Count > 0)
    {
      for (int i = 0; i < mSprite.Count; i++)
      {
        DestroyObject(mSprite[i]);
      }

      mSprite.Clear();
    }
  }





}
