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
	
	const int BUTTONHIGH =	1;
	const int BUTTONMID =	2;
	const int BUTTONLOW =	4;
	const int BUTTONRIGHT =	8;
	const int BUTTONLEFT =	16;
	const int BUTTONUP =	32;
	const int BUTTONDOWN =	64;
	const int BUTTONBLOCK =	128;
	
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
			buttonState += BUTTONHIGH;
		
		if(Input.GetButton(buttonMid)) 
			buttonState += BUTTONMID;
		
		if(Input.GetButton(buttonLow)) 
			buttonState += BUTTONLOW;
		
		if(Input.GetButton(buttonRight)) 
			buttonState += BUTTONRIGHT;
		
		if(Input.GetButton(buttonLeft)) 
			buttonState += BUTTONLEFT;
		
		if(Input.GetButton(buttonUp))
			buttonState += BUTTONUP;
		
		if(Input.GetButton(buttonDown))
			buttonState += BUTTONDOWN;
		
		if(Input.GetButton(buttonBlock))
			buttonState += BUTTONBLOCK;
			
		//Remove oldest input
		if(myBuffer.Count >= MaxBufferSize)
			myBuffer.Dequeue();
			
		myBuffer.Enqueue(buttonState);
	}
	
	public Move InputtedMove()
	{
		//UGLY TEMP SOLUTION - Will be solved later by having movelist find the correct attack anyway
		int buttonState = myBuffer.ToArray()[MaxBufferSize - 1];
		
		if(Input.GetButton(buttonHigh))
		{
			int a = 0;
			a++;
		}
		
		if((buttonState & BUTTONHIGH) > 0)
		{
			return myMoves.GetMove("High");
		}
		if((buttonState & BUTTONMID) > 0)
		{
			return myMoves.GetMove("Mid");
		}
		if((buttonState & BUTTONLOW) > 0)
		{
			return myMoves.GetMove("Low");
		}
		
		return null;
	}
}
