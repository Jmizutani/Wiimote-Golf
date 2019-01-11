using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//デバッグ用
//受信メッセージを表示し続ける
public class WiiBalanceBoardMessageDisplay : WiiBalanceBoardDisplayTextBase
{
	string recvMessage = "";
	//float weight = 0f;
	//Vector2 copPos;
	override protected void Output(){
		recvMessage = wiiBalanceBoardCliant.recvBalanceBoardDatalist.message; 
		text.text = "ReceievedMessage:" + recvMessage;
	}

}