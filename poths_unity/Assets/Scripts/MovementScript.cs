using UnityEngine;

using System.Collections;



public enum State
{
	Idle,
	Jumping,
	Attacking,
	Blocking,
	Hitstun,
	Blockstun
};

public class MovementScript : MonoBehaviour {
	
	public string buttonRight;
	public string buttonLeft;
	public string buttonUp;
	public string buttonDown;
	public string buttonHigh;
	public string buttonBlock;
	
	public Vector3 defaultPos;
	
	public GameObject targetCharacter;
	public GameObject managerCollection;
	
	private int health = 1000;
	
	private float horSpeed = 0;
	private float verSpeed = 0;

	private int viewDirection = 1;
	
	private State currentState = State.Idle;
	
	private MoveList myMoves;
	
	private int attackTimer = 0;
	private int attackTime = 0;
	private int stunTime = 0;
	
	//TODO do more robust sytem, this is just to get something working asap
	private bool attackHit = false;
	private bool hitThisFrame = false;
	
	// Use this for initialization
	void Start () {
	
		myMoves = new MoveList();
		myMoves.Load("Movelists");
		
		if(!animation)
			return;
			
		animation["idle"].speed = 0.1f;
		animation["walk"].speed = 0.1f;
		animation["jump"].speed = 0.1f;
		animation["hit"].speed = 0.1f;
		animation["block"].speed = 0.8f;

		animation["idle"].wrapMode = WrapMode.Loop;
		animation["walk"].wrapMode = WrapMode.Loop;
		animation["jump"].wrapMode = WrapMode.Once;
		animation["hit"].wrapMode = WrapMode.Once;
		animation["block"].wrapMode = WrapMode.Once;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		hitThisFrame = false;
		
		if (attackTimer < attackTime)
		{
			attackTimer++;
			if (attackTimer >= attackTime) 
			{
				currentState = State.Idle;
			}
		}
		else if (stunTime > 0)
		{
			stunTime--;
			if (stunTime <= 0) 
			{
				currentState = State.Idle;
			}
		}
		
		CollObject hurtBox1 = new CollObject();
		hurtBox1.owner = this.gameObject;
		hurtBox1.radius = 2;
		hurtBox1.type = CollisionType.Hittable;
		
		CollObject hurtBox2 = new CollObject();
		hurtBox2.owner = this.gameObject;
		hurtBox2.radius = 2;
		hurtBox2.type = CollisionType.Hittable;
		
		CollObject hurtBox3 = new CollObject();
		hurtBox3.owner = this.gameObject;
		hurtBox3.radius = 2;
		hurtBox3.type = CollisionType.Hittable;
		
		CollObject hurtBox4 = new CollObject();
		hurtBox4.owner = this.gameObject;
		hurtBox4.radius = 2;
		hurtBox4.type = CollisionType.Hittable;
		
		//MAGIC NUMBERS FOR NOW: FIX
		//position of hitbox will be read from data later
		hurtBox1.pos = new Vector2(this.transform.position.x, this.transform.position.y+1.0f);
		hurtBox2.pos = new Vector2(this.transform.position.x, this.transform.position.y+3.0f);
		hurtBox3.pos = new Vector2(this.transform.position.x, this.transform.position.y+5.0f);
		hurtBox4.pos = new Vector2(this.transform.position.x, this.transform.position.y+7.0f);
		
		managerCollection.GetComponent<CollisionManagerScript>().AddCollBox(hurtBox1);
		managerCollection.GetComponent<CollisionManagerScript>().AddCollBox(hurtBox2);
		managerCollection.GetComponent<CollisionManagerScript>().AddCollBox(hurtBox3);
		managerCollection.GetComponent<CollisionManagerScript>().AddCollBox(hurtBox4);
		
		if(currentState == State.Attacking && !attackHit)
		{
			Move attack = myMoves.GetMove("High");
			if(attack != null)
			{
				Hitbox box = attack.Hitboxes[0];
				if(attackTimer > box.Spawntime && attackTimer < attack.Attacktime)
				{
					CollObject attackBox = new CollObject();
					attackBox.owner = this.gameObject;
					attackBox.pos = new Vector2(this.transform.position.x + (viewDirection * box.PosX), this.transform.position.y+box.PosY);
					attackBox.radius = box.Radius;
					attackBox.type = box.Type;
					attackBox.properties = new System.Collections.Generic.Dictionary<PropertyType, int>();
					foreach(AttackProperty prop in box.Properties)
					{
						attackBox.properties.Add(prop.Type, prop.Value);
					}
					managerCollection.GetComponent<CollisionManagerScript>().AddCollBox(attackBox);
				}
			}
		}		
		
		DoMovement();
		
		//Animations
		if (animation == null)
			return;

		if (currentState == State.Idle)
		{
			if(Input.GetButton(buttonHigh)) 
			{
				Move attack = myMoves.GetMove("High");
				
				horSpeed = 0;
				currentState = State.Attacking;
				attackHit = false;
				attackTime = attack.Totaltime;
				attackTimer = 0;
				
				//Find Animation Effect
				Effect anim = attack.Effects.Find(delegate(Effect obj) {
					return obj.Type == EffectType.Animation;
				});
				
				if(anim != null)
				{
					//Find Name property
					EffectProperty prop = anim.Properties.Find(delegate(EffectProperty obj) {
						return obj.Type == PropertyType.Name;	
					});
					
					if(prop != null)
					{
						animation[prop.Value].wrapMode = WrapMode.Once;
						animation[prop.Value].speed = animation[prop.Value].length / (attackTime/60.0f);
						animation.Play(prop.Value);
					}
				}
			}
		}
		
		if (currentState == State.Idle)
		{
			if(Input.GetButton(buttonBlock)) 
			{
				horSpeed = 0;
				
				animation.Play("block");
					
				currentState = State.Blocking;
			}
		}
		else if (currentState == State.Blocking)
		{
			if(!Input.GetButton(buttonBlock)) 
				currentState = State.Idle;
		}
		
		if (currentState == State.Idle) 
		{
			if(horSpeed != 0)
				animation.Play("walk");
			else
				animation.Play ("idle");
		}
	}

