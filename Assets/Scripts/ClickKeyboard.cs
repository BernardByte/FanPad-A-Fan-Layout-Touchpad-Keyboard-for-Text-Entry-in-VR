using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using TMPro;

/* 
 * ԭʼ���̡���б���������Ļ��ࡣ���������̵��߼�������ȫһ����ֻ��ӳ�䷽ʽ��ͬ.
 * ����ű�Ӧ��ʵ��ClickKeyboard�������߼���ֻ����Axis2Letter������������д!
 */
public class ClickKeyboard : KeyboardBase
{
    public Transform symbolBox;

    GameObject hoveringKey, checkKey = null;   // hoveringKey�ǵ�ǰ�����ڵİ�����checkKey�������жϳ�����
    Color oldColor, hoveringColor = new Color(255, 255, 0, 60);
    int _mode = 0;   //���ģʽ״̬��0-Сд��1-��д(����һ��Shift), 2-�����ַ�(����)
    bool isCapitalDisplay = false;   // �Ǵ�дչʾ�ļ���.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (touched)
        {
            if (checkKey == null)
            {
                checkKey = hoveringKey;
                return;
            }
            if (_mode != 2)  //��û�г���.
            {
                if (checkKey != hoveringKey)
                {
                    // hoveringKey�ı���.���¼�ʱ.
                    hold_time_start = Time.time;
                    checkKey = hoveringKey;
                }
                else if(Time.time - hold_time_start > 1)
                {
                    // ����1s, ��������ſ�.
                    _mode = 2;
                    // TODO: ��symbolBox��λ�ø�ֵ.
                    symbolBox.position = hoveringKey.transform.position;  //��ûд�꣬Ҫ�����������ڳ�������ô�����޸�.
                    symbolBox.gameObject.SetActive(true);
                    hoveringKey.GetComponent<MeshRenderer>().material.color = oldColor;
                    hoveringKey = symbolBox.Find("Rectangle002").gameObject;
                    Material mat = hoveringKey.GetComponent<MeshRenderer>().material;
                    oldColor = mat.color;
                    mat.color = hoveringColor;  //��ɫ.
                }
            }
        }
    }

    // OnTouchDown
    public override void OnTouchDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        base.OnTouchDown(fromAction, fromSource);  //touched = true.
        // ��Ҫ��¼����!.
        hold_time_start = Time.time;
        // �ʼ��¼��ǰ���ĸ�������.
        Axis2Letter(PadSlide[fromSource].axis, fromSource, _mode, out hoveringKey);
        Material material = hoveringKey.GetComponent<MeshRenderer>().material;
        oldColor = material.color;
        material.color = hoveringColor;  //�ı䵱ǰ����������ɫ!
    }

    // OnTouchUp, �ɿ������壬����Ҫ�����!
    public override void OnTouchUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        base.OnTouchUp(fromAction, fromSource);  // touched = false.
        if (selected || deleted)  //��������ƶ�������ɾ���ַ����͵�����Ч!
            return;
        GameObject tmp;
        int ascii = Axis2Letter(PadSlide[fromSource].axis, fromSource, _mode, out tmp);
        // ������Ƽ���Ŀǰֻ��VKCode.Shift.
        if(ascii == (int)VKCode.Shift)  //Shift, mode��0��1����1��0. �������Ƽ���ʱ��_mode��Ӧ���ܱ�Ϊ2.
        {
            _mode = _mode == 1 ? 0 : 1;
            isCapitalDisplay = !isCapitalDisplay;
            foreach(var key in gameObject.GetComponentsInChildren<TextMeshProUGUI>())
            {
                // TODO: ��������ĸ���Ĵ�Сдת��. �����TextMeshProUGUIֻ��һ�����ܵ�ʵ�֣��������õ�������Ļ����ǻ�Ӧ����������.
                // �����µ�isCapitalDisplay�Ĵ�Сд.
            }
        }
        // ����������Ƽ�...(����)
        // ����������Ƽ���ȷʵҪ����ַ�.
        else
        {
            OutputLetter(ascii);
            _mode = _mode == 2 ? (isCapitalDisplay ? 1 : 0) : _mode;  //�����2����ص�ԭ�ȵ�״̬.
            if(_mode == 2)
            {
                _mode = isCapitalDisplay ? 1 : 0;
                symbolBox.gameObject.SetActive(false); // ������ſ�
            }
        }
        checkKey = null;   //checkKey�ÿգ�Ϊ�´δ�����׼��.
    }

    // ClickKeyboard�еİ��´�����û���ر�����壬�������������Ű�. PressUpһ������TouchUp�������ٵ���һ��.

    // Core: OnPadSlide.
    public override void OnPadSlide(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        // ��������������ǿյģ�����.
        if (deleted)  // ����ɾ��������!
            return;
        if (selected)
        {
            //���˰�������ƶ���ֻ꣬�����꣬��������.
            do_caret_move(axis);
        }
        else
        {
            // ���Ҫ��С��ָʾ�Ļ���Ҳ�������С���λ��
            // ���������Ϊ�����ڼ������ƶ�.
            GameObject oldkey = hoveringKey;
            Axis2Letter(axis, fromSource, _mode, out hoveringKey);
            if(oldkey != hoveringKey)
            {
                // ��ɫ.
                oldkey.GetComponent<MeshRenderer>().material.color = oldColor;
                Material mat = hoveringKey.GetComponent<MeshRenderer>().material;
                oldColor = mat.color;
                mat.color = hoveringColor;
            }
        }
    }
}
