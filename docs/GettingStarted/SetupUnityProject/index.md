# Unityプロジェクトのセットアップ

このページは、CoRE.SIM Unityプロジェクトのセットアップに関するチュートリアルです。

## 環境の準備

### システムのセットアップ
[GettingStarted/QuickStartDemo](GettingStarted/QuickStartDemo/index.md)に従って環境構築してください。

### Unity Hubのインストール
=== "Ubuntu"
    [このページ](https://docs.unity3d.com/hub/manual/InstallHub.html#install-hub-linux)の指示に従ってください。

    1. 公開署名キーを追加するには、ターミナルで次のコマンドを実行します：
        ```bash
        wget -qO - https://hub.unity3d.com/linux/keys/public | gpg --dearmor | sudo tee /usr/share/keyrings/Unity_Technologies_ApS.gpg > /dev/null
        ```

    2. Unity Hubリポジトリを含めるには、`/etc/apt/sources.list.d`にエントリーを作成する必要があります。以下のコマンドを使用してUnity Hubリポジトリを追加します：
        ```bash
        sudo sh -c 'echo "deb [signed-by=/usr/share/keyrings/Unity_Technologies_ApS.gpg] https://hub.unity3d.com/linux/repos/deb stable main" > /etc/apt/sources.list.d/unityhub.list'
        ```

    3. パッケージキャッシュを更新し、次のコマンドでUnity Hubをインストールします：
        ```bash
        sudo apt update
        sudo apt install unityhub
        ```

    4. Unity Hubを起動し、ライセンスを取得します。ほとんどのユーザーにとって、個人用ライセンスが十分でしょう。

=== "Windows"
    [unity.com/download](https://unity.com/ja/download){ .md-button }

### CoRE.SIMプロジェクトを開く

UnityエディターでUnity CoRE.SIMプロジェクトを開くには：

1. CoRE.SIMリポジトリをクローンしていることを確認してください
    ```
    git clone https://github.com/StrayedCats/CoRE.SIM.git
    ```

2. Unity Hubを起動します。

3. Unity Hubでプロジェクトを開きます
    - `Add` ボタンをクリック

    ![](pic-0.png)

    - CoRE.SIMリポジトリがクローンされたディレクトリに移動

    - Unityエディターをインストール

    ![](pic-1.png)

    ![](pic-2.png)

    - プロジェクトはこれで使用準備ができています

    ![](pic-3.png)

!!! warning

    UnityEditorを起動する際にセーフモードのダイアログが表示される場合、opensslをインストールする必要があるかもしれません。

    1. libsslをダウンロード  
    `wget http://archive.ubuntu.com/ubuntu/pool/main/o/openssl/libssl1.1_1.1.0g-2ubuntu4_amd64.deb`
    2. インストール  
    `sudo dpkg -i libssl1.1_1.1.0g-2ubuntu4_amd64.deb`

### CoRE.SIMを実行する

1. `Assets/CoRE/` ディレクトリの下に配置された `OutdoorScene.unity` シーンを開きます。
2. エディターの上部にある `Play` ボタンをクリックしてシミュレーションを実行します。
![](pic-5.png)
<br><br><br><br>