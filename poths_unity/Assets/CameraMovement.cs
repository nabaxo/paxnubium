using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	
	public float minX;
	public float maxX;
	public float minZ;
	public float maxZ;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		GameObject player1 = GameObject.FindWithTag("Player");
		GameObject player2 = GameObject.FindWithTag("Player2");
		
		float leftXPos = Mathf.Min(player1.transform.position.x,player2.transform.position.x);
		float rightXPos = Mathf.Max(player1.transform.position.x,player2.transform.position.x);
		
		float playerXDistance = rightXPos - leftXPos;
		
		float bottomYPos = Mathf.Min(player1.transform.position.y,player2.transform.position.y);
		float topYPos = Mathf.Max(player1.transform.position.y,player2.transform.position.y);
				
		float playerYDistance = topYPos - bottomYPos;
		
		//float playerMaxDistance = Mathf.Max(playerXDistance,playerYDistance);
		
		Vector3 desiredPosition = new Vector3(Mathf.Min(maxX, Mathf.Max(minX, leftXPos + playerXDistance/2)),
												8 + bottomYPos + playerYDistance/2, 
												transform.position.z /*-Mathf.Min(maxZ,Mathf.Max(minZ, 10 + playerMaxDistance))*/);
		
		Vector3 relativePos = new Vector3(desiredPosition.x, desiredPosition.y, -20) - transform.position;
			
		Quaternion tempRot = Quaternion.LookRotation(relativePos);
		transform.rotation = Quaternion.Lerp(transform.rotation, tempRot, 10 * Time.deltaTime);
		transform.position = Vector3.Lerp(transform.position, desiredPosition,30 * Time.deltaTime);
	}
}
