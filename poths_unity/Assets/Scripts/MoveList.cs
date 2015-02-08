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
		
		//Find Recovery effect
		Effect recoveryEffect = Effects.Find(delegate(Effect obj) {
			return obj.Type == EffectType.Recovery;
		});
		
		if(recoveryEffect != null)
		{
			//Find Time property
			EffectProperty prop = recoveryEffect.Properties.Find(delegate(EffectProperty obj) {
				return obj.Type == PropertyType.Time;	
			});
				
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
		}
		
		Totaltime = Attacktime + recoveryTime;
	}
		
	[XmlAttribute("Name")]
	public string Name = "";
	
	[XmlAttribute("Recovery")]
	public int RecoveryTime = 0;
	
	[XmlElement("Hitbox")]
	public List<Hitbox> Hitboxes = new List<Hitbox>();
	
	[XmlElement("Effect")]
	public List<Effect> Effects = new List<Effect>();
	
	
	//Dynamically calculated values:
	public int Attacktime;
	public int Totaltime;
}

public class Hitbox
{
	[XmlAttribute("Spawntime")]
	public int Spawntime = 0;
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
	[XmlAttribute("Type")]
	public EffectType Type = 0;
	
	[XmlElement("Property")]
	public List<EffectProperty> Properties = new List<EffectProperty>();
}

public class EffectProperty
{
	[XmlAttribute("Type")]
	public PropertyType Type = PropertyType.Unknown;
	[XmlAttribute("Value")]
	public string Value = "";
}