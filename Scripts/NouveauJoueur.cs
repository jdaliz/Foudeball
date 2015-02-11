using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NouveauJoueur : MonoBehaviour {
	private int cpt;
	public GameObject InputName;

  public GameObject ShowText;

	// Use this for initialization
	void Start () {
		cpt = 1;
		while (PlayerPrefs.HasKey(cpt.ToString())) 
    {
			cpt = cpt+3;
		}


    if (PlayerPrefs.HasKey("FoudeballGoogle"))
    {
      if (ShowText.GetComponent<Text>() != null)
      ShowText.GetComponent<Text>().text = PlayerPrefs.GetString("FoudeballGoogle");
    }

		PlayerPrefs.SetInt ("CurrentPlayer", cpt);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void PasserNiveauSuivant(string LevelName)
	{

		Text NomJoueur = InputName.GetComponent<Text>();

    if (PlayerPrefs.HasKey("FoudeballGoogle"))
      NomJoueur.text = PlayerPrefs.GetString("FoudeballGoogle");
		PlayerPrefs.SetString ((cpt+2).ToString(),NomJoueur.text);

		
		Application.LoadLevel(LevelName);
	}


  // Use this for initialization
  public void ResetAllParam()
  {
    PlayerPrefs.DeleteAll();

  }
	
}
