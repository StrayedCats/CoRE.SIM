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
    private ROS2UnityComponent ros2Unity;
    private ROS2Node ros2Node;

    private ISubscription<geometry_msgs.msg.Twist> control_sub;
    private IPublisher<geometry_msgs.msg.Pose> robo_pose_pub;
    private IPublisher<geometry_msgs.msg.Pose> gun_pose_pub;
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
          robo_pose_pub = ros2Node.CreatePublisher<geometry_msgs.msg.Pose>("pose/robot");
          gun_pose_pub = ros2Node.CreatePublisher<geometry_msgs.msg.Pose>("pose/gun");
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
        if(sub_msg.Linear.X >= 0.1 || sub_msg.Linear.X <= -0.1){
          if(sub_msg.Linear.X <= 10 && sub_msg.Linear.X >= -10){
            double dx = sub_msg.Linear.X * 57.295779;
            GameObject obj;
            if(Random.value > 0.5){
              obj = Instantiate (red_ball, gun_target_gameobj.transform.position, Quaternion.Euler(gun_target_gameobj.transform.parent.eulerAngles.x,gun_target_gameobj.transform.parent.eulerAngles.y,0));
            }else{
              obj = Instantiate (green_ball, gun_target_gameobj.transform.position, Quaternion.Euler(gun_target_gameobj.transform.parent.eulerAngles.x,gun_target_gameobj.transform.parent.eulerAngles.y,0));
            }
            
            obj.GetComponent<Rigidbody>().AddForce(gun_target_gameobj.transform.forward * (float)dx);
            sub_msg.Linear.X = 0;
          }
        }

        // Robot State Pub
        geometry_msgs.msg.Pose msg_pose = new geometry_msgs.msg.Pose();
        msg_pose.Position.X = (float)(this.transform.position.x);
        msg_pose.Position.Y = (float)(this.transform.position.y);
        msg_pose.Position.Z = (float)(this.transform.position.z);
        msg_pose.Orientation.X = (float)(this.transform.rotation.x);
        msg_pose.Orientation.Y = (float)(this.transform.rotation.y);
        msg_pose.Orientation.W = (float)(this.transform.rotation.w);
        msg_pose.Orientation.Z = (float)(this.transform.rotation.z);
        robo_pose_pub.Publish(msg_pose);

        msg_pose.Position.X = (float)(gun_gameobj.transform.localPosition.x);
        msg_pose.Position.Y = (float)(gun_gameobj.transform.localPosition.y);
        msg_pose.Position.Z = (float)(gun_gameobj.transform.localPosition.z);
        msg_pose.Orientation.X = (float)(gun_gameobj.transform.localRotation.x);
        msg_pose.Orientation.Y = (float)(gun_gameobj.transform.localRotation.y);
        msg_pose.Orientation.W = (float)(gun_gameobj.transform.localRotation.w);
        msg_pose.Orientation.Z = (float)(gun_gameobj.transform.localRotation.z);
        gun_pose_pub.Publish(msg_pose);
      }
    }  
}
} // namespace ROS2