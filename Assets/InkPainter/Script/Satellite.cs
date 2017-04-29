using UnityEngine;


public class Satellite : MonoBehaviour
{
	public GameObject ellipseCenter;
	public float speed;
	RayPainter rayPainter;

	public int energyComsumption;
	void Start(){
		this.rayPainter = GetComponent<RayPainter> ();
	}


	void Update(){
		MoveToCircle ();
	}

	void MoveToCircle(){
		float angle = speed * Time.deltaTime;
		transform.RotateAround(ellipseCenter.transform.position,new Vector3(1,0,0),angle);
	}

	public int Paint(int playerID){
		rayPainter.Paint (playerID);
		return energyComsumption;
	}
}

