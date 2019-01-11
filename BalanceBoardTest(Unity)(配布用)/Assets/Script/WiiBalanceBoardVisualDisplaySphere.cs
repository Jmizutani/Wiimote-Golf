using UnityEngine;
using System.Collections;
/// <summary>
/// 荷重中心の位置を使って軌跡を表示
/// </summary>
public class WiiBalanceBoardVisualDisplaySphere : WiimoteInfoDisplayBase 
{
	protected float gainScale = 0.01f;
	//距離の単位変換ゲイン[cm-> m なので1/100]


	// Use this for initialization
	override protected void Output()
	{
		//荷重中心の位置を使ってオブジェクトの位置を上書きする
		Vector3 localPos = new Vector3
			(balanceBoardData.copPos.y * gainScale ,
				gameObject.transform.localPosition.y,
				balanceBoardData.copPos.x * gainScale); 
		gameObject.transform.localPosition = localPos;
	}
}
