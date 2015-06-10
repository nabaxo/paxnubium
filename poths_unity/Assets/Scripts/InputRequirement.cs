using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InputRequirement
{

	public void ParseInput(String inputString)
	{
		inputs.Clear();
		Input newInput = new Input();
		
		bool combine = false;
		
		newInput.buttonState = 0;
		foreach(char c in inputString)
		{
			if(combine)
			{
				Input toCombine = inputs.Last();
				toCombine.buttonState |= ParseButton(c);
				inputs[inputs.Count - 1] = toCombine;
				combine = false;
			}
			else
			{
				if(c == '+')
				{
					combine = true;
				}
				else
				{
					newInput.buttonState = ParseButton(c);
					newInput.leniency = 3;
					
					inputs.Add(newInput);
				}
			}
		}
	}
	
	private int ParseButton(char c)
	{
		if(c == 'H')
			 return Constants.BUTTONHIGH;
		if(c == 'M') 
			return Constants.BUTTONMID;
		if(c == 'L')
			return Constants.BUTTONLOW;
		if(c == '1') 
			return (Constants.BUTTONDOWNLEFT);
		if(c == '2') 
			return Constants.BUTTONDOWN;
		if(c == '3')
			return Constants.BUTTONDOWNRIGHT;
		if(c == '4')
			return Constants.BUTTONLEFT;
		if(c == '6') 
			return Constants.BUTTONRIGHT;
		if(c == '7') 
			return Constants.BUTTONUPLEFT;
		if(c == '8')
			return Constants.BUTTONUP;
		if(c == '9')
			return Constants.BUTTONUPRIGHT;
		if(c == 'B')
			return Constants.BUTTONBLOCK;
			
		return -1;
	}
	
	public bool Verify(Queue<int> buffer)
	{
		//Final button press needs to be pressed on the current frame no matter what
		if(this.inputs.Last().buttonState != buffer.Last())
			return false;
			
		//If we only have one input, that's it!
		if(this.inputs.Count == 1)
			return true;
			
		int currentInputIndex = this.inputs.Count - 2; //Last input already confirmed, move to second last
		int tension = 0; // How many inputs have we skipped looking for the current input. If this exceeds the inputs leniency value, we abort
		for(int i = buffer.Count - 2; i >= 0; i--)
		{
			Input expectedInput = this.inputs[currentInputIndex];
			int bufferInput = buffer.ToArray()[i];
			
			if((bufferInput & expectedInput.buttonState) == expectedInput.buttonState)	//Does expected input exist in the buffer input.
			{
				tension = 0;
				currentInputIndex--;
				if(currentInputIndex < 0)
					return true;
			}
			else
			{
				tension++;
				if(tension > expectedInput.leniency)
					return false;
			}
		}
					
		return false;
	}
	
	private struct Input
	{
		public int buttonState;
		public int leniency;
	}
	private List<Input> inputs = new List<Input>();
}

