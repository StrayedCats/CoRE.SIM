using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;

public class GUIText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI red_kill;
    [SerializeField]
    private TextMeshProUGUI red_HP1;
    [SerializeField]
    private TextMeshProUGUI red_HP2;
    [SerializeField]
    private TextMeshProUGUI red_HP3;
    [SerializeField]
    private TextMeshProUGUI red_buff;

    [SerializeField]
    private TextMeshProUGUI blue_kill;
    [SerializeField]
    private TextMeshProUGUI blue_HP1;
    [SerializeField]
    private TextMeshProUGUI blue_HP2;
    [SerializeField]
    private TextMeshProUGUI blue_HP3;
    [SerializeField]
    private TextMeshProUGUI blue_buff;

    public RedTeamData red;
    public RedAttackerBotControl red_attacker1;
    public RedAttackerBotControl red_attacker2;
    public RedBufferBotControl red_buffer;
    public BlueTeamData blue;
    public BlueAttackerBotControl blue_attacker1;
    public BlueAttackerBotControl blue_attacker2;
    public BlueBufferBotControl blue_buffer;

    void Update()
    {
        red_kill.SetText("{}",red.killcount);
        red_HP1.SetText("{}", red_attacker1.HP);
        red_HP2.SetText("{}", red_attacker2.HP);
        red_HP3.SetText("{}", red_buffer.HP);
        red_buff.SetText("Å~{}", red.buff);
        blue_kill.SetText("{}",blue.killcount);
        blue_HP1.SetText("{}", blue_attacker1.HP);
        blue_HP2.SetText ("{}", blue_attacker2.HP);
        blue_HP3.SetText ("{}", blue_buffer.HP);
        blue_buff.SetText("Å~{}", blue.buff);
    }
}
