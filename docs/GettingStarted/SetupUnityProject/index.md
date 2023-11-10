# Setup Unity Project

!!! info

    It is advised to checkout the [Quick Start Demo](../QuickStartDemo) tutorial before reading this section.

This page is a tutorial for setting up a CoRE.SIM Unity project.

## Environment preparation

### System setup

**Ubuntu 22**

1. Make sure your machine meets the [required hardware specifications](../QuickStartDemo/#pc-specs).
    - *NOTE: PC requirements may vary depending on simulation contents which may change as the simulator develops*
2. Prepare a desktop PC with Ubuntu 22.04 installed.
2. Install [NVIDIA drivers and Vulkan Graphics API](../QuickStartDemo/#running-the-awsim-simulation-demo).
3. Install [git](https://git-scm.com/).


### Unity Hub Installation
Follow the instructions on [this page](https://docs.unity3d.com/hub/manual/InstallHub.html#install-hub-linux).

1. To add the public signing key, execute the following command in the terminal:
    ```bash
    wget -qO - https://hub.unity3d.com/linux/keys/public | gpg --dearmor | sudo tee /usr/share/keyrings/Unity_Technologies_ApS.gpg > /dev/null
    ```

2. To include the Unity Hub repository, you must create an entry in `/etc/apt/sources.list.d`. Use this command to add the Unity Hub repository:
    ```bash
    sudo sh -c 'echo "deb [signed-by=/usr/share/keyrings/Unity_Technologies_ApS.gpg] https://hub.unity3d.com/linux/repos/deb stable main" > /etc/apt/sources.list.d/unityhub.list'
    ```

3. Refresh the package cache and install Unity Hub with these commands:
    ```bash
    sudo apt update
    sudo apt install unityhub
    ```

4. Launch Unity Hub and acquire a license. For most users, the Personal license will be sufficient.

### Open CoRE.SIM project

To open the Unity CoRE.SIM project in Unity Editor:
1. Make sure you have the CoRE.SIM repository cloned
    ```
    git clone https://github.com/StrayedCats/CoRE.SIM.git
    ```

2. Launch UnityHub.

3. Open the project in UnityHub
    - Click the `Add` button

    ![](pic-0.png)

    - Navigate the directory where the CoRE.SIM repository was cloned to

    - install Unity edtitor

    ![](pic-1.png)

    ![](pic-2.png)

    - The project is now ready to use

    ![](pic-3.png)

!!! warning

    If you get the safe mode dialog when starting UnityEditor, you may need to install openssl.

    1. download libssl  
    `wget http://archive.ubuntu.com/ubuntu/pool/main/o/openssl/libssl1.1_1.1.0g-2ubuntu4_amd64.deb`
    2. install  
    `sudo dpkg -i libssl1.1_1.1.0g-2ubuntu4_amd64.deb`

### Run CoRE.SIM

1. Open the `OutdoorScene.unity` scene placed under `Assets/CoRE/` directory
2. Run the simulation by clicking `Play` button placed at the top section of Editor.
![](pic-5.png)
<br><br><br><br>
