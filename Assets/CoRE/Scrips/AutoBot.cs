using System.Collections;
using System.Collections.Generic;
using AWSIM.Lanelet;
using TMPro;
using UnityEngine;

namespace ROS2
{
    public class AutoBot : MonoBehaviour
    {
        public string robot_namespace;
        public GameObject gun_gameobj;
        public GameObject gun_target_gameobj;

        public GameObject red_ball;
        public GameObject green_ball;
        public GameObject realsense;
        public GameObject rs_color;
        public TextMeshProUGUI reamining_bullets;
        public TextMeshProUGUI default_bullets;

        private ROS2UnityComponent ros2Unity;
        private ROS2Node ros2Node;

        private IPublisher<tf2_msgs.msg.TFMessage> tf_pub;
        private geometry_msgs.msg.Twist sub_msg;
        private std_msgs.msg.Float32 sub_gun_msg;
        private double target_gun;

        private int bullet_count;
        public double motor_noise_percent;

        public static ITimeSource TimeSource { get; private set; } = new UnityTimeSource();
        static ROS2Clock ros2Clock;

        void Start()
        {
            ros2Unity = GetComponent<ROS2UnityComponent>();
            sub_msg = new geometry_msgs.msg.Twist();
            sub_gun_msg = new std_msgs.msg.Float32();
            target_gun = 75f;
            bullet_count = 40;
            reamining_bullets.SetText(bullet_count.ToString());
            default_bullets.SetText("/" + bullet_count.ToString());
        }

        geometry_msgs.msg.Transform globalObjTramsformToROS2(GameObject obj)
        {
            var transform_rt = new geometry_msgs.msg.Transform();
            transform_rt.Translation.X = obj.transform.position.x;
            transform_rt.Translation.Y = obj.transform.position.z;
            transform_rt.Translation.Z = obj.transform.position.y;
            transform_rt.Rotation.X = obj.transform.rotation.x;
            transform_rt.Rotation.Y = obj.transform.rotation.z;
            transform_rt.Rotation.Z = obj.transform.rotation.w;
            transform_rt.Rotation.W = obj.transform.rotation.y;
            return transform_rt;
        }
        geometry_msgs.msg.Transform localObjTramsformToROS2(GameObject obj)
        {
            var transform_rt = new geometry_msgs.msg.Transform();
            transform_rt.Translation.X = obj.transform.localPosition.x;
            transform_rt.Translation.Y = obj.transform.localPosition.z;
            transform_rt.Translation.Z = obj.transform.localPosition.y;
            transform_rt.Rotation.X = obj.transform.localRotation.x;
            transform_rt.Rotation.Y = obj.transform.localRotation.z;
            transform_rt.Rotation.Z = obj.transform.localRotation.w;
            transform_rt.Rotation.W = obj.transform.localRotation.y;
            return transform_rt;
        }
        static public builtin_interfaces.msg.Time GetCurrentRosTime()
        {
            var timeMsg = new builtin_interfaces.msg.Time();
            ros2Clock.UpdateROSClockTime(timeMsg);
            return timeMsg;
        }

