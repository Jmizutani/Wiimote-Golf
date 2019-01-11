using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class WiiBalanceBoardStatusTextDisplay : WiiBalanceBoardDisplayTextBase
{
	
	override protected void Output(){
		text.text = "Weight:"+ balanceBoardData.weight.ToString("f2")+"[kg]\n";
		text.text += "COP.posX:" + balanceBoardData.copPos.x.ToString("f2") + "[cm]\n";
		text.text += "COP.posY:" + balanceBoardData.copPos.y.ToString("f2") + "[cm]\n";
		text.text += "SensorWeight.TopRight(sensor生値): " + balanceBoardData.sensorLoad.TopRight.ToString("f2") + "\n";
		text.text += "SensorWeight.TopLeft(sensor生値): " + balanceBoardData.sensorLoad.TopLeft.ToString("f2") + "\n";
		text.text += "SensorWeight.BottomRight(sensor生値)" + balanceBoardData.sensorLoad.BottomRight.ToString("f2")+"\n";
		text.text += "SensorWeight.BottomLeft(sensor生値)" + balanceBoardData.sensorLoad.BottomLeft.ToString("f2") + "\n";

	}
}