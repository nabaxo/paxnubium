using UnityEngine;

using System;
using System.IO;

using System.Collections.Generic;

using System.Xml;
using System.Xml.Serialization;

public class MoveList : List<Move>
{
		public MoveList ()
		{
		}
		
		public Move GetMove(Queue<int> buffer)
		{
			foreach(Move move in this)
			{
				if(move.VerifyInput(buffer))
				{ 
					return move;
				}
			}
			
			return null;
		}
	
		public Move GetMove(string moveName)
		{
			foreach(Move move in this)
			{
				if(move.Name == moveName)
				{ 
					return move;
				}
			}
			
			return null;
		}
		
		public void Load(string directoryPath)
		{
			DirectoryInfo info = new DirectoryInfo(directoryPath);
			FileInfo[] fileInfo = info.GetFiles();
			foreach (FileInfo file in fileInfo)
			{
				if(file.Extension == ".xml")
				{
					LoadMove(file.FullName);
				}
			}
		}
		
		void LoadMove(string path)
		{
			var serializer = new XmlSerializer(typeof(Move));
			var stream = new FileStream(path, FileMode.Open);
			Move loadedMove = serializer.Deserialize(stream) as Move;
			stream.Close();
			
			loadedMove.Init();
		
			Add(loadedMove);
		}
}

[XmlRoot("Move")]
public class Move
{
	//***************************************************
	//Initialization
	public void Init()
	{
		int highestTotaltime = 0;
		foreach(Hitbox attack in Hitboxes)
		{
			int totalTime = attack.Spawntime + attack.Lifetime;
			if(totalTime > highestTotaltime)
				highestTotaltime = totalTime;
		}
		
		Attacktime = highestTotaltime;
		
		int recoveryTime = 0;
		
		EffectProperty prop = GetProperty(EffectType.Recovery, PropertyType.Time);
		if(prop != null)
		{
			try
			{
				recoveryTime = Convert.ToInt32(prop.Value);
			}
			catch (Exception)
			{
				Debug.Log("Recovery not correct format in move: " + Name);
				recoveryTime = 0;
			}
		}
		
		Totaltime = Attacktime + recoveryTime;
		
		inputReq.ParseInput(req.input);
	}
	
	//***************************************************
	//Actions
	
	public bool VerifyInput(Queue<int> buffer)
	{
		return inputReq.Verify(buffer);
	}
	
	//Perform move
	public void Do(MovementScript parent)
	{
		AttackTimer = 0;
		foreach(Hitbox attack in Hitboxes)
		{
			attack.enabled = true;
		}
		parentRef = parent;
	}
	
	public void Update()
	{
		foreach(Hitbox box in Hitboxes)
		{
			if(AttackTimer > box.Spawntime && AttackTimer < box.Spawntime + box.Lifetime)
			{
				parentRef.CreateHitbox(box);
			}
		}
		
		foreach(Effect effect in Effects)
		{
			EffectProperty timeProp = effect.GetProperty(PropertyType.Starttime);
			
			if(AttackTimer == ((timeProp != null) ? timeProp.GetValueInt() : 0))
			{
				switch(effect.Type)
				{
					case EffectType.Animation:
						EffectProperty nameProp = effect.GetProperty(PropertyType.Name);
						parentRef.PlayAnimation(((nameProp != null) ? nameProp.Value : ""), Totaltime);
					break;
				}
			}
		}
		
		AttackTimer++;
	}
	
	public void OnCollision(CollObject myObj, CollObject theirObj)
	{
		foreach(Hitbox attack in Hitboxes)
		{
			if(attack.HitID == myObj.HitID)
			{
				attack.enabled = false;
			}
		}
	}
	
	//***************************************************
	//Accessors
	public EffectProperty GetProperty(EffectType effect, PropertyType property)
	{
		//Find Effect
		Effect eff = Effects.Find(delegate(Effect obj) {
			return obj.Type == effect;
		});
		
		if(eff != null)
		{
			return eff.GetProperty(property);
		}
		
		return null;
	}
		
	public bool Active()
	{
		return (AttackTimer <= Totaltime);
	}
		
	//***************************************************
	//Xml
	[XmlAttribute("Name")]
	public string Name = "";
	
	[XmlElement("Requirement")]
	public Requirement req;
	
	[XmlElement("Hitbox")]
	public List<Hitbox> Hitboxes = new List<Hitbox>();
	
	[XmlElement("Effect")]
	public List<Effect> Effects = new List<Effect>();
	
	
	//Dynamically calculated values:
	public InputRequirement inputReq = new InputRequirement();
	
	public int Attacktime;
	public int Totaltime;
	
	//Values that change while attack is happening:
	public int AttackTimer;
	
	//Reference to creator
	private MovementScript parentRef;
}

public class Requirement
{	
	[XmlElement("Input")]
	public String input = "";
}

public class Hitbox
{
	//Controls
	public bool enabled;
	
	//Attributes
	[XmlAttribute("Spawntime")]
	public int Spawntime = 0;
	[XmlAttribute("HitID")]
	public int HitID = 0;
	[XmlAttribute("Lifetime")]
	public int Lifetime = 0;
	[XmlAttribute("PosX")]
	public float PosX = 0.0f;
	[XmlAttribute("PosY")]
	public float PosY = 0.0f;
	[XmlAttribute("Radius")]
	public float Radius = 0.0f;
	[XmlAttribute("Type")]
	public CollisionType Type = CollisionType.Unknown;
	
	[XmlElement("Property")]
	public List<AttackProperty> Properties = new List<AttackProperty>();
}

public class AttackProperty
{
	[XmlAttribute("Type")]
	public PropertyType Type = PropertyType.Unknown;
	[XmlAttribute("Value")]
	public int Value = 0;
}

public class Effect
{
	public EffectProperty GetProperty( PropertyType property)
	{		
		//Find property
		EffectProperty prop = Properties.Find(delegate(EffectProperty obj) {
			return obj.Type == property;	
		});
		
		return prop;
	}
	
	[XmlAttribute("Type")]
	public EffectType Type = 0;
	
	[XmlElement("Property")]
	public List<EffectProperty> Properties = new List<EffectProperty>();
}

public class EffectProperty
{
	public int GetValueInt()
	{		
		int ret;
		try
		{
			ret = Convert.ToInt32(Value);
		}
		catch (Exception)
		{
			Debug.Log(Type.ToString() + " not correct format in move");
			return 0;
		}
		
		return ret;
	}
	
	[XmlAttribute("Type")]
	public PropertyType Type = PropertyType.Unknown;
	[XmlAttribute("Value")]
	public string Value = "";
}