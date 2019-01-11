using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WiimoteLib;           
using TCPComm;              //WiimoteのデータをTCPクライアントへ流すためのnamespace

// 2016/03/17 written by meka
//このプロジェクト一式は
//http://wiimotelib.codeplex.com/
//MicrosoftPrmissiveLicenceで提供された、
//WiimoteLib1.7/SampleCSを使用しています。



namespace WiimoteTest
{

    public partial class MultipleWiimoteForm : Form
    {
        // map a wiimote to a specific state user control dealie
        Dictionary<Guid, WiimoteInfo> mWiimoteMap = new Dictionary<Guid, WiimoteInfo>();
        WiimoteCollection mWC;
        WiimoteState wiimoteState = null;


        ///TCP Server
        WiimoteTCPServer server = new WiimoteTCPServer();

        public MultipleWiimoteForm()
        {
            InitializeComponent();
            Console.WriteLine("MultipleWiimoteForm()");
            server.Initalize(8888,"127.0.0.1");         //(port,dist address)
        }


        //プログラムロード時に走るコード
        private void MultipleWiimoteForm_Load(object sender, EventArgs e)
        {
            // find all wiimotes connected to the system
            mWC = new WiimoteCollection();
            int index = 0;
            Console.WriteLine("MultipleWiimoteForm_Load()");

            try
            {
                Console.WriteLine("mWC.FindAllWiimotes()...");
                mWC.FindAllWiimotes();              //検索
            }
            catch (WiimoteNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Wiimote not found error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (WiimoteException ex)
            {
                MessageBox.Show(ex.Message, "Wiimote error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //発見した子機の数だけイベントハンドラを登録/通信接続
            foreach (Wiimote wm in mWC)
            {
                // create a new tab
                TabPage tp = new TabPage("Wiimote " + index);
                tabWiimotes.TabPages.Add(tp);

                // create a new user control
                WiimoteInfo wi = new WiimoteInfo(wm);
                wi.index = index;                       ///コントローラ番号を付記
                tp.Controls.Add(wi);

                //サーバで送信するコントローラの情報枠を増やす
                server.AddController();

                // setup the map from this wiimote's ID to that control
                mWiimoteMap[wm.ID] = wi;

                // connect it and set it up as always
                wm.WiimoteChanged += wm_WiimoteChanged;
                wm.WiimoteExtensionChanged += wm_WiimoteExtensionChanged;

                wm.Connect();
                if (wm.WiimoteState.ExtensionType != ExtensionType.BalanceBoard)
                    wm.SetReportType(InputReport.IRExtensionAccel, IRSensitivity.Maximum, true);

                //LEDをつける（コントローラ）
                wm.SetLEDs(index + 1);
                index++;
            }
        }

        //wiimoteのステータスが変わったのを検知して起動するイベント
        void wm_WiimoteChanged(object sender, WiimoteChangedEventArgs e)
        {
            //表示系（Form）の情報更新
            WiimoteInfo wi = mWiimoteMap[((Wiimote)sender).ID];
            wi.UpdateState(e);

            //WiimoteStateの値を取得
            wiimoteState = e.WiimoteState;

            switch (wiimoteState.ExtensionType)
            {
                //接続相手がbalanceboardであれば
                case ExtensionType.BalanceBoard:

                    //デバッグ表示用
                    //重さ(Kg)
                    string weight = wiimoteState.BalanceBoardState.WeightKg + "kg";
                    //重心のX座標
                    string posX = "X:" +
                        wiimoteState.BalanceBoardState.CenterOfGravity.X;
                    //重心のY座標
                    string posY = "Y:" +
                        wiimoteState.BalanceBoardState.CenterOfGravity.Y;

                    //送信用データリストを更新する
                    //ここで横流しするデータを更新している
                    WiimoteMessage message = new WiimoteMessage();                                                      //データ用意
                    message.index = wi.index;
                    message.weight = wiimoteState.BalanceBoardState.WeightKg;
                    message.copPosX = wiimoteState.BalanceBoardState.CenterOfGravity.X;
                    message.copPosY = wiimoteState.BalanceBoardState.CenterOfGravity.Y;

                    message.loadTopRight = wiimoteState.BalanceBoardState.SensorValuesKg.TopRight;
                    message.loadTopLeft = wiimoteState.BalanceBoardState.SensorValuesKg.TopLeft;
                    message.loadBottomRight = wiimoteState.BalanceBoardState.SensorValuesKg.BottomRight;
                    message.loadBottomLeft = wiimoteState.BalanceBoardState.SensorValuesKg.BottomLeft;

                    server.UpdateMessage(wi.index,message);                                                             //更新

                    break;

            }

        }

        void wm_WiimoteExtensionChanged(object sender, WiimoteExtensionChangedEventArgs e)
        {
            // find the control for this Wiimote
            WiimoteInfo wi = mWiimoteMap[((Wiimote)sender).ID];
            wi.UpdateExtension(e);

            if (e.Inserted)
                ((Wiimote)sender).SetReportType(InputReport.IRExtensionAccel, true);
            else
                ((Wiimote)sender).SetReportType(InputReport.IRAccel, true);
        }

        private void MultipleWiimoteForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //threadを閉じる
            server.CloseChildThreads();
            //ソケットを閉じる
            server.Finalize();

            //wiimoteを切断
            foreach (Wiimote wm in mWC)
                wm.Disconnect();

        }

        /*
                //TCP/IP Socketの設定
                private bool InitializeSocket()
                {
                    Console.WriteLine("InitializeSocket.");
                    string ipString = "127.0.0.1";                                                   //自PC
                    System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(ipString);

                    //ホスト名からIPアドレスを取得する時は、次のようにする
                    //string host = "localhost";
                    //System.Net.IPAddress ipAdd =
                    //    System.Net.Dns.GetHostEntry(host).AddressList[0];
                    //.NET Framework 1.1以前では、以下のようにする
                    //System.Net.IPAddress ipAdd =
                    //    System.Net.Dns.Resolve(host).AddressList[0];

                    //Listenするポート番号
                    int port = 8888;

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

                    Console.WriteLine("ReadThreadを作成します");
                    readThread = new Thread(new ThreadStart(ReadData));
                    readThread.IsBackground = true;
                    readThread.Start();

                    //
                    Console.WriteLine("SendThreadを作成します");
                    sendThread = new Thread(new ThreadStart(SendData));
                    sendThread.IsBackground = true;
                    sendThread.Start();

                    return true;
                }

                //TCP/IP Socketの終了処理
                private bool FinalizeSocket()
                {
                    //            Console.WriteLine("ReadThreadの終了を待っています。");
                    //            readThread.Join();                                                  //readThreadの//終了待ち
                    //            Console.WriteLine("ReadThreadを終了しました。");

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

                //通信先からのコマンド受け取り
                public void ReadData()
                {
                    while (_continueFlg)
                    {
                        //                Console.WriteLine("ReadData()");
                        //特に何もさせていない
                    }
                }

                //センサー情報をTCPソケットへ送信
                public void SendData()
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
                        //これいらなくない？
                        if (wiimoteState == null)
                        {
                            Console.WriteLine("wiimotestate is null.");
                            continue;
                        }
                        //クライアントに送信する文字列
        #if DEBUG
                        Console.WriteLine("MessageMaking...");
        #endif
                        List<string> sendMsg = new List<string>(4);


                        //sendMsg = "[sendedData]" +"\n" ;
                        for (int i = 0; i < sendData.Count; i++)
                        {
                            sendMsg.Add("");
                            sendMsg[i] =
                                sendData[i].index + ","
                                + sendData[i].weight + ","
                                + sendData[i].copPosX + ","
                                + sendData[i].copPosY
                                + "\n";

                            //クライアントにデータを送信する
                            //文字列をByte型配列に変換
                            byte[] sendBytes = enc.GetBytes(sendMsg[i]);
                            //データを送信する
                            //int datasize = socket.Send(sendBytes);
                            try
                            {
                                ns.Write(sendBytes, 0, sendBytes.Length);
                                Console.WriteLine(sendMsg[i]);
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



                */
    }
}
