using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/* ��ͨ���̣��̳���ClickKeyboard��Ӧ��ֻ��ʵ���Լ���Axis2Letter����. */
public class NormalKeyboard : ClickKeyboard
{
    public override int Axis2Letter(Vector2 axis, SteamVR_Input_Sources hand, int mode, ref GameObject key)
    {
        // TODO!! ��ͨ���̵�ӳ��.
        return 0;
    }
}
