・はじめに
-本ソフトウェア群ではバランスWiiボード※をUnityアプリのコントローラとして使用するためのサンプルとして製作しました。


・使い方
-PCとバランスボードをあらかじめペアリングしておく必要があります。（ペアリングの方法は各自でお調べください。）

BalanceBoardTest-|-BalanceboardTest.unitypackage :
                 |-WiimoteServer		　　 :
の構成で配布しています。

BalanceboardTest.unitypacageを適当なプロジェクトで展開後、
（プロジェクトフォルダ）
|-Assets
|-Library
|-ProjectSettings

↓

（プロジェクトフォルダ）
|-Assets
|-Library
|-ProjectSettings
|-WiimoteServer
となるように、WiimoteServerフォルダを移動して下さい。

シーン：WiiBalanceBoardTest　を開き、実行すると
UnityからWiimoteServer/WiimoteTest.exeを立ち上げ、バランスボードとの通信が行われます。
バランスボード -> WiimoteTest.exe -> Unity
という形でセンサ値を渡しています。 

・ライセンス概略（詳細はLicense.txtをご覧ください。）
本ソフトはMicrosoft Public Licenseを適応しています。
[Unity側]
dobon!!様のDOBON.NET > プログラミング道 > .NET Tips　より、
(http://dobon.net/)
TCP/IP通信系のコード（MITライセンス）を改変して使用しています。
-----------------------------------------------------------
[サーバアプリ側]
/WiimoteServer　フォルダ以下

WiimoteLib1.7（Copyright 2007-2009 Brian Peek）
http://www.brianpeek.com/
http://www.codeplex.com/WiimoteLib
http://www.wiimotelib.org/
より、SampleCS以下のプロジェクト(Microsoft Permissive License)
を改変・ビルドしています。
-----------------------------------------------------------
・更新履歴
2016/03/26 Ver01 作成
-バランスボードとの複数接続可能に
-重量読み取り
-各センサー値読み取り
-COP（荷重中心）読み取り可能に
-軌跡表示

未実装
-ゼロ点調整
-センサーキャリブレーション
----------------------------------------------------------
・連絡先
HandleName：Meka
e-mail:kto1989@hotmail.com
Twitter:meka@kikatyan

※バランスWiiボード、Wiiは任天堂株式会社の商標です。