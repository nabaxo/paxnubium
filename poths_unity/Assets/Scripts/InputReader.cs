using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputReader : MonoBehaviour {

	public string buttonRight;
	public string buttonLeft;
	public string buttonUp;
	public string buttonDown;
	public string buttonHigh;
	public string buttonMid;
	public string buttonLow;
	public string buttonBlock;
	
	private MoveList myMoves;
	
	const int MaxBufferSize = 300;
	private Queue<int> myBuffer;
	
	// Use this for initialization
	void Start () {
		myBuffer = new Queue<int>(MaxBufferSize);
		//Fill buffer with empty input
		for(int i=0;i<MaxBufferSize;i++)
		{
			myBuffer.Enqueue(0);
		}
		
		myMoves = new MoveList();
		myMoves.Load("Movelists");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		int buttonState = 0;
		if(Input.GetButton(buttonHigh))
			buttonState += Constants.BUTTONHIGH;
		
		if(Input.GetButton(buttonMid)) 
			buttonState += Constants.BUTTONMID;
		
		if(Input.GetButton(buttonLow)) 
			buttonState += Constants.BUTTONLOW;
		
		if(Input.GetButton(buttonRight)) 
			buttonState += Constants.BUTTONRIGHT;
		
		if(Input.GetButton(buttonLeft)) 
			buttonState += Constants.BUTTONLEFT;
		
		if(Input.GetButton(buttonUp))
			buttonState += Constants.BUTTONUP;
		
		if(Input.GetButton(buttonDown))
			buttonState += Constants.BUTTONDOWN;
		
		if(Input.GetButton(buttonBlock))
			buttonState += Constants.BUTTONBLOCK;
			
		//Remove oldest input
		if(myBuffer.Count >= MaxBufferSize)
			myBuffer.Dequeue();
			
		myBuffer.Enqueue(buttonState);
	}
	
	public Move InputtedMove()
	{
		return myMoves.GetMove(myBuffer);
	}
}
