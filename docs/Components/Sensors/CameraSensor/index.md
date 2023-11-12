# CameraSensor

## はじめに
`CameraSensor`は*RGB*カメラをシミュレートするコンポーネントです。
自動運転車はさまざまな目的で使用される多くのカメラを装備することがあります。
現在の*AWSIM*のバージョンでは、カメラは主に*Autoware*の信号機認識モジュールに画像を提供するために使用されています。

### プレハブ
プレハブは以下のパスにあります:

```
Assets/AWSIM/Prefabs/Sensors/CameraSensor.prefab
```

### デフォルトシーンへのリンク
言及された単一の`CameraSensor`は、そのデータが公開されるフレーム`traffic_light_left_camera/camera_link`を持っています。
センサープレハブはこのフレームに追加されます。
`traffic_light_left_camera/camera_link`リンクは、`URDF`内の`base_link`オブジェクトに追加されます。

![リンク](link.png)

`URDF`構造とプレハブ`Lexus RX450h 2015`に追加されたセンサーの詳細な説明は、この[セクション](../../../Components/Vehicle/URDFAndSensors/)で提供されています。


## CameraSensorHolder（スクリプト）
![InspectorSetup](InspectorSetup.png)

*CameraSensorHolder*（スクリプト）は複数のカメラセンサーの順次レンダリングを許可します。
それを利用するには、各`CameraSensor`オブジェクトを`CameraSensorHolder`の子オブジェクトとしてアタッチする必要があります。
![シーンオブジェクト階層](SceneObjectHierarchy.png)

#### エディターレベルで設定可能な要素
- `Camere Sensors` - レンダリングに使用されるカメラセンサーのコレクション
- `Publish Hz` - カメラのレンダリング、画像処理、およびコールバックが実行される頻度
- `Render In Queue` - カメラセンサーのレンダリングシーケンスタイプ：*キュー内（一つずつ）*または*すべて同じフレームで*

### カメラセンサーコンポーネント
![コンポーネント](components.png)

`CameraSensor`が正しく機能するためには、スクリプトが追加された*GameObject*にも以下が必要です：

