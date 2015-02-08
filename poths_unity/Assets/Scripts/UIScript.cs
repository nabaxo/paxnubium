using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScript : MonoBehaviour {

	public Text P1Health;
	public Text P2Health;
	
	public Text P1Rounds;
	public Text P2Rounds;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetP1Health(int newHealth)
	{
		P1Health.text = newHealth.ToString();
	}
	
	public void SetP2Health(int newHealth)
	{
		P2Health.text = newHealth.ToString();
	}
	
	public void SetP1Rounds(int newRounds)
	{
		P1Rounds.text = newRounds.ToString();
	}
	
	public void SetP2Rounds(int newRounds)
	{
		P2Rounds.text = newRounds.ToString();
	}
}