        void FixedUpdate()
        {
            if (ros2Unity.Ok())
            {
                // topic & node init
                if (ros2Node == null)
                {
                    ros2Node = ros2Unity.CreateNode("CoREdotSim_" + robot_namespace);
                    ros2Clock = new ROS2Clock(TimeSource);
                    ros2Node.CreateSubscription<geometry_msgs.msg.Twist>(
                      robot_namespace + "/control/cmd_vel", msg =>{
                          sub_msg = msg;
                      });

                    ros2Node.CreateSubscription<std_msgs.msg.Float32>(
                        robot_namespace + "/control/gunshot", msg => {
                            sub_gun_msg = msg;
                        });
                    tf_pub = ros2Node.CreatePublisher<tf2_msgs.msg.TFMessage>("/tf");
                }

                // Auto Robot Yaw Control
                double target_z = 0f;
                if (sub_msg.Angular.Z >= 0.03145 || sub_msg.Angular.Z <= -0.03145)
                {
                    if (sub_msg.Angular.Z > 0.5) { sub_msg.Angular.Z = 0.5f; }
                    if (sub_msg.Angular.Z < -0.5) { sub_msg.Angular.Z = -0.5f; }
                    double dz = sub_msg.Angular.Z * 57.295779 * Time.deltaTime;
                    target_z = dz + (Random.value - 0.5) * dz * motor_noise_percent;
                    this.transform.Rotate(0f, (float)(target_z), 0f);
                }
                else
                {
                    this.transform.Rotate(0f, 0f, 0f);
                }

                // Auto Robot Gun Roll Control
                if (sub_msg.Angular.X >= 0.01 || sub_msg.Angular.X <= -0.01)
                {
                    if (sub_msg.Angular.X <= 0.2 && sub_msg.Angular.X >= -0.2)
                    {
                        double dx = sub_msg.Angular.X * 57.295779 * Time.deltaTime;
                        target_gun += dx + (Random.value - 0.5) * dx * motor_noise_percent;
                    }
                }
                if (target_gun > 90) { target_gun = 90; }
                if (target_gun < 40) { target_gun = 40; }

                gun_gameobj.transform.localRotation = Quaternion.Euler((float)(target_gun), 0, 0);


                // Auto Robot Shot Control
                if (Mathf.Abs((float)sub_gun_msg.Data) >= 0.1 && Mathf.Abs((float)sub_gun_msg.Data) <= 10 && bullet_count > 0)
                {
                    float forceRandomness = Random.Range(0.85f, 1.15f);
                    Vector3 torqueRandomness = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

                    GameObject obj;
                    if (Random.value > 0.5)
                    {
                        obj = Instantiate(red_ball, gun_target_gameobj.transform.position, Quaternion.Euler(gun_target_gameobj.transform.parent.eulerAngles.x, gun_target_gameobj.transform.parent.eulerAngles.y, 0));
                    }
                    else
                    {
                        obj = Instantiate(green_ball, gun_target_gameobj.transform.position, Quaternion.Euler(gun_target_gameobj.transform.parent.eulerAngles.x, gun_target_gameobj.transform.parent.eulerAngles.y, 0));
                    }

                    Rigidbody rb = obj.GetComponent<Rigidbody>();
                    rb.AddForce(gun_target_gameobj.transform.forward * (float)sub_gun_msg.Data * 57.295779f * forceRandomness);
                    rb.AddTorque(torqueRandomness * 10f);

                    sub_gun_msg.Data = 0;

                    bullet_count -= 1;
                    reamining_bullets.SetText(bullet_count.ToString());
                }


                // Robot State Pub
                var pose_robot = new geometry_msgs.msg.TransformStamped();
                pose_robot.Header.Frame_id = "map";
                pose_robot.Child_frame_id = robot_namespace + "_base_link";
                pose_robot.Transform = globalObjTramsformToROS2(this.gameObject);
                pose_robot.Header.Stamp = GetCurrentRosTime();

                var gun_robot = new geometry_msgs.msg.TransformStamped();
                gun_robot.Header.Frame_id = robot_namespace + "_base_link";
                gun_robot.Child_frame_id = robot_namespace + "_gun";
                gun_robot.Transform = localObjTramsformToROS2(gun_gameobj.gameObject);
                gun_robot.Header.Stamp = GetCurrentRosTime();

                var pose_rs = new geometry_msgs.msg.TransformStamped();
                pose_rs.Header.Frame_id = robot_namespace + "_base_link";
                pose_rs.Child_frame_id = robot_namespace + "_camera_link";
                pose_rs.Transform = localObjTramsformToROS2(realsense.gameObject);
                pose_rs.Header.Stamp = GetCurrentRosTime();

                var pose_rs_c = new geometry_msgs.msg.TransformStamped();
                pose_rs_c.Header.Frame_id = robot_namespace + "_camera_link";
                pose_rs_c.Child_frame_id = robot_namespace + "_camera_color_frame";
                pose_rs_c.Transform = localObjTramsformToROS2(rs_color.gameObject);
                pose_rs_c.Header.Stamp = GetCurrentRosTime();

                var tf = new tf2_msgs.msg.TFMessage
                {
                    Transforms = new geometry_msgs.msg.TransformStamped[4] { pose_robot, gun_robot, pose_rs, pose_rs_c, }
                };
                tf_pub.Publish(tf);
            }
        }

        public void ResetAutoBot()
        {
            this.transform.position = new Vector3(4.294943f, 0f, -14.16366f);
            this.transform.rotation = new Quaternion(0, 0.522135735f, 0, -0.852862358f);
            gun_gameobj.transform.localPosition = new Vector3(0, 0.246000007f, 0);
            gun_gameobj.transform.localRotation = new Quaternion(0.60876137f, 0, 0, 0.793353379f);
            sub_msg = new geometry_msgs.msg.Twist();
            sub_gun_msg = new std_msgs.msg.Float32();
            bullet_count = 40;
            reamining_bullets.SetText(bullet_count.ToString());
        }
    }
} // namespace ROS2