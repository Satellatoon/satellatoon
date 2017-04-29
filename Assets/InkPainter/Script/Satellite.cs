using UnityEngine;


public class Satellite : MonoBehaviour
{
	//円周用(中心・スピード)
	public GameObject ellipseCenter;
	public float speed;



	public int energyComsumption;
	public float loudness;		//塗れる範囲

	public SENSOR_TYPE sensorType=SENSOR_TYPE.NONE;
	public enum SENSOR_TYPE{
		IR, //赤外線
		AERIAL_PHOTO, //航空写真
		NONE,			//なし
	}

	//実際に塗るためのコンポーネント
	RayPainter rayPainter;
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

	//塗る
	public int Paint(int playerColor){
		rayPainter.Paint (playerColor);
		return energyComsumption;
	}


}

