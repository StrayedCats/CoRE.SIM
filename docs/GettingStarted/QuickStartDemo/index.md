# クイックスタートデモ

以下に、CoRE.SIMシミュレーションのセットアップ手順があります。

### PCの仕様

シミュレーションを正しく実行するために、コンピュータが以下の要件を満たしていることを確認してください：

|必要なPC仕様||
|:--|:--|
|OS|Ubuntu 22/20, Windows11/10|
|CPU|4コアと8スレッド以上|
|GPU|GTX 1070以上|
|NVIDIAドライバー（Ubuntu 22）|>=515.43.04|

=== "Ubuntu"
    1. ローカルホストの設定

        シミュレーションは、CoRE.SIMシミュレーションとROS 2ソフトウェアのトラブルフリーな通信を可能にする適切なネットワーク設定に基づいています。
        必要なローカルホスト設定を適用するには、次の行を`~/.bashrc`および`~/.profile`ファイルに追加してください。

        ```bash
        source /opt/ros/humble/setup.bash
        export RMW_IMPLEMENTATION=rmw_cyclonedds_cpp
        ```


    2. NVIDIA GPUドライバーをインストールします（すでにインストールされている場合はスキップ）。
        1. 推奨されるバージョンのドライバーをインストールします。
            ```
            sudo apt update
            sudo ubuntu-drivers autoinstall
            ```
        2. インストールされたドライバーがシステムに検出されるように、マシンを再起動します。
            ```
            sudo reboot
            ```
        3. ターミナルを開いて、`nvidia-smi`コマンドが利用可能で、以下に示すような要約を出力するか確認します。
            ```
            nvidia-smi 
            ```
            ```
            Fri Oct 14 01:41:05 2022       
            +-----------------------------------------------------------------------------+
            | NVIDIA-SMI 515.65.01    Driver Version: 515.65.01    CUDA Version: 11.7     |
            |-------------------------------+----------------------+----------------------+
            | GPU  Name        Persistence-M| Bus-Id        Disp.A | Volatile Uncorr. ECC |
            | Fan  Temp  Perf  Pwr:Usage/Cap|         Memory-Usage | GPU-Util  Compute M. |
            |                               |                      |               MIG M. |
            |===============================+======================+======================|
            |   0  NVIDIA GeForce ...  Off  | 00000000:01:00.0  On |                  N/A |
            | 37%   31C    P8    30W / 250W |    188MiB / 11264MiB |      3%      Default |
            |                               |                      |                  N/A |
            +-------------------------------+----------------------+----------------------+

            +-----------------------------------------------------------------------------+
            | Processes:                                                                  |
            |  GPU   GI   CI        PID   Type   Process name                  GPU Memory |
            |        ID   ID                                                   Usage      |
            |=============================================================================|
            |    0   N/A  N/A      1151      G   /usr/lib/xorg/Xorg                133MiB |
            |    0   N/A  N/A      1470      G   /usr/bin/gnome-shell               45MiB |
            +-----------------------------------------------------------------------------+
            ```

    3. Vulkan Graphics Libraryをインストールします（すでにインストールされている場合はスキップ）。
        1. 環境を更新します。
            ```
            sudo apt update
            ```
        2. ライブラリをインストールします。
            ```
            sudo apt install libvulkan1
            ```

    4. CoRE.SIMデモバイナリをダウンロードして実行します。

        1. `CoRE.SIM-ubuntu.zip`をダウンロードします。

            [Ubuntu用のCoRE.SIMデモをダウンロード](https://github.com/StrayedCats/CoRE.SIM/releases){.md-button .md-button--primary}

        2. ダウンロードしたファイルを解凍します。

        3. `CoRE.SIM.x86_64`ファイルに実行権限を付与します。

            `CoRE.SIM.x86_64`ファイルを右クリックし、`実行`のチェックボックスをオンにします

            または、以下のコマンドを実行します。

            ```
            chmod +x <CoRE.SIMフォルダへのパス>/CoRE.SIM.x86_64
            ```

        4. `CoRE.SIM.x86_64`を起動します。
            ```
            ./<CoRE.SIMフォルダへのパス>/CoRE.SIM.x86_64
            ``` 
=== "Windows"
    1. NVIDIA GPUドライバのインストール
        [NVIDIA 公式サイト](https://www.nvidia.co.jp/Download/index.aspx?lang=jp)よりダウンロードしてインストールしてください。
    2. CoRE.SIMデモバイナリをダウンロードして実行します。

        1. `CoRE.SIM-windows.zip`をダウンロードします。

            [Windows用のCoRE.SIMデモをダウンロード](https://github.com/StrayedCats/CoRE.SIM/releases){.md-button .md-button--primary}

        2. ダウンロードしたファイルを解凍します。

        3. `CoRE.SIM.exe`をタブルクリックします。

        4. 警告を回避します。

            `WindowsによってPCが保護されました`というウィンドウが起動します。`詳細情報`をクリックして`実行`をクリックします。
            ![pic-1](./error-1.png)
            ![pic-2](./error-2.png)

        5. 必要な許可を与えます。

            ローカルネットワークへのアクセス許可を与えるために`許可`クリックします。

            ![net_pic](./prvt_net.png)