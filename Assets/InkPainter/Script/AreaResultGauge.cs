using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaResultGauge : MonoBehaviour {
	public float myArea;
	public float enemyArea;

	float tmpLimit;
	float tmpMyArea;
	float tmpEnemyArea;
	public Image myAreaGaugeBar;
	public Image enemyAreaGaugeBar;
	public Text myAreaGaugeText;
	public Text enemyAreaGaugeText;

	


	public void SetAreaResultParams(float myArea,float enemyArea)
	{
		this.myArea = myArea*100;
		this.enemyArea = enemyArea*100;

		tmpMyArea = tmpEnemyArea = 0f;
		//少ない方の1/3までは増える
		tmpLimit = Mathf.Max(this.myArea, this.enemyArea)*0.3f;

		Debug.LogWarning(this.myArea);
		Debug.LogWarning(this.enemyArea);
		Debug.LogWarning(tmpLimit);
	}

	bool lastResult=false;
	public enum STATE
	{
		COUNTING,
		STOPPING,
		RESULT,
	}
	STATE state = STATE.COUNTING;
	int counter = 0;
	public STATE UpdateAreaPercentage()
	{
		counter++;
		switch (state)
		{
			case STATE.COUNTING:
				float all = myArea + enemyArea + 0.0000001f;

				tmpMyArea += 0.01f * myArea;
				tmpEnemyArea += 0.01f * enemyArea;

				tmpMyArea = Mathf.Min(tmpMyArea, tmpLimit);
				tmpEnemyArea = Mathf.Min(tmpEnemyArea, tmpLimit);

				myAreaGaugeBar.fillAmount = tmpMyArea / all;
				enemyAreaGaugeBar.fillAmount = tmpEnemyArea / all;

				myAreaGaugeText.text = ((int)((int)(tmpMyArea * 10000) / all)).ToString();
				enemyAreaGaugeText.text = ((int)((int)(tmpEnemyArea * 10000) / all)).ToString();
				Debug.LogWarning(tmpMyArea / all);
				Debug.LogWarning(tmpEnemyArea / all);

				
				if (counter >= 20)
				{
					state = STATE.STOPPING;
					counter = 0;
				}
				break;
			case STATE.STOPPING:
				if (counter >= 10)
				{
					state = STATE.RESULT;
					counter = 0;
				}
				break;
			case STATE.RESULT:
				ShowAreaPeacentageResult();
				break;
		}
		return state;
	}

	public void ShowAreaPeacentageResult()
	{
		float all = myArea + enemyArea + 0.0000001f;
		myAreaGaugeBar.fillAmount = myArea / all;
		enemyAreaGaugeBar.fillAmount = enemyArea / all;

		myAreaGaugeText.text = ((int)((int)(myArea * 10000) / all)).ToString();
		enemyAreaGaugeText.text = ((int)((int)(enemyArea * 10000) / all)).ToString();
	}
}
