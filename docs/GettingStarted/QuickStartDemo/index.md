# Quick Start Demo

Below you can find instructions on how to setup the self-driving demo of CoRE.SIM simulation controlled by Autoware.
The instruction assumes using the Ubuntu OS.

### PC specs

Please make sure that your machine meets the following requirements in order to run the simulation correctly:

|Required PC Specs||
|:--|:--|
|OS|Ubuntu 22.04|
|CPU|4cores and 8thread or higher|
|GPU|GTX 1070 or higher|
|NVIDIA Driver (Ubuntu 22)|>=515.43.04|


### Localhost settings

The simulation is based on the appropriate network setting, which allows for trouble-free communication of the CoRE.SIM simulation with the Autoware software.
To apply required localhost settings please add the following lines to `~/.bashrc` & `~/.profile` files.

``` bash
source /opt/ros/humble/setup.bash
export RMW_IMPLEMENTATION=rmw_cyclonedds_cpp
```

## Start the demo

### Running the CoRE.SIM simulation demo

To run the simulator, please follow the steps below.

1. Install Nvidia GPU driver (Skip if already installed).
    1. Install the recommended version of the driver.
    ```
    sudo apt update
    sudo ubuntu-drivers autoinstall
    ```
    2. Reboot your machine to make the installed driver detected by the system.
    ```
    sudo reboot
    ```
    3. Open terminal and check if `nvidia-smi` command is available and outputs summary similar to the one presented below.
    ```
    $ nvidia-smi 
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

2. Install Vulkan Graphics Library (Skip if already installed).
    1. Update the environment.
    ```
    sudo apt update
    ```
    2. Install the library.
    ```
    sudo apt install libvulkan1
    ```

3. Download and Run CoRE.SIM Demo binary.

    1. Download `CoRE.SIM.zip`.

        [Download CoRE.SIM Demo for ubuntu](https://github.com/StrayedCats/CoRE.SIM/releases){.md-button .md-button--primary}
    
    2. Unzip the downloaded file.

    3. Make the `CoRE.Sim.x86_64` file executable.

        Rightclick the `CoRE.SIM.x86_64` file and check the `Execute` checkbox

        ![](Image_1.png)

        or execute the command below.

        ```
        chmod +x <path to CoRE.SIM folder>/CoRE.SIM.x86_64
        ```

    4. Launch `CoRE.SIM.x86_64`.
        ```
        ./<path to CoRE.SIM folder>/CoRE.SIM.x86_64
        ``` 
        
        !!! warning
        
            It may take some time for the application to start the so please wait until image similar to the one presented below is visible in your application window.