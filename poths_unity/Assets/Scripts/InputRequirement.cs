using System;
using System.Collections;
using System.Collections.Generic;

public class InputRequirement
{

	public void ParseInput(String inputString)
	{
		inputs.Clear();
		Input newInput = new Input();
		
		newInput.buttonState = 0;
		foreach(char c in inputString)
		{
			if(c == 'H')
				newInput.buttonState |= Constants.BUTTONHIGH;
			if(c == 'M') 
				newInput.buttonState |= Constants.BUTTONMID;
			if(c == 'L')
				newInput.buttonState |= Constants.BUTTONLOW;
			if(c == '6') 
				newInput.buttonState |= Constants.BUTTONRIGHT;
			if(c == '4') 
				newInput.buttonState |= Constants.BUTTONLEFT;
			if(c == '8')
				newInput.buttonState |= Constants.BUTTONUP;
			if(c == '2')
				newInput.buttonState |= Constants.BUTTONDOWN;
			if(c == 'B')
				newInput.buttonState |= Constants.BUTTONBLOCK;
		}
		
		newInput.leniency = 0;
		
		inputs.Add(newInput);
	}
	
	public bool Verify(Queue<int> buffer)
	{
		//UGLY TEMP SOLUTION - Loop through buffer backwards and compare with list of required inputs eventually
		int buttonState = buffer.ToArray()[buffer.Count - 1];
		
		if(this.inputs[0].buttonState == buttonState)
			return true;
			
		return false;
	}
	
	private struct Input
	{
		public int buttonState;
		public int leniency;
	}
	private List<Input> inputs = new List<Input>();
}

