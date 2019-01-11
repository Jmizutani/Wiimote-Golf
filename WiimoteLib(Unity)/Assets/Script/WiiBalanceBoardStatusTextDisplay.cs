using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class WiiBalanceBoardStatusTextDisplay : WiiBalanceBoardDisplayTextBase
{
	
	override protected void Output(){
		text.text = "xacc:"+ balanceBoardData.xacc.ToString("f2")+"[kg]\n";
		text.text += "yacc:" + balanceBoardData.yacc.ToString("f2") + "[cm]\n";
		text.text += "zacc:" + balanceBoardData.zacc.ToString("f2") + "[cm]\n";
        //text.text += "yaw: " + balanceBoardData.yaw.ToString("f2") + "\n";
        //text.text += "pitch: " + balanceBoardData.pitch.ToString("f2") + "\n";
        //text.text += "roll" + balanceBoardData.roll.ToString("f2")+"\n;

        

		
	}
}