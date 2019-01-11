using UnityEngine;
using System.Collections;

//情報取得（input）と表示など（Output）の基本クラス
public class WiimoteInfoDisplayBase: MonoBehaviour
{
	///コントローラ番号
	public int index = 0;
	/// <summary>
	//（コントローラの情報をもっている）サーバと通信してるTCPクライアント
	[SerializeField]
	protected WiiBalanceBoardCliant wiiBalanceBoardCliant;
	//バランスボードのステータスを格納する構造体
	[SerializeField]
	protected BalanceBoardData balanceBoardData;

	//[Wii Controller informations]
//	protected BalanceBoardData 

	// Use this for initialization
	protected void Start ()
	{
		//通信クライアントコンポーネントへアクセスできるようにする
//		wiiBalanceBoardCliant = wiiBalanceBoardCliantObject.GetComponent<WiiBalanceBoardCliant> ();
		if (wiiBalanceBoardCliant == null) {
			Debug.LogError ("WiiBalanceBoardCliant is null");
		}
	}

	// Update is called once per frame
	void Update ()
	{
		//WiiバランスボードのInputを取得する
		if (!GetInputData ()) {
			//データが取得できなかったら表示せず次のフレームへ
			return;
		}

		//表示やコントローラとしての利用など。
		//中身は子クラスで実装して下さい
		Output ();
	}

	//
	/// <summary>
	/// Gets the data.
	/// </summary>
	/// <returns><c>true</c>, if data was gotten, <c>false</c> otherwise.</returns>
	bool GetInputData ()
	{
		//通信クライアントにアクセスできない
		if (wiiBalanceBoardCliant == null) {
			Debug.LogError ("BalanceBoardCliant is null");
			return false;
		}
		//バランスボードが接続されていない
		if (wiiBalanceBoardCliant.recvBalanceBoardDatalist.balanceBoardData == null) {
			Debug.LogError ("リストがありません");
			return false;
		}
		//表示対象のコントローラがない
		if (!(index < wiiBalanceBoardCliant.recvBalanceBoardDatalist.balanceBoardData.Count)) {
			Debug.LogWarning ("デバイスの数が足りません index(コントローラ番号[0始まり]):" 
				+ index
				+ " count（総コントローラ数）:"
				+ wiiBalanceBoardCliant.recvBalanceBoardDatalist.balanceBoardData.Count);
			return false;
		}

		//データ取得部
		balanceBoardData = wiiBalanceBoardCliant.recvBalanceBoardDatalist.balanceBoardData [index];
		return true;
	}

	//子クラスで実装する
	protected virtual void Output ()
	{
	}

}
