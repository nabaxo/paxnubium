using UnityEngine;
using System.Collections;

public class InputReader : MonoBehaviour {

	public string buttonRight;
	public string buttonLeft;
	public string buttonUp;
	public string buttonDown;
	public string buttonHigh;
	public string buttonBlock;
	
	private MoveList myMoves;
	
	// Use this for initialization
	void Start () {
		myMoves = new MoveList();
		myMoves.Load("Movelists");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}
	
	public Move InputtedMove()
	{
		if(Input.GetButton(buttonHigh)) 
		{
			return myMoves.GetMove("High");
		}
		
		return null;
	}
}
