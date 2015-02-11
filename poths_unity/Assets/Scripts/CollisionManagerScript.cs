//#define RENDERHITSPHERES

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum CollisionType{
	Unknown,
	Hittable,
	Hit
}

public interface Collider {
	void OnCollision(CollObject myObj, CollObject theirObj);
}

public struct CollObject {

	public CollObject(int ID, Collider collider, Vector2 pos, float radius, CollisionType type) 
	{
		this.ID = ID;
		this.collider = collider;
		this.pos = pos;
		this.radius = radius;
		this.type = type;
		this.properties = new Dictionary<PropertyType, int>();
	}
	
	public int ID;
	public Collider collider;
	public Vector2 pos;
	public float radius;
	public CollisionType type;
	public Dictionary<PropertyType, int> properties;
}



public class CollisionManagerScript : MonoBehaviour {

	#if RENDERHITSPHERES
	private List<GameObject> myRenderSpherePool;
	#endif
	private List<CollObject> myCircleList;
	private int IDGenerator = 0;
	
	// Use this for initialization
	void Start () {
		myCircleList = new List<CollObject>();
		#if RENDERHITSPHERES
		myRenderSpherePool = new List<GameObject>();
		#endif
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		for(int i = 0; i < myCircleList.Count; i++)
		{
			for(int j = i; j < myCircleList.Count; j++)
			{
				CollisionCheck(myCircleList[i], myCircleList[j]);
			}
		}
		
		#if RENDERHITSPHERES
		foreach(GameObject sphere in myRenderSpherePool)
		{
			sphere.renderer.enabled = false;
		}
		
		for(int i = 0; i < myCircleList.Count; i++)
		{
			while(myRenderSpherePool.Count <= i)
			{
				GameObject newSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);	
				myRenderSpherePool.Add(newSphere);
			}
			CollObject obj = myCircleList[i];
			GameObject sphere = myRenderSpherePool[i];
			
			//sphere.transform.localScale = Vector3();
			sphere.transform.position = new Vector3(obj.pos.x, obj.pos.y, 0);
			sphere.transform.localScale = new Vector3(obj.radius, obj.radius, obj.radius);
			sphere.renderer.enabled = true;
			if(obj.type == CollisionType.Hit)
				sphere.renderer.material.color = Color.red;
			if(obj.type == CollisionType.Hittable)
				sphere.renderer.material.color = Color.green;
		}
		#endif
		myCircleList.Clear();
	}
	
	void CollisionCheck(CollObject obj, CollObject otherObj)
	{
		if(obj.ID == otherObj.ID)
			return;
			
		//one object has to be Hit and the other has to be Hittable
		if(!(obj.type == CollisionType.Hit && otherObj.type == CollisionType.Hittable ||
		   obj.type == CollisionType.Hittable && otherObj.type == CollisionType.Hit))
		   return;
		   
		if(Vector2.Distance(obj.pos, otherObj.pos) < (obj.radius + otherObj.radius))
		{
			obj.collider.OnCollision(obj, otherObj);
			otherObj.collider.OnCollision(otherObj, obj);
		}
	}
	
public void AddCollBox(CollObject obj)
	{
		myCircleList.Add(obj);
	}
	
	public int GetColliderID() { return IDGenerator++; }
}
