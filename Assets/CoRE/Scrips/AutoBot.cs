using System.Collections;
using System.Collections.Generic;
using AWSIM.Lanelet;
using UnityEngine;

namespace ROS2
{
public class AutoBot : MonoBehaviour
{
    public GameObject gun_gameobj;
    public GameObject gun_target_gameobj;

    public GameObject red_ball;
    public GameObject green_ball;
    public GameObject enemy_robot_1;

    public GameObject base_target_1;
    public GameObject base_target_2;
    public GameObject base_target_3;

    public GameObject realsense;
    public GameObject rs_color;


    private ROS2UnityComponent ros2Unity;
    private ROS2Node ros2Node;

    private ISubscription<geometry_msgs.msg.Twist> control_sub;
    private IPublisher<tf2_msgs.msg.TFMessage> tf_pub;
    private geometry_msgs.msg.Twist sub_msg;
    private double target_gun;

    public double motor_noise_percent;


    void Start()
    {
        ros2Unity = GetComponent<ROS2UnityComponent>();
        sub_msg = new geometry_msgs.msg.Twist();
        target_gun = 75f;
    }
  
    void Update()
    {

    }

    geometry_msgs.msg.Transform globalObjTramsformToROS2(GameObject obj){
      var transform_rt = new geometry_msgs.msg.Transform();
      transform_rt.Translation.X = (float)(obj.transform.position.x);
      transform_rt.Translation.Y = (float)(obj.transform.position.z);
      transform_rt.Translation.Z = (float)(obj.transform.position.y);
      transform_rt.Rotation.X = (float)(obj.transform.rotation.x);
      transform_rt.Rotation.Y = (float)(obj.transform.rotation.z);
      transform_rt.Rotation.Z = (float)(obj.transform.rotation.w);
      transform_rt.Rotation.W = (float)(obj.transform.rotation.y);
      return transform_rt;
    }
    geometry_msgs.msg.Transform localObjTramsformToROS2(GameObject obj){
      var transform_rt = new geometry_msgs.msg.Transform();
      transform_rt.Translation.X = (float)(obj.transform.localPosition.x);
      transform_rt.Translation.Y = (float)(obj.transform.localPosition.z);
      transform_rt.Translation.Z = (float)(obj.transform.localPosition.y);
      transform_rt.Rotation.X = (float)(obj.transform.localRotation.x);
      transform_rt.Rotation.Y = (float)(obj.transform.localRotation.z);
      transform_rt.Rotation.Z = (float)(obj.transform.localRotation.w);
      transform_rt.Rotation.W = (float)(obj.transform.localRotation.y);
      return transform_rt;
    }