- [*カメラコンポーネント*](https://docs.unity3d.com/Manual/class-Camera.html) - カメラの機能をUnity内のデバイスとして提供し、プレイヤーに世界をキャプチャして表示する基本コンポーネント。
- [*HD追加カメラデータ*（スクリプト）](https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@13.1/api/UnityEngine.Rendering.HighDefinition.HDAdditionalCameraData.html) - カメラに対する[*HDRP*](https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@16.0/manual/index.html)固有のパラメータを保持する追加のコンポーネントです。このコンポーネントは[*カメラコンポーネント*](https://docs.unity3d.com/Manual/class-Camera.html)と一緒に自動的に追加されるべきです。

!!! ヒント "信号機認識"
    *Autoware*で信号機の認識に問題がある場合、*AWSIM*のカメラの画像解像度と焦点距離を増やすと助けになるかもしれません。

!!! ヒント "カメラの設定"
    カメラがキャプチャした画像を調整したい場合は、[このマニュアル](https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@11.0/manual/HDRP-Camera.html)を読むことをお勧めします。

`CameraSensor`の機能は2つのスクリプトに分かれています：

- *カメラセンサー*（スクリプト） - *Unity*の[カメラ](https://docs.unity3d.com/ScriptReference/Camera.html)から画像を取得し、変換し、*BGR8*形式で保存します。この形式はカメラのパラメータと一緒にスクリプトの出力です。スクリプトはまた、それのためのコールバックも呼び出します。
- *カメラRos2パブリッシャー*（スクリプト） - `CameraSensor`の出力を特定の*ROS2*トピックで[Image][image_msg]と[CameraInfo][camera_info_msg]メッセージタイプとしてパブリッシュする機能を提供します。

スクリプトは次のパスにあります：

```
Assets/AWSIM/Scripts/Sensors/CameraSensor/*
```

同じ場所には、使用される[`ComputeShader`](https://docs.unity3d.com/ScriptReference/ComputeShader.html)が含まれている`*.compute`ファイルもあります。

## カメラセンサー（スクリプト）
![スクリプト](script.png)

*カメラセンサー*（スクリプト）は、コアのカメラセンサーコンポーネントです。これは*OpenCV*の歪みと*BGR8*形式へのエンコードを適用する責任があります。歪みモデルは*プラムボブ*と想定されています。このスクリプトは[カメラ](https://docs.unity3d.com/ScriptReference/Camera.html)から画像を[`Texture2D`](https://docs.unity3d.com/ScriptReference/Texture2D.html)にレンダリングし、歪みパラメータを使用して変換します。この画像はGUIに表示され、*BGR8*形式のバイトリストをスクリプトの出力として取得するためにさらに処理されます。

このスクリプトは2つの[`ComputeShader`](https://docs.unity3d.com/ScriptReference/ComputeShader.html)を使用します。これらはスクリプトと同じ場所にあります：

- `CameraDistortion` - カメラの歪みパラメータを使用して画像を修正するためのもの。
- `RosImageShader` - 2つのピクセルカラー（*bgr8* - 3バイト）を1つの（*uint32* - 4バイト）にエンコードして*ROSイメージ* *BGR8*バッファを生成するためのものです。

    ![コンピュート](compute.png)

| API      | タイプ | 機能                                                                                   |
| :------- | :--- | :------------------------------------------------------------------------------------- |
| DoRender | void | Unityカメラをレンダリングし、レンダリングされたイメージにOpenCVの歪みを適用し、出力データを更新します。 |

以下は、エディターレベルから設定可能な要素です。

- `Output Hz` - 出力の計算とコールバックの頻度（デフォルト: `10Hz`）
- *GUI上の画像*:
    - `表示` - カメラ画像をGUI上に表示するかどうか（デフォルト: `true`）
    - `スケール` - カメラからの画像の縮小率、`1`は実際のサイズの画像を提供し、`2`は2倍小さくなります（デフォルト: `4`）
    - `X軸` - 表示される画像の左上隅のX軸での位置、`0`は左端です（デフォルト: `0`）
    - `Y軸` - 表示される画像の左上隅のY軸での位置、`0`は上端です（デフォルト: `0`）
- *カメラパラメータ*
    - `幅` - 画像の幅（デフォルト: `1920`）
    - `高さ` - 画像の高さ（デフォルト: `1080`）
    - `K1, K2, P1, P2, K3` - *プラムボブ*モデルのカメラ歪み係数（デフォルト: `0, 0, 0, 0, 0`）
- `カメラオブジェクト` - 基本的な[*カメラコンポーネント*](https://docs.unity3d.com/Manual/class-Camera.html)への参照（デフォルト: `None`）
- `歪みシェーダー` - *歪みシェーダー*の機能に関する[*ComputeShader*](https://docs.unity3d.com/ScriptReference/ComputeShader.html)アセットへの参照（デフォルト: `None`）
- `Rosイメージシェーダー` - *Rosイメージシェーダー*の機能に関する[*ComputeShader*](https://docs.unity3d.com/ScriptReference/ComputeShader.html)アセットへの参照（デフォルト: `None`）

#### 出力データ
センサー計算の出力形式は以下の通りです：

|      カテゴリー      |       タイプ       | 説明                   |
| :----------------: | :--------------: | :---------------------------- |
| *ImageDataBuffer*  |     byte[ ]      | 画像データのバッファ       |
| *CameraParameters* | CameraParameters | カメラパラメータのセット |

## CameraRos2Publisher（スクリプト）
![script_ros2](script_ros2.png)

`CameraSensor`からのデータ出力を*ROS2*の [Image][image_msg] および [CameraInfo][camera_info_msg] タイプのメッセージに変換し、それらを公開します。
変換および公開は、現在の出力の`callback`である `Publish(CameraSensor.OutputData outputData)` メソッドを使用して実行されます。

常に全体の画像が公開されるため、メッセージの [`ROI`](https://docs.ros2.org/latest/api/sensor_msgs/msg/RegionOfInterest.html) フィールドは常にゼロで埋められています。
スクリプトはまた、`binning` がゼロであると仮定し、補正行列が単位行列であることを確認します。

!!! 警告
    このスクリプトは *CameraSensorスクリプト* で設定されたカメラパラメータを使用します - 使用するカメラに応じてそれらを設定することを忘れないでください。

以下はエディターレベルから設定可能な要素です。

- `イメージトピック` - [`Image`][image_msg] メッセージが公開される *ROS2* トピック（デフォルト: `"/sensing/camera/traffic_light/image_raw"`）
- `カメラインフォトピック` - [`CameraInfo`][camera_info_msg] メッセージが公開される *ROS2* トピック（デフォルト: `"/sensing/camera/traffic_light/camera_info"`）
- `フレームID` - データが公開されるフレーム、[`Header`](https://docs.ros2.org/latest/api/std_msgs/msg/Header.html)で使用されます（デフォルト: `"traffic_light_left_camera/camera_link"`）
- `QoS設定` - 公開に使用される品質サービスプロファイル（デフォルト: `ベストエフォート`、`ボラタイル`、`最後の状態を保持`、`1`）

#### Publishされるトピック
- 周波数: `10Hz`
- QoS: `ベストエフォート`、`ボラタイル`、`最後の状態を保持/1`

|    カテゴリー    | トピック                                       | メッセージタイプ                                |               `frame_id`                |
| :------------: | :------------------------------------------ | :------------------------------------------ | :-------------------------------------: |
| *カメラ情報*  | `/sensing/camera/traffic_light/camera_info` | [`sensor_msgs/CameraInfo`][camera_info_msg] | `traffic_light_left_camera/camera_link` |
| *カメラ画像* | `/sensing/camera/traffic_light/image_raw`   | [`sensor_msgs/Image`][image_msg]            | `traffic_light_left_camera/camera_link` |

[image_msg]: https://docs.ros2.org/latest/api/sensor_msgs/msg/Image.html
[camera_info_msg]: https://docs.ros2.org/latest/api/sensor_msgs/msg/CameraInfo.html