	void DoMovement()
	{		
		//Handle landing
		if(currentState == State.Jumping && transform.position.y <= 0){
			currentState = State.Idle;
		}

		//Handle movement input
		if(currentState == State.Idle)
		{
			if(Input.GetButton(buttonRight)) {
				horSpeed = 0.5f;
			}
			else if(Input.GetButton(buttonLeft)) {
				horSpeed = -0.5f;
			}
			else {
				horSpeed = 0;
			}
			
			if(Input.GetButton(buttonUp)){
				verSpeed = 1.2f;
				horSpeed *= 0.7f;

				currentState = State.Jumping;
				if(animation)
					animation.Play("jump");
			}
		}
		else
		{
			verSpeed -= 0.05f;
		}
		
		//Handle actual movement
		if(horSpeed != 0)
		{
			float maxDist = 30;
			float targetX = transform.position.x;
			if(transform.position.x + horSpeed < -10){
				targetX = -10;
				horSpeed = 0;
			}else if(transform.position.x + horSpeed > 120) {
				targetX = 120;
				horSpeed = 0;
			}
			//Enforce maximum distance between characters
			else if(Mathf.Abs(transform.position.x + horSpeed - targetCharacter.transform.position.x) > maxDist) {
				targetX = targetCharacter.transform.position.x + (maxDist * Mathf.Sign(transform.position.x - targetCharacter.transform.position.x));
				horSpeed = 0;
			}
			
			transform.position = new Vector3(targetX,transform.position.y,transform.position.z);
		}
		
		//Handle rotation
		if(currentState == State.Idle)
		{
			transform.LookAt(new Vector3(targetCharacter.transform.position.x,transform.position.y,transform.position.z));
			viewDirection = (transform.position.x < targetCharacter.transform.position.x) ? 1 : -1;
		}
		
		if(transform.position.y + verSpeed < 0)
		{
			transform.position = new Vector3(transform.position.x,0,transform.position.z);
			verSpeed = 0;
		}

		transform.position += new Vector3(horSpeed,verSpeed,0);
	}
	
	public void NewRound()
	{
		health = 1000;
		transform.position = defaultPos;
	}
	
	public void OnCollision(CollObject myObj, CollObject theirObj)
	{
		if(myObj.type == CollisionType.Hittable && !hitThisFrame)
		{
			hitThisFrame = true;
						
			if(currentState == State.Blocking || currentState == State.Blockstun)
			{
				if(theirObj.properties.TryGetValue(PropertyType.Blockstun, out stunTime))
				{
					horSpeed = 0;
					currentState = State.Blockstun;
					/*if(animation)
					{
						animation["block"].speed = animation["block"].length / (stunTime/60.0f);
						animation.Play("block");
					}*/
				}
				
				Debug.Log(name + " says: Not this time!");
			}
			else
			{			
				if(theirObj.properties.TryGetValue(PropertyType.Hitstun, out stunTime))
				{
					horSpeed = 0;
					currentState = State.Hitstun;
					if(animation)
					{
						animation["hit"].speed = animation["hit"].length / (stunTime/60.0f);
						animation.Play("hit");
					}
				}
				
				int dmg = 0;
				theirObj.properties.TryGetValue(PropertyType.Damage, out dmg);
				health -= dmg;
			}
		}	
		
		if(myObj.type == CollisionType.Hit)
			attackHit = true;
	}
	
	public int GetHealth()
	{
		return health;
	}
}
