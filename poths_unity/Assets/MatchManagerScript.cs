using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MatchManagerScript : MonoBehaviour {

	public GameObject Player1;
	public GameObject Player2;
	
	public UIScript UI;
	
	int P1Rounds = 0;
	int P2Rounds = 0;
	
	// Use this for initialization
	void Start () {
		UI.SetP1Rounds(0);
		UI.SetP2Rounds(0);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		int P1Health = Player1.GetComponent<MovementScript>().GetHealth();
		int P2Health = Player2.GetComponent<MovementScript>().GetHealth();
		
		UI.SetP1Health(P1Health);
		UI.SetP2Health(P2Health);
		
		if(P1Health <= 0)
		{
			P2Rounds++;
			UI.SetP2Rounds(P2Rounds);
			ResetRound();
		}
		
		if(P2Health <= 0)
		{
			P1Rounds++;
			UI.SetP1Rounds(P1Rounds);
			ResetRound();
		}
		
	}
	
	void ResetRound()
	{
		Player1.GetComponent<MovementScript>().NewRound();
		Player2.GetComponent<MovementScript>().NewRound();
	}
}
