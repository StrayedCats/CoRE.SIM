# ROS2 topic list

## List of subscribers
| Category | Topic | Message type |
| :--: | :-- | :--: |
| 青チーム自動機制御 | `/blue/control/cmd_vel`         | `geometry_msg/Twist` |
| 青チーム自動機制御 | `/blue/control/gunshot`         | `std_msgs/Float32` |


## List of publishers

|                        Category                         | Topic                                       |                   Message type                    |               `frame_id`                | `Hz`  |                     `QoS`                      |
| :-----------------------------------------------------: | :------------------------------------------ | :-----------------------------------------------: | :-------------------------------------: | :---: | :--------------------------------------------: |
|                         カメラ                          | `/blue/camera/camera_info` |             `sensor_msgs/CameraInfo`              | `blue_camera_link` | `20+`  | `Best effort`,<br>`Volatile`,<br>`Keep last/1` |
|                         カメラ                          | `/blue/camera/image_raw`   |                `sensor_msgs/Image`                | `blue_camera_link` | `20+`  | `Best effort`,<br>`Volatile`,<br>`Keep last/1` |