using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;


// 本コード(WiimoteTCPServer.CS)内では
// dobon!!様のDOBON.NET > プログラミング道 > .NET Tips　より、
// TCP/IP通信系のコード（MITライセンス）を改変して使用しています。
// http://dobon.net/vb/dotnet/internet/tcpclientserver.html 
// 以下ライセンス許諾文表示
/*The MIT License (MIT)
Copyright (c) 2016 DOBON! <http://dobon.net>
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

/// <summary>
/// Wiiコントローラの情報を渡すためのサーバ
/// </summary>
namespace TCPComm
{

    public class WiimoteTCPServer
    {
        List<WiimoteMessage> sendData = new List<WiimoteMessage>(4);   //配列一つに1コントローラ分の情報を格納する。

        bool _continueFlg = true;
        Thread readThread = null;
        Thread sendThread = null;
        NetworkStream ns;
        TcpClient client;
        TcpListener listener;

        System.Text.Encoding enc = System.Text.Encoding.UTF8;
        bool SocketIsConnected = false;
        bool disconnected = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port">Listenするポート番号</param>
        /// <param name="distIPAdressString">接続先IPアドレス</param>
        /// <returns></returns>
        public bool Initalize(int port, string distIPAdressString)
        {
            Console.WriteLine("InitializeSocket.");
            System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(distIPAdressString);

            //ホスト名からIPアドレスを取得する時は、次のようにする
            //string host = "localhost";
            //System.Net.IPAddress ipAdd =
            //    System.Net.Dns.GetHostEntry(host).AddressList[0];
            //.NET Framework 1.1以前では、以下のようにする
            //System.Net.IPAddress ipAdd =
            //    System.Net.Dns.Resolve(host).AddressList[0];


            //TcpListenerオブジェクトを作成する
            listener =
                new System.Net.Sockets.TcpListener(ipAdd, port);

            //Listenを開始する
            listener.Start();
            Console.WriteLine("Listenを開始しました({0}:{1})。",
                ((System.Net.IPEndPoint)listener.LocalEndpoint).Address,
                ((System.Net.IPEndPoint)listener.LocalEndpoint).Port);

            //接続要求があったら受け入れる
            client = listener.AcceptTcpClient();
            Console.WriteLine("クライアント({0}:{1})と接続しました。",
                ((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Address,
                ((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Port);

            //NetworkStreamを取得
            ns = client.GetStream();

            //読み取り、書き込みのタイムアウトを10秒にする
            //デフォルトはInfiniteで、タイムアウトしない
            //(.NET Framework 2.0以上が必要)
            ns.ReadTimeout = 2000;
            ns.WriteTimeout = 2000;


            SocketIsConnected = true;

            //受信Threadを作成
            // 受信スレッドを作成・実行
            //クライアントからコマンドを受信（子スレッド内にて）
            //今は特に何もやっていない。クライアントからの要求を受けたいときに実装する
            Console.WriteLine("ReadThreadを作成します");
            readThread = new Thread(new ThreadStart(ReadData));
            readThread.IsBackground = true;
            readThread.Start();

            //クライアントへコントローラ情報を送信（子スレッド内にて）
            Console.WriteLine("SendThreadを作成します");
            sendThread = new Thread(new ThreadStart(SendData));
            sendThread.IsBackground = true;
            sendThread.Start();

            return true;

        }

        public bool Finalize()
        {
            //子スレッドを閉じる
            CloseChildThreads();

            Console.WriteLine("FinalizeSocket()");
            //ストリーム、ソケットを閉じる
            ns.Close();
            client.Close();
            disconnected = true;

            Console.WriteLine("クライアントとの接続を閉じました。");

            //リスナを閉じる
            listener.Stop();
            Console.WriteLine("Listenerを閉じました。");

            Console.ReadLine();
            SocketIsConnected = false;
            return true;
        }


        public bool UpdateMessage(int index, WiimoteMessage message_in)
        {
            //Listがない
            if (sendData == null)
            {
                Console.WriteLine("sendData List is null.");
                return false;
            }
            //コントローラ番号が用意されてない
            if (sendData.Count <= index)
            {
                Console.WriteLine("Controller Index size is wrong.");
                return false;
            }

            sendData[index] = message_in;
            return true;
        }

        /// <summary>
        /// 子スレッドを閉じる
        /// </summary>
        /// <returns></returns>
        public bool CloseChildThreads()
        {
            //////////////////////
            //threadのloopフラグを下げる
            _continueFlg = false;

            //Readスレッドの終了待ち
            Console.WriteLine("Wating dead readThread...");
            readThread.Join();
            Console.WriteLine("readThread end.");
            //SendThreadの終了待ち
            Console.WriteLine("wating dead sendThread ...");
            sendThread.Join();
            Console.WriteLine("sendThread end.");
            return true;
        }

        //送信対象となるコントローラ数を増やす

        public void AddController()
        {
            sendData.Add(new WiimoteMessage());
        }

        //データの送信（(Wiiコントローラ)->Server->Unity）
        void SendData()
        {

            while (_continueFlg)
            {
                Console.WriteLine("SendData_thread");

                if (disconnected)
                {
                    Console.WriteLine("Disconnected.");
                    continue;
                }
                if (ns == null)
                {
                    Console.WriteLine("socket is null.");
                    continue;
                }

#if DEBUG
                Console.WriteLine("MessageMaking...");
#endif
                //クライアントに送信する文字列
                for (int i = 0; i < sendData.Count; i++)
                {
                    //i:コントローラ番号
                    string msg = sendData[i].GetMessage();      //コントローラから送信用stringを取得

                    //クライアントにデータを送信する
                    //文字列をByte型配列に変換
                    byte[] sendBytes = enc.GetBytes(msg);
                    try
                    {
                        ns.Write(sendBytes, 0, sendBytes.Length);
                        Console.WriteLine(msg);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }

                Thread.Sleep(1);
            }
            return;

        }

        //データの受信(Unity->Server)
        void ReadData()
        {
        }

    }


    //TCPクライアントへ横流しするステータスをまとめたもの
    public class WiimoteMessage
    {

        public int index;            //コントローラ番号　0始まり

        //4センサから計算された値
        public float weight;            //重量[kgf]
        public float copPosX;           //圧力中心位置[cm]
        public float copPosY;           //圧力中心位置[cm]

        //4センサ生重量
        public float loadTopRight;       //右前
        public float loadTopLeft;        //左前
        public float loadBottomRight;    //右後
        public float loadBottomLeft;     //左後
        //通信用の格納データ
        private string message;

        //ステータスを送信用文字列へ変換して返す
        public string GetMessage()
        {
            message =
                index + ","
                + weight + ","
                + copPosX + ","
                + copPosY + ","
                + loadTopRight + ","
                + loadTopLeft + ","
                + loadBottomRight + ","
                + loadBottomLeft
                + "\n";

            return message;
        }

    }

}