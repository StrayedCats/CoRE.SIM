<!-- reusable hyperlinks -->
[qos]: https://docs.ros.org/en/humble/Concepts/About-Quality-of-Service-Settings.html
[header]: https://docs.ros2.org/latest/api/std_msgs/msg/Header.html
[time]: https://docs.ros2.org/latest/api/builtin_interfaces/msg/Time.html

# ROS2 For Unity
[*Ros2ForUnity*](https://github.com/RobotecAI/ros2-for-unity)（`R2FU`）モジュールは、*Unity*と*ROS2*エコシステムを効果的に接続するコミュニケーションソリューションで、強力な統合を維持します。
他のソリューションとは異なり、通信をブリッジングに依存せず、むしろ*ROS2*ミドルウェアスタック（特に`rcl`レイヤーおよびそれ以下）を利用して、*Unity*シミュレーション内で*ROS2*ノードを含めることができます。

`R2FU`は多くの理由で*CoRE.SIM*で使用されています。
まず第一に、*Unity*と*ROS2*間の高性能な統合を提供し、ブリッジングソリューションと比較してスループットが向上し、レイテンシが低くなります。
*Unity*内のシミュレーションエンティティに対して実際の*ROS2*機能を提供し、標準およびカスタムメッセージをサポートし、*Unity*アセットとしてラップされた便利な抽象化とツールを含んでいます。
詳細な説明については、[*README*](https://github.com/RobotecAI/ros2-for-unity/blob/master/README.md)をご覧ください。

## 前提条件
このアセットは、2つの異なるモードで準備できます：

- *スタンドアロンモード* - このモードでは、対象のマシンに *ROS2* のインストールが必要ありません。例：*Unity* シミュレーションサーバー。
必要なすべての依存関係がインストールされ、完全な *Unity* プラグインセットとして使用できます。
- *オーバーレイモード* - このモードでは、対象のマシンに *ROS2* のインストールが必要です。
アセットライブラリと生成されたメッセージのみがインストールされるため、*ROS2* のインスタンスをソース化する必要があります。

デフォルトでは、*CoRE.SIM* のアセット `R2FU` は *スタンドアロンモード* で準備されています。
これにより、*ROS2* のインスタンスをソース化する必要がなく、*Unity* エディタを実行するだけで済みます。

!!! 質問 "トピックが表示されない"
    エラーは表示されませんが、`R2FU` によって発行されたトピックが表示されません。

    - DDS（[localhostの設定](../../../GettingStarted/QuickStartDemo/#localhost-settings)）の設定が正しいことを確認してください。
    - 時々 *ROS2* デーモンはネットワークインターフェースの変更や *ROS2* バージョンの変更時に中断することがあります。

    強制的に停止してみてください（`pkill -9 ros2_daemon`） そして再起動してください（`ros2 daemon start`）。

## Concept
「CoRE.SIM」内で「R2FU」の概念を説明する際、以下の点を区別します：

- *ROS2Node* - これは *ROS2* のノードに相当し、独自の名前を持ち、任意の数のサブスクライバ、パブリッシャ、サービスサーバ、およびサービスクライアントを持つことができます。現在の *CoRE.SIM* の実装では、メインノードは1つしかありません。
- *ROS2Clock* - これは選択したソースを使用してシミュレーション時間を生成する責任があります。
- *SimulatorROS2Node* - これは *CoRE.SIM<->ROS2* の通信に直接責任を負うクラスで、*ROS2Node* および *ROS2Clock* の各1つのインスタンスを含み、*ROS2* でメインの *CoRE.SIM* ノードを作成し、選択したソースから時間をシミュレーションします（現在は [UnityTimeSource](https://docs.unity3d.com/ScriptReference/Time.html) が使用されています）。これは *Unity* シーンを実行する瞬間に [`RuntimeInitializeOnLoadMethod`](https://docs.unity3d.com/ScriptReference/RuntimeInitializeOnLoadMethodAttribute.html) マークを使用して初期化されます。
- *Publisher* - これは *ROS2* のパブリッシャに相当し、選択したメッセージのタイプを発行できる単一のトピックを使用し、選択した [*QoS*][qos] プロファイルを持っています。*CoRE.SIM* 内の各パブリッシャは、*SimulatorROS2Node* クラスの *ROS2Node* オブジェクト内で作成されます。
- *Subscriber* - これは *ROS2* のサブスクライバに相当し、選択したメッセージのタイプをサブスクライブするための単一のトピックを使用し、選択した [*QoS*][qos] プロファイルを持っています。*CoRE.SIM* 内の各サブスクライバは、*SimulatorROS2Node* クラスの *ROS2Node* オブジェクト内で作成されます。

「SimulatorROS2Node」の実装は、「R2FU」の使用により、任意の Unity コンポーネントに対して *ROS2* を介した通信を追加することができます。例えば、他の *ROS2* ノードから制御コマンドを受信し、*Ego* の現在の状態（環境内の位置など）を発行することができます。

!!! ヒント "シミュレーション時間"
    *Unity* の時間の代わりにシステム時間（*ROS2* 時間）を使用したい場合は、`SimulatorROS2Node` クラスで `UnityTimeSource` の代わりに `ROS2TimeSource` を使用してください。


## パッケージ構造
`Ros2ForUnity` アセットには以下が含まれています：

- *プラグイン* - *Windows* と *Linux* 用の動的に読み込まれるライブラリ (`*.dll` および `*.so` ファイル)。
必要なライブラリに加えて、*ROS2* メッセージの種類を生成した結果として作成されたライブラリも含まれています。これらは通信に使用されます。
- *スクリプト* - `R2FU` を *Unity* で使用するためのスクリプト - 詳細は以下の [下記](#scripts) をご覧ください。
- *拡張スクリプト* - `R2FU` を *CoRE.SIM* で使用するためのスクリプト。単一のメイン *Node* の抽象化とインタフェースの簡略化を提供します - 詳細は以下の [下記](#extension-scripts) をご覧ください。これらのスクリプトはライブラリ自体ではなく、直接ディレクトリ `Assets/CoRE.SIM/Scripts/ROS/**` にあります。

### スクリプト
- `ROS2UnityCore` - *ROS2* ノードと実行可能なアクションの処理を行うための主要なクラス。専用スレッドで回転し、アクションを実行します（例：クロック、センサーのパブリッシュトリガーなど）。
- `ROS2UnityComponent` - *Unity* コンポーネントとして動作するように適応された `ROS2UnityCore`。
- `ROS2Node` - *ROS2* ノードを表すクラスで、`ROS2UnityComponent` クラスを介して構築する必要があり、回転もこのクラスで処理されます。
- `ROS2ForUnity` - *ROS2* 通信の確認、適切な初期化、シャットダウンを処理する内部クラス。
- `ROS2ListenerExample` - 基本的な *ROS2->Unity* 通信のテスト用に提供される例類クラス。
- `ROS2TalkerExample` - 基本的な *Unity->ROS2* 通信のテスト用に提供される例類クラス。
- `ROS2PerformanceTest` - *ROS2<->Unity* 通信のパフォーマンステスト用に提供される例類クラス。
- `Sensor` - *ROS2* 対応のセンサーの抽象基本クラス。
- `Transformations` - *Unity* と *ROS2* の座標系間の変換関数のセット。
- `PostInstall` - `R2FU` メタデータファイルのインストールを処理する内部クラス。
- `Time` スクリプト - 異なる時間源を使用できるようにする一連のクラス：
    - `ROS2Clock` - *Unity* または *ROS2* システム時間と *ROS2* メッセージ間のインタフェースを提供する *ROS2* クロッククラス。
    - `ROS2TimeSource` - *ROS2* 時間を取得します（デフォルトではシステム時間）。
    - `UnityTimeSource` - *Unity* 時間を取得します。
    - `DotnetTimeSource` - `Stopwatch` を使用して解像度を高めた *Unity* `DateTime` ベースのクロックを取得します。
    - `ITimeSource` - 任意のソースからの一般的な時間の取得用のインタフェース。
    - `TimeUtils` - 時間変換用のユーティリティ。

拡張スクリプト
さらに、*CoRE.SIM*を`R2FU`の使用に適応させるために、以下のスクリプトが使用されています。

- `SimulatorROS2Node` - これは*CoRE.SIM<->ROS2*の通信に直接責任を持つクラスです。
- `ClockPublisher` - *SimulatorROS2Node*で実行されているクロックからのシミュレーション時間の発行を可能にします。
シーンにコンポーネントとして追加する必要があり、シーンが実行されると現在の時間を発行します。

    ![clock_publisher](clock_publisher.png)

- `QoSSettings` - これは*CoRE.SIM*内のサブスクライバーとパブリッシャーの*QoS*を指定する*ROS2*の相当です。
[Ros2cs](https://github.com/RobotecAI/ros2cs)ライブラリから[`QualityOfServiceProfile`](https://github.com/RobotecAI/ros2cs/blob/develop/src/ros2cs/ros2cs_core/QualityOfServiceProfile.cs)の実装を使用しています。
- `ROS2Utility` - これは、例えば、*ROS2*の座標系から*CoRE.SIM*の座標系に位置を変換するためのユーティリティを持つクラスです。
- `DiagnosticsManager` - `*.yaml`設定ファイルで説明されている要素に対する診断情報を出力します。


## Default message types
The basic *ROS2* msgs types that are supported in *CoRE.SIM* by default include:

- [common_interfaces](https://index.ros.org/r/common_interfaces/github-ros2-common_interfaces/):
    - [`std_msgs`](https://index.ros.org/p/std_msgs/github-ros2-common_interfaces/#humble).
    - [`geometry_msgs`](https://index.ros.org/p/geometry_msgs/github-ros2-common_interfaces/#humble),
    - [`sensor_msgs`](https://index.ros.org/p/sensor_msgs/github-ros2-common_interfaces/#humble),
    - [`nav_msgs`](https://index.ros.org/p/nav_msgs/github-ros2-common_interfaces/#humble),
    - [`diagnostic_msgs`](https://index.ros.org/p/diagnostic_msgs/github-ros2-common_interfaces/#humble),
- [rcl_interfaces](https://index.ros.org/r/rcl_interfaces/github-ros2-rcl_interfaces/):
    - [`builtin_interfaces`](https://index.ros.org/p/builtin_interfaces/github-ros2-rcl_interfaces/#humble),
    - [`action_msgs`](https://index.ros.org/p/action_msgs/github-ros2-rcl_interfaces/#humble),
    - [`rosgraph_msgs`](https://index.ros.org/p/rosgraph_msgs/github-ros2-rcl_interfaces/#humble),
    - [`test_msgs`](https://index.ros.org/p/test_msgs/github-ros2-rcl_interfaces/#humble).
- [autoware_auto_msgs](https://github.com/tier4/autoware_auto_msgs):
    - [`autoware_auto_control_msgs`](https://index.ros.org/p/autoware_auto_control_msgs/gitlab-autowarefoundation-autoware-auto-autoware_auto_msgs/#humble),
    - [`autoware_auto_geometry_msgs`](https://index.ros.org/p/autoware_auto_geometry_msgs/gitlab-autowarefoundation-autoware-auto-autoware_auto_msgs/#humble),
    - [`autoware_auto_planning_msgs`](https://index.ros.org/p/autoware_auto_planning_msgs/gitlab-autowarefoundation-autoware-auto-autoware_auto_msgs/#humble),
    - [`autoware_auto_mapping_msgs`](https://index.ros.org/p/autoware_auto_mapping_msgs/gitlab-autowarefoundation-autoware-auto-autoware_auto_msgs/#humble),
    - [`autoware_auto_vehicle_msgs`](https://index.ros.org/p/autoware_auto_vehicle_msgs/gitlab-autowarefoundation-autoware-auto-autoware_auto_msgs/#humble).
- [tier4_autoware_msgs](https://github.com/tier4/tier4_autoware_msgs):
    - [`tier4_control_msgs`](https://github.com/tier4/tier4_autoware_msgs/tree/tier4/universe/tier4_control_msgs),
    - [`tier4_vehicle_msgs`](https://github.com/tier4/tier4_autoware_msgs/tree/tier4/universe/tier4_vehicle_msgs).
- Others:
    - [`tf2_msgs`](https://index.ros.org/p/tf2_msgs/github-ros2-geometry2/#humble),
    - [`unique_identifier_msgs`](https://index.ros.org/p/unique_identifier_msgs/github-ros2-unique_identifier_msgs/#humble).

In order for the message package to be used in *Unity*, its `*.dll` and `*.so` libraries must be generated using `R2FU`.

!!! tip "Custom message"
    If you want to generate a custom message to allow it to be used in *CoRE.SIM* please read [this tutorial](../AddACustomROS2Message/).

## Use of generated messages in *Unity*
Each message type is composed of other types - which can also be a complex type.
All of them are based on built-in [*C#* types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/built-in-types).
The most common built-in types in messages are `bool`, `int`, `double` and `string`.
These types have their communication equivalents using *ROS2*.

A good example of a complex type that is added to other complex types in order to specify a reference - in the form of a timestamp and a frame - is [std_msgs/Header][header].
This message has the following form:

```csharp
builtin_interfaces/msg/Time stamp
string frame_id
```

!!! warning "ROS2 directive"
    In order to work with *ROS2* in *Unity*, remember to add the directive `using ROS2;` at the top of the file to import types from this namespace.
    
### Create an object
The simplest way to create an object of [`Header`][header] type is:

```csharp
var header = new std_msgs.msg.Header()
{
    Frame_id = "map"
}
```

It is not required to define the value of each field.
As you can see, it creates an object, filling only `frame_id` field - and left the field of complex [`builtin_interfaces/msg/Time`][time] type initialized by default.
Time is an important element of any message, how to fill it is written [here](#filling-a-time).

### Accessing and filling in message fields 
As you might have noticed in the previous example, a *ROS2* message in *Unity* is just a structure containing the same fields - keep the same names and types.
Access to its fields for reading and filling is the same as for any *C#* structure.

```csharp
var header2 = new std_msgs.msg.Header();
header2.Frame_id = "map";
header2.Stamp.sec = "1234567";
Debug.Log($"StampSec: {header2.Stamp.sec} and Frame: {header2.Frame_id}");
```

!!! warning "Field names"
    There is one always-present difference in field names.
    The **first letter** of each message field in *Unity* is **always** **uppercase** - even if the base *ROS2* message from which it is generated is lowercase.

### Filling a time
In order to complete the time field of the [`Header`][header] message, we recommend the following methods in *CoRE.SIM*:

1. When the message has no [`Header`][header] but only the [`Time`][time] type:

    ```csharp
    var header2 = new std_msgs.msg.Header();
    header2.Stamp = SimulatorROS2Node.GetCurrentRosTime();
    ```

2. When the message has a [`Header`][header] - like for example  [autoware_auto_vehicle_msgs/VelocityReport](https://github.com/tier4/autoware_auto_msgs/blob/tier4/main/autoware_auto_vehicle_msgs/msg/VelocityReport.idl):

    ```csharp
    velocityReportMsg = new autoware_auto_vehicle_msgs.msg.VelocityReport()
    {
        Header = new std_msgs.msg.Header()
        {
            Frame_id = "map",
        }
    };
    var velocityReportMsgHeader = velocityReportMsg as MessageWithHeader;
    SimulatorROS2Node.UpdateROSTimestamp(ref velocityReportMsgHeader);
    ```

These methods allow to fill the [`Time`][time] field in the message object with the simulation time - from *ROS2Clock*

### Create a message with array
Some message types contain an [array](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/) of some type.
An example of such a message is [`nav_msgs/Path`](https://docs.ros2.org/foxy/api/nav_msgs/msg/Path.html), which has a [`PoseStamped`](https://docs.ros2.org/foxy/api/geometry_msgs/msg/PoseStamped.html) array.
In order to fill such an array, you must first create a [`List<T>`](https://learn.microsoft.com/pl-pl/dotnet/api/system.collections.generic.list-1?view=net-8.0), fill it and then convert it to a [raw array](https://learn.microsoft.com/pl-pl/dotnet/api/system.collections.generic.list-1.toarray?view=net-8.0).

```csharp
var posesList = new List<geometry_msgs.msg.PoseStamped>();
for(int i=0; i<=5;++i)
{
    var poseStampedMsg = new geometry_msgs.msg.PoseStamped();
    poseStampedMsg.Pose.Position.X = i;
    poseStampedMsg.Pose.Position.Y = 5-i;
    var poseStampedMsgHeader = poseStampedMsg as MessageWithHeader;
    SimulatorROS2Node.UpdateROSTimestamp(ref poseStampedMsgHeader);
    posesList.Add(poseStampedMsg);
}
var pathMsg = new nav_msgs.msg.Path(){Poses=posesList.ToArray()};
var pathMsgHeader = pathMsg as MessageWithHeader;
SimulatorROS2Node.UpdateROSTimestamp(ref pathMsgHeader);
// pathMsg is ready
```

### Publish on the topic
In order to publish messages, a publisher object must be created.
The static method `CreatePublisher` of the `SimulatorROS2Node` makes it easy.
You must specify the *type* of message, the *topic* on which it will be published and the *QoS* profile.<br>
Below is an example of `autoware_auto_vehicle_msgs.msg.VelocityReport` type message publication with a frequency of `30Hz` on `/vehicle/status/velocity_status` topic, the [*QoS*][qos] profile is `(Reliability=Reliable, Durability=Volatile, History=Keep last, Depth=1`):

```csharp
using UnityEngine;
using ROS2;

namespace CoRE.SIM
{
    public class VehicleReportRos2Publisher : MonoBehaviour
    {
        float timer = 0;
        int publishHz = 30;
        QoSSettings qosSettings = new QoSSettings()
        {
            ReliabilityPolicy = ReliabilityPolicy.QOS_POLICY_RELIABILITY_RELIABLE,
            DurabilityPolicy = DurabilityPolicy.QOS_POLICY_DURABILITY_VOLATILE,
            HistoryPolicy = HistoryPolicy.QOS_POLICY_HISTORY_KEEP_LAST,
            Depth = 1,
        };
        string velocityReportTopic = "/vehicle/status/velocity_status";
        autoware_auto_vehicle_msgs.msg.VelocityReport velocityReportMsg;
        IPublisher<autoware_auto_vehicle_msgs.msg.VelocityReport> velocityReportPublisher;

        void Start()
        {
            // Create a message object and fill in the constant fields
            velocityReportMsg = new autoware_auto_vehicle_msgs.msg.VelocityReport()
            {
                Header = new std_msgs.msg.Header()
                {
                    Frame_id = "map",
                }
            };

            // Create publisher with specific topic and QoS profile
            velocityReportPublisher = SimulatorROS2Node.CreatePublisher<autoware_auto_vehicle_msgs.msg.VelocityReport>(velocityReportTopic, qosSettings.GetQoSProfile());
        }

         bool NeedToPublish()
        {
            timer += Time.deltaTime;
            var interval = 1.0f / publishHz;
            interval -= 0.00001f;
            if (timer < interval)
                return false;
            timer = 0;
            return true;
        }

        void FixedUpdate()
        {
            // Provide publications with a given frequency
            if (NeedToPublish())
            {
                // Fill in non-constant fields
                velocityReportMsg.Longitudinal_velocity = 1.00f;
                velocityReportMsg.Lateral_velocity = 0.00f;
                velocityReportMsg.Heading_rate = 0.00f;

                // Update Stamp
                var velocityReportMsgHeader = velocityReportMsg as MessageWithHeader;
                SimulatorROS2Node.UpdateROSTimestamp(ref velocityReportMsgHeader);

                // Publish
                velocityReportPublisher.Publish(velocityReportMsg);
            }
        }
    }
}
```

### Subscribe to the topic
In order to subscribe messages, a subscriber object must be created.
The static method `CreateSubscription` of the `SimulatorROS2Node` makes it easy.
You must specify the *type* of message, the *topic* from which it will be subscribed and the [*QoS*][qos] profile.
In addition, the *callback* must be defined, which will be called when the message is received - in particular, it can be defined as a [lambda expression](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions).<br>
Below is an example of `std_msgs.msg.Bool` type message subscription on `/vehicle/is_vehicle_stopped` topic, the [*QoS*][qos] profile is [`“system default”`][qos]:
```csharp
using UnityEngine;
using ROS2;

namespace CoRE.SIM
{
    public class VehicleStoppedSubscriber : MonoBehaviour
    {
        QoSSettings qosSettings = new QoSSettings();
        string isVehicleStoppedTopic = "/vehicle/is_vehicle_stopped";
        bool isVehicleStopped = false;
        ISubscription<std_msgs.msg.Bool> isVehicleStoppedSubscriber;

        void Start()
        {
            isVehicleStoppedSubscriber = SimulatorROS2Node.CreateSubscription<std_msgs.msg.Bool>(isVehicleStoppedTopic, VehicleStoppedCallback, qosSettings.GetQoSProfile());
        }

        void VehicleStoppedCallback(std_msgs.msg.Bool msg)
        {
            isVehicleStopped = msg.Data;
        }

        void OnDestroy()
        {
            SimulatorROS2Node.RemoveSubscription<std_msgs.msg.Bool>(isVehicleStoppedSubscriber);
        }
    }
}
```
