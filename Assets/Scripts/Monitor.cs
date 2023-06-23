using UnityEngine;
using Valve.VR;

public class Monitor : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // �ֱ����ͣ��������ֻ�����
    public SteamVR_Action_Vector2 thumbstickAction; // Ĵָ������Ĳ���
    public SteamVR_Action_Boolean triggerAction; // ��������ť����Ĳ���

    void Update()
    {
        // ��ȡĴָ�˵�����ֵ
        Vector2 thumbstickValue = thumbstickAction.GetAxis(handType);
        Debug.Log("Thumbstick Value: " + thumbstickValue);

        // ��ȡ��������ť��״̬
        bool triggerPressed = triggerAction.GetState(handType);
        if (triggerPressed)
        {
            Debug.Log("Trigger Pressed");
        }
    }
}