using UnityEngine;
using Es.InkPainter;

	public class RayPainter : MonoBehaviour
	{
		[SerializeField]
		private Brush brush;

		public void Paint(){
			Ray ray = new Ray (transform.position, -transform.up);
			bool success = true;
			RaycastHit hitInfo;
			Debug.Log ("update");
			if(Physics.Raycast(ray, out hitInfo))
			{
				Debug.Log ("ray enabled");
				InkCanvas paintObject = hitInfo.transform.GetComponent<InkCanvas>();
				if (paintObject != null) {
					success = paintObject.Paint(brush, hitInfo);
					Debug.Log ("ray enabled"+ success);
				}

				if(!success)
					Debug.LogError("Failed to paint.");
			}
		}

		private void Update()
		{
			
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