    void FixedUpdate()
    {
      if(ros2Unity.Ok())
      {
        // topic & node init
        if (ros2Node == null)
        {
          ros2Node = ros2Unity.CreateNode("CoREdotSim");
          control_sub = ros2Node.CreateSubscription<geometry_msgs.msg.Twist>(
            "control/cmd_vel", msg => {
              sub_msg = msg;
            });
          tf_pub = ros2Node.CreatePublisher<tf2_msgs.msg.TFMessage>("/tf");
        }

        // Auto Robot Yaw Control
        double target_z = 0f;
        if(sub_msg.Angular.Z >= 0.03145 || sub_msg.Angular.Z <= -0.03145){
          if(sub_msg.Angular.Z > 0.5){sub_msg.Angular.Z = 0.5f;}
          if(sub_msg.Angular.Z < -0.5){sub_msg.Angular.Z = -0.5f;}
          double dz = sub_msg.Angular.Z * 57.295779 * Time.deltaTime;
          target_z = dz + (Random.value - 0.5) * dz * motor_noise_percent;
          this.transform.Rotate(0f, (float)(target_z), 0f);
        }else{
          this.transform.Rotate(0f, 0f, 0f);
        }

        // Auto Robot Gun Roll Control
        if(sub_msg.Angular.X >= 0.01 || sub_msg.Angular.X <= -0.01){
          if(sub_msg.Angular.X <= 0.2 && sub_msg.Angular.X >= -0.2){
            double dx = sub_msg.Angular.X * 57.295779 * Time.deltaTime;
            target_gun += dx + (Random.value - 0.5) * dx * motor_noise_percent;
          }
        }
        if(target_gun > 90){target_gun = 90;}
        if(target_gun < 40){target_gun = 40;}

        gun_gameobj.transform.localRotation = Quaternion.Euler((float)(target_gun), 0, 0);
        

        // Auto Robot Shot Control
        if(Mathf.Abs((float)sub_msg.Linear.X) >= 0.1 && Mathf.Abs((float)sub_msg.Linear.X) <= 10){
            float forceRandomness = Random.Range(0.9f, 1.1f);
            Vector3 torqueRandomness = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            GameObject obj;
            if(Random.value > 0.5){
                obj = Instantiate(red_ball, gun_target_gameobj.transform.position, Quaternion.Euler(gun_target_gameobj.transform.parent.eulerAngles.x, gun_target_gameobj.transform.parent.eulerAngles.y, 0));
            }else{
                obj = Instantiate(green_ball, gun_target_gameobj.transform.position, Quaternion.Euler(gun_target_gameobj.transform.parent.eulerAngles.x, gun_target_gameobj.transform.parent.eulerAngles.y, 0));
            }

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.AddForce(gun_target_gameobj.transform.forward * (float)sub_msg.Linear.X * 57.295779f * forceRandomness);
            rb.AddTorque(torqueRandomness * 10f);
            
            sub_msg.Linear.X = 0;
        }


        // Robot State Pub
        var pose_robot = new geometry_msgs.msg.TransformStamped();
        pose_robot.Header.Frame_id = "map";
        pose_robot.Child_frame_id = "base_link";
        pose_robot.Transform = globalObjTramsformToROS2(this.gameObject);

        var gun_robot = new geometry_msgs.msg.TransformStamped();
        gun_robot.Header.Frame_id = "base_link";
        gun_robot.Child_frame_id = "gun";
        gun_robot.Transform = localObjTramsformToROS2(gun_gameobj.gameObject);

        var pose_rs = new geometry_msgs.msg.TransformStamped();
        pose_rs.Header.Frame_id = "base_link";
        pose_rs.Child_frame_id = "camera_link";
        pose_rs.Transform = localObjTramsformToROS2(realsense.gameObject);

        var pose_rs_c = new geometry_msgs.msg.TransformStamped();
        pose_rs_c.Header.Frame_id = "camera_link";
        pose_rs_c.Child_frame_id = "camera_color_frame";
        pose_rs_c.Transform = localObjTramsformToROS2(rs_color.gameObject);


        // demo
        var pose_enemy_robot_1 = new geometry_msgs.msg.TransformStamped();
        pose_enemy_robot_1.Header.Frame_id = "map";
        pose_enemy_robot_1.Child_frame_id = "enemy_robot_1_link";
        pose_enemy_robot_1.Transform = globalObjTramsformToROS2(enemy_robot_1);

        var pose_base_target_1 = new geometry_msgs.msg.TransformStamped();
        pose_base_target_1.Header.Frame_id = "map";
        pose_base_target_1.Child_frame_id = "base_target_1_link";
        pose_base_target_1.Transform = globalObjTramsformToROS2(base_target_1);

        var pose_base_target_2 = new geometry_msgs.msg.TransformStamped();
        pose_base_target_2.Header.Frame_id = "map";
        pose_base_target_2.Child_frame_id = "base_target_2_link";
        pose_base_target_2.Transform = globalObjTramsformToROS2(base_target_2);

        var pose_base_target_3 = new geometry_msgs.msg.TransformStamped();
        pose_base_target_3.Header.Frame_id = "map";
        pose_base_target_3.Child_frame_id = "base_target_3_link";
        pose_base_target_3.Transform = globalObjTramsformToROS2(base_target_3);

        // demo end

        var tf = new tf2_msgs.msg.TFMessage();
        tf.Transforms = new geometry_msgs.msg.TransformStamped[8]{
          pose_robot,gun_robot,pose_rs,pose_rs_c,
          pose_enemy_robot_1,
          pose_base_target_1,pose_base_target_2,pose_base_target_3
          };
        tf_pub.Publish(tf);
      }
    }  
}
} // namespace ROS2