using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/* �������������ڻ����ķ����������в�̫һ�����߼����������KeyboardBase�Ļ�����ʵ�ֵ���������. */
public class SlideKeyboard : KeyboardBase
{
    int row = 2;
    int column = 2;
    public override void OnPadSlide(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {

        /* 
         ��ָ�ڴ������ϻ�����һ������һ���Ĺ����ƶ��ı������һ���ڰ��°����ʱ���ƶ����.
         */

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
