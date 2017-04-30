using System;
using UnityEngine;
//using AssemblyCSharp;
using Zeptomoby.OrbitTools;

public class Satellite : MonoBehaviour
{
	//円周用(中心・スピード)
	public GameObject ellipseCenter;
	public GameObject EarthObject;
	public float Inclination;	// 軌道傾斜角
	public float RAAN;			// 昇交点赤径 (Right Ascension of Ascending Node)
	public float Eccentricity;	// 離心率
	public float ArgOfPerigee;	// 近地点離角
	public float MeanAnomaly;	// 平均近点角
	public float MeanMotion; 	// 平均運動

	public string tle1;			// TLE
	public string tle2;			// TLE
	public string tle3;			// TLEh

	private float speed;
	private Vector3 rotationAxis;

	// 地球中心のxyz座標系
	// https://www.wikiwand.com/en/Earth-centered_inertial
	private Vector3 posEciStart;	// 開始位置
	private Vector3 posEciNow;		// 現在位置

	private Zeptomoby.OrbitTools.Satellite sat;

	// 
	private float velAngle;

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

		rotationAxis [0] = UnityEngine.Random.Range (0, 100); 
		rotationAxis [1] = UnityEngine.Random.Range (0, 100); 
		rotationAxis [2] = UnityEngine.Random.Range (0, 100); 

		// WriteFile ();
		// LoadTLS();

		// Test SGP4
		string str1 = "SGP4 Test";
		string str2 = "1 88888U          80275.98708465  .00073094  13844-3  66816-4 0     8";
		string str3 = "2 88888  72.8435 115.9689 0086731  52.6988 110.5714 16.05824518   105";

		Tle tle = new Tle(tle1, tle2, tle3);
		sat = new Zeptomoby.OrbitTools.Satellite(tle);
		Eci eci = sat.PositionEci(0);
		eci.ScalePosVector (0.01);
		eci.ScaleVelVector (0.01);

		Vector3 pos;
		pos.x = (float)eci.Position.X;
		pos.y = (float)eci.Position.Y;
		pos.z = (float)eci.Position.Z;
		Debug.Log (pos);
	}

	void WriteFile() {
		System.IO.StreamWriter sw = new System.IO.StreamWriter(
			@"test1.txt"
		);
		sw.Write("test");
		sw.Close();
	}

	void LoadTLS() {
		System.IO.StreamReader sr = new System.IO.StreamReader(
			@"tls.txt");
		//内容をすべて読み込む
		//		string s = sr.ReadToEnd();
		string line;
		int lineCounter = 0;
		int satCounter = 0;
		string[] tlsList = new string[3];
		while ((line = sr.ReadLine ()) != null) {
			int i = lineCounter%3;
			tlsList[i] = line;

			if(i==2){
				// add Satellite
				satCounter++;
			}

			lineCounter++;
		}
		Debug.Log (satCounter);

		//閉じる
		sr.Close();
	}

	void Update(){
		MoveToCircle ();

		//		Debug.Log (Eccentricity);
		//		float rate = getEllipseRateFromEccentricity (Eccentricity);

		//		getRadiusFromMeanMotion (MeanMotion);
		//		float r = getRadiusFromMeanMotion (1.0f);

	}

	void MoveToCircle(){
		//		float angle = speed * Time.deltaTime;
		//		transform.RotateAround(ellipseCenter.transform.position, rotationAxis, angle); 
		Vector3 pos;
		float angle = speed * Time.fixedTime;
		float r = 50.0f;
		pos.x = 0;
		pos.y = r * Mathf.Cos (angle * Mathf.PI / 180.0f);
		pos.z = r * Mathf.Sin (angle * Mathf.PI / 180.0f);

		Eci eci = sat.PositionEci(Time.fixedTime / 60.0f * 100.0f);
		eci.ScalePosVector (0.01);
		eci.ScaleVelVector (0.01);
		pos.x = (float)eci.Position.X;
		pos.y = (float)eci.Position.Y;
		pos.z = (float)eci.Position.Z;
		//Debug.Log (pos);

		//Debug.Log (Time.fixedTime);

		// 特定の位置に移動して 地球を向く
		transform.position = pos;
		transform.LookAt (new Vector3 (0, 0, 0));
		transform.Rotate (new Vector3 (0, 90, 0));
	}

	void OrbitToolsTest(){
	}

	float getEllipseRateFromEccentricity (float e){
		// 離心率から楕円の比を求める
		// https://www.wikiwand.com/ja/%E9%9B%A2%E5%BF%83%E7%8E%87
		// e^2 = (a^2 - b^2) / a^2
		// e^2 = 1 - b^2 / a^2
		// b^2 / a^2 = 1 - e^2
		// b^2 = a^2 * (1 - e^2)
		// b = a * sqrt(1 - e^2)
		// 0 <= e <= 1
		float rate = Mathf.Sqrt (1.0f - e * e);	// 楕円の比
		Debug.Log(rate);

		return rate;
	}

	float getRadiusFromMeanMotion(float m){
		// 周期から軌道半径を求める
		// http://wakariyasui.sakura.ne.jp/p/mech/bannyuu/jinnkou.html
		float G = 6.67E-11f;	// 万有引力定数
		float M = 6.0E+24f;		// 地球の質量[kg]
		// w^2 = G * M / r^3
		// r^3 = G * M / w^2
		// r = (G * M / w^2)^(1/3)
		float w = m * Mathf.PI / 3600f / 12f;
		float r = Mathf.Pow (G * M / w / w, 1.0f / 3.0f);
		Debug.Log (w);
		Debug.Log (r);

		return r;
	}

	//塗る
	public int Paint(int playerColor){
		Debug.Log ("sat paint");
		Vector3 rayDirection = EarthObject.transform.position - this.transform.position;

		rayPainter.Paint (playerColor,rayDirection.normalized);
		return energyComsumption;
	}


}

