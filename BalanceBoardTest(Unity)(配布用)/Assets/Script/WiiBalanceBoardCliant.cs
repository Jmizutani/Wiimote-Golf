using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

// 2016/03/17 written by meka 
// 本コード内では
// dobon!!様のDOBON.NET > プログラミング道 > .NET Tips　より、
// TCP/IP通信系のコード（MITライセンス）を改変して使用しています。
// http://dobon.net/vb/dotnet/internet/tcpclientserver.html 
//(Topページへのリンク　http://dobon.net/)

// 以下ライセンス許諾文表示
/*The MIT License (MIT)
Copyright (c) 2016 DOBON! <http://dobon.net>
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
//データ番号定義
//この順番で区切られたメッセージが来る
enum BalanceBoardInfo
{
	index,					//
	Weight,					//
	CoPPosX,				//
	CoPPosY,				//
	SensorLoadTopRight,		//
	SensorLoadTopLeft,		//
	SenserLoadBottomRight,	//
	SensorLoadBottomLeft,		//
	EnumMaxNumber			//個数チェック用
}

//複数のバランスボードのデータを保持するリスト
public class BalanceBoardDataList
{

	public List <BalanceBoardData> balanceBoardData = new List<BalanceBoardData>();
	public string message;	//デバッグ用に保持させる

	//string型のmessageを区切り、数値データとして変換する。
	public bool parseMessage(string message_in)
	{
		if(message_in == null){
			Debug.LogError("TCP message is null");
			return false;
		}
		message = message_in;

		string[] stArryData = message_in.Split(',');
		//Debug.LogWarning(stArryData[0]);
		//Debug.Log("個数："+ stArryData.Length+"要素数:"+(int)WiiBalanceBoardInfo.EnumMaxNumber);
		if(stArryData.Length < (int)BalanceBoardInfo.EnumMaxNumber){
			Debug.LogError("メッセージのデータ個数が不正です。データを破棄します。 個数："+ stArryData.Length+"正しい要素数:"+(int)BalanceBoardInfo.EnumMaxNumber);
			//データ個数が不正
			return false;
		}

		int _index;
		//コントローラ番号を見分ける
		if(!int.TryParse(stArryData[(int)BalanceBoardInfo.index].ToString(),out _index))
			Debug.LogWarning("failed to parse.index:" + stArryData[(int)BalanceBoardInfo.index]);

		//新しいコントローラを追加
		while(balanceBoardData.Count < _index+1 ){
			balanceBoardData.Add(new BalanceBoardData());
			Debug.Log("adding / data.Count:" + balanceBoardData.Count +" index");
		}


		//各値を変換
		//weight[kg]
		if(!float.TryParse(stArryData[(int)BalanceBoardInfo.Weight].ToString(),out balanceBoardData[_index].weight))
			Debug.LogWarning("failed to parse.wight:" + stArryData[(int)BalanceBoardInfo.Weight]);

		//Center of Pressure X[cm]
		if(!float.TryParse(stArryData[(int)BalanceBoardInfo.CoPPosX].ToString(),out balanceBoardData[_index].copPos.x))
			Debug.LogWarning("failed to parse.copX:" + stArryData[(int)BalanceBoardInfo.CoPPosX]);
		
		//Center of Pressure Y[cm]
		if(!float.TryParse(stArryData[(int)BalanceBoardInfo.CoPPosY].ToString(),out balanceBoardData[_index].copPos.y))
			Debug.LogWarning("failed to parse.copY:" + stArryData[(int)BalanceBoardInfo.CoPPosY]);

		//sensor weight data[]

		if(!float.TryParse(stArryData[(int)BalanceBoardInfo.SensorLoadTopRight].ToString(),out balanceBoardData[_index].sensorLoad.TopRight))
			Debug.LogWarning("failed to parse.SensorKgTopRight:" + stArryData[(int)BalanceBoardInfo.SensorLoadTopRight]);

		if(!float.TryParse(stArryData[(int)BalanceBoardInfo.SensorLoadTopLeft].ToString(),out balanceBoardData[_index].sensorLoad.TopLeft))
			Debug.LogWarning("failed to parse.SensorKgTopLeft:" + stArryData[(int)BalanceBoardInfo.SensorLoadTopLeft]);

		if(!float.TryParse(stArryData[(int)BalanceBoardInfo.SenserLoadBottomRight].ToString(),out balanceBoardData[_index].sensorLoad.BottomRight))
			Debug.LogWarning("failed to parse.SensorKgBottomRight:" + stArryData[(int)BalanceBoardInfo.SenserLoadBottomRight]);

		if(!float.TryParse(stArryData[(int)BalanceBoardInfo.SensorLoadBottomLeft].ToString(),out balanceBoardData[_index].sensorLoad.BottomLeft))
			Debug.LogWarning("failed to parse.SensorKgBottomLeft:" + stArryData[(int)BalanceBoardInfo.SensorLoadBottomLeft]);
		


		return true;
	}

}

//WiiBalanceBoard各センサーにかかっている重量（4か所）


/// <summary>
/// 受信データを意味のある形にしたもの.
/// </summary>
[System.Serializable]
public class BalanceBoardData
{
	/// <summary>
	/// WiiBalanceBoardの各センサにかかっている重量
	/// </summary>
	[System.Serializable]
	public struct SensorLoad
	{
		public float TopRight;
		public float TopLeft;
		public float BottomRight;
		public float BottomLeft;
	}

	public float weight;		//重量[kgf]
	public Vector2 copPos;		//荷重中心[cm]
	public SensorLoad sensorLoad;		//WiiBalanceBoard各センサーにかかっている重量（4か所）
}

[System.Serializable]
public class WiiBalanceBoardCliant : MonoBehaviour
{
	//Socket tcpSocket;
	TcpClient tcpClient;
	Thread readThread;
	bool flg_continue = true;
	public string DistIPAddress = "127.0.0.1";					//自分のPCを指すアドレス
	public int portNum = 8888;									//ポート番号
	public int timeout = 2000;

	//通信データ格納用データリスト
	//Wiiバランスボードのインスタンス別に管理
	public BalanceBoardDataList recvBalanceBoardDatalist = new BalanceBoardDataList();

	// Use this for initialization
	void Start ()
	{
		
		IPAddress serverIP = IPAddress.Parse (DistIPAddress);

		if (tcpClient != null)
			return;
		
		tcpClient = new TcpClient ();
		tcpClient.ReceiveTimeout = 2000;	//２秒でタイムアウト
		tcpClient.SendTimeout = 2000;		//２秒でタイムアウト
		tcpClient.Connect (serverIP, portNum);
		Debug.Log ("init client");

		readThread = new Thread (new ThreadStart (recvTask));		//スレッドで呼び出す関数を登録
		readThread.IsBackground = true;
		readThread.Start ();

	}

	// Update is called once per frame
	void Update ()
	{
		//なにもしない
	}

	void OnDestroy ()
	{
		flg_continue = false;
		//readThreadの終了待ち
		if (readThread != null) {
			Debug.Log ("waiting for read thread kill.");
			readThread.Join ();
			Debug.Log ("read thread killed");
		}
		//TCPクライアント（ソケット）を閉じる
		if (tcpClient != null) {
			Debug.Log ("tcpClient Closing...");
			tcpClient.Close ();
			Debug.Log ("tcpClient Closed.");
			
		}

		
	}

	//受信メッセージを処理
	private void recvTask ()
	{
		NetworkStream ns = tcpClient.GetStream ();
		if(ns == null ){
			Debug.LogError("tcpCliant.Getstream() is failed.");
			return;
		}
		Debug.Log ("GetStream Success");
		//networkStream setup
		ns.ReadTimeout = 2000;
		ns.WriteTimeout = 2000;
		System.Text.Encoding enc = System.Text.Encoding.UTF8;


		byte[] resBytes = new byte[1];									//一文字づつ読ませるため1個ぶん
		//スレッド内で回し続ける
		while (flg_continue) {
			//読み取り、書き込みのタイムアウトを10秒にする
			//デフォルトはInfiniteで、タイムアウトしない
			//(.NET Framework 2.0以上が必要)

			//サーバーから送られたデータを受信する
			System.IO.MemoryStream ms = new System.IO.MemoryStream ();

			int resSize = 0;
			do {
				//データの一部を受信する
				//					Debug.Log ("ns.Read()");
				resSize = ns.Read (resBytes, 0, resBytes.Length);
				//					Debug.Log ("readed... " + resSize + " byte");

				if (!(resSize > 0))
					break;													//通信に返答なし

				//受信したデータをmemoryStreamへ蓄積する
				ms.Write (resBytes, 0, resSize);
				//まだ読み取れるデータがあり、データの最後が\n（改行コード）でない時は受信を続ける
				//改行コードがきたら、それがデータの区切りなので切り出す
			} while(resBytes [resSize - 1] != '\n'&& ns.DataAvailable);

			//受信したデータをmemoryStreamから吐き出す
			string resMsg = enc.GetString (ms.GetBuffer (), 0, (int)ms.Length);
			recvBalanceBoardDatalist.parseMessage(resMsg);
			//holdMessage(resMsg);
			ms.Close ();

		}
		Thread.Sleep (1);
	}


}


