using UnityEngine;


public class Satellite : MonoBehaviour
{
	//円周用(中心・スピード)
	public GameObject ellipseCenter;
	public float MeanMotion; 
	private float speed;
	private Vector3 rotationAxis;

	// ユーザが乗っているか 
	public bool isUserRiding = false; 

	//	インスタンスを追加するコード
	//	GameObject obj;
	//	Satellite sat = obj.GetComponent<Satellite>();
	//	Instantiate(sat)

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

		if (MeanMotion == 0.0) { 
			MeanMotion = 8460; // 60 x 60 x 24 x 0.1[rps] 
		} 
		speed = 360.0f * MeanMotion / 86400.0f; 

		rotationAxis [0] = Random.Range (0, 100); 
		rotationAxis [1] = Random.Range (0, 100); 
		rotationAxis [2] = Random.Range (0, 100); 
	}


	void Update(){
		MoveToCircle ();
	}

	void MoveToCircle(){
		float angle = speed * Time.deltaTime;
		transform.RotateAround(ellipseCenter.transform.position, rotationAxis, angle); 
	}


	//塗る
	public int Paint(int playerColor){
		rayPainter.Paint (playerColor);
		return energyComsumption;
	}


}

