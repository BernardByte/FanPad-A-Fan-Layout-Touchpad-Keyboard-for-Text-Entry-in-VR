
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public enum Keybd_Flags
{
    KeyDown,
    KeyHold,
    KeyUp,
}

public class KeyboardBase : MonoBehaviour
{
    // Start is called before the first frame update
    [DllImport("User32.dll", EntryPoint = "keybd_event")]
    static extern void keybd_event(byte bVK, byte bScan, int dwFlags, int dwExtraInfo);

    /*
     * TODO:
     * ��ȡ�ֱ������������������action.�����ܹ�ע��.
     * ���¡������Ȼص��������麯�����ӿڣ���������һ��ʼ����Base��ע����Щ�ص�����.
     * �����ھ���ļ������о�ֻ��Ҫʵ����Щ�ص�����������߼�.
     * ���ǵ����ܽ�Ҫ��triggerʵ�֣�Ҳ��������ԼӸ�map�����ߴ洢��ǰλ������ı�����Ȼ��onTriggerEnter�Ƿ�ҲӦ��������?
     * �������ֻ��һ����touch�������ɿ���trigger�Ƿ�����з�Ӧ���������������ʱ��Ҫ������һ��Axis2Key��Action������ӳ��Ϊ���ţ��Ƿ�Ҳ��Ҫ������ʵ��?
     * 
     */


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void PushKey(byte bVK)
    {
        // ���°���
        keybd_event(bVK, 0, (int)Keybd_Flags.KeyDown, 0);
    }

    void ReleaseKey(byte bVK)
    {
        // �ɿ�����
        // һ����Ҫ���ǰ�����Ҫ�ɿ�.
        keybd_event(bVK, 0, (int)Keybd_Flags.KeyUp, 0);
    }
}