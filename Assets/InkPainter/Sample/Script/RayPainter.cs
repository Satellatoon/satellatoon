using UnityEngine;
using Es.InkPainter;

public class RayPainter : MonoBehaviour
{
	[SerializeField]
	private Brush brushMine;
	[SerializeField]
	private Brush brushOpponent;


	public void Paint(int playerColor,Vector3 dir){
		//Ray ray = new Ray (transform.position, -transform.up);
		Ray ray = new Ray (transform.position, dir);
		bool success = true;
		RaycastHit hitInfo;
		Debug.Log ("update");

		Brush brush = brushMine;
		switch (playerColor) {
		case 0:
			brush = brushMine;
			break;
		case 1:
			brush = brushOpponent;
			break;
		default:
			break;
		}

		if(Physics.Raycast(ray, out hitInfo))
		{
			Debug.Log ("ray enabled");
			InkCanvas paintObject = hitInfo.transform.GetComponent<InkCanvas>();
			if (paintObject != null) {
				brush.playerID = playerColor;
				success = paintObject.Paint(brush, hitInfo);

				Debug.Log ("ray enabled"+ success);
			}

			if(!success)
				Debug.LogError("Failed to paint.");
		}
	}
		

	public void OnGUI()
	{
		if(GUILayout.Button("Reset"))
		{
			foreach(var canvas in FindObjectsOfType<InkCanvas>())
				canvas.ResetPaint();
		}
	}
}
