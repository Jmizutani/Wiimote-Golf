using UnityEngine;
using System.Collections;
using System;

// 2016/03/17 written by meka
// 本コード内では
// dobon!!様のDOBON.NET > プログラミング道 > .NET Tips　より、
//	外部プロセスの実行系のコード(MITライセンス）を改変して使用しています。
//(Topページへのリンク　http://dobon.net/)
//http://dobon.net/vb/dotnet/process/openfile.html				（外部プロセスの実行、イベントハンドラの登録）
//http://dobon.net/vb/dotnet/process/processwindowstyle.html	(ウィンドウを非アクティブにして実行)
//http://dobon.net/vb/dotnet/process/shell.html					（StartInfoの使い方）
//http://dobon.net/vb/dotnet/process/killprocesse.html			(外部プロセスを終了させる)
// 以下、dobon!!様のコードのライセンス許諾文表示
/*The MIT License (MIT)
Copyright (c) 2016 DOBON! <http://dobon.net>
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

//Todo::多重起動対策
//Wiimoteサーバーアプリを起動するランチャ

public class WiimoteServerLancher : MonoBehaviour
{
	//外部プロセスのプロセスオブジェクト
	System.Diagnostics.Process wiimoteServerProcess;

	//サーバアプリをバックグラウンドで起動するか
	public bool hideServerApplicationWindow = false;

	//サーバアプリを自動で起動するか サーバアプリの開発時などにfalseにして使う
	public bool lunchTheServerProcess = true;

	//与える引数(現在未使用)
	public string argments = "";

	//このフォルダと.exeはあらかじめ作っておく
	public string applicationPath = "/WiimoteServer/WiimoteTest.exe";

	//クライアントが(Start関数で)起動する前にサーバを起動させたいので、
	//Awakeでサーバを起動する
	void Awake ()
	{
		if(!lunchTheServerProcess)
			return;
		
		string dir = "";
		string filepath = "";				//起動したいアプリケーション
		//エディター時とビルド済み実行ファイルで同じ場所を見るようにする
		//アプリと同じ階層をさがすように設定
		if (Application.isEditor) {
			dir = Application.dataPath + "/..";
		} else {
			dir = Application.dataPath + "/..";
		}
		//起動するファイルのフルパス
		filepath = dir + applicationPath;	

		Debug.Log (filepath);

		//Processオブジェクトを作成する
		wiimoteServerProcess = new System.Diagnostics.Process ();
		//起動するファイルを指定する
		wiimoteServerProcess.StartInfo.FileName = filepath;
		//イベントハンドラの追加
		wiimoteServerProcess.Exited += new EventHandler (wiimoteServerProcess_Exited);
		//プロセスが終了したときに Exited イベントを発生させるように設定
		wiimoteServerProcess.EnableRaisingEvents = true;
		//引数を指定する
		wiimoteServerProcess.StartInfo.Arguments = argments;
		//(フラグを見て)外部プロセスをバックグラウンドで起動するように設定
		if (hideServerApplicationWindow) {
			wiimoteServerProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		}
		//起動する
		try {
			wiimoteServerProcess.Start ();
		} catch (Exception e) {
			//外部プロセスをスタートできなかったらエラー表示
			Debug.LogError ("Failed to start process." + e.Message);
		}

	}


	//実行終了時に外部プロセスが生きているなら自動終了させる
	void OnApplicationQuit ()
	{
		if(!lunchTheServerProcess)
			return;
		
		if (wiimoteServerProcess.HasExited) {
			Debug.Log ("外部プロセスは終了済みです");
			return;
		}

		//まだ外部プロセスが動いていれば終了させる。
		try {
			//メインウィンドウを閉じる
			wiimoteServerProcess.CloseMainWindow ();
			//プロセスが終了するまで最大2秒待機する
			wiimoteServerProcess.WaitForExit (2);
			//プロセスが終了したか確認する
			if (wiimoteServerProcess.HasExited) {
				Debug.Log ("外部プロセスが終了しました");
			} else {
				Debug.LogError ("外部プロセスが終了しませんでした。強制終了させます。");
				wiimoteServerProcess.Kill ();		///プロセスを強制終了
			}
		} catch (Exception e) {
			Debug.LogError (e.Message);
		}

	}



	//外部プロセス終了を検知して呼び出される関数
	void wiimoteServerProcess_Exited (object sender, System.EventArgs e)
	{
		if(!lunchTheServerProcess)
			return;
		
		UnityEngine.Debug.Log ("WiimoteProssess_ExitEvent");
		wiimoteServerProcess.Dispose();						//プロセスを破棄
	}




}
