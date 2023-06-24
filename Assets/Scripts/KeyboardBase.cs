
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Valve.VR;
using Valve.VR.InteractionSystem;
using TMPro;

public enum Keybd_Flags
{
    KeyDown = 0,
    KeyHold = 1,
    KeyUp = 2,
}

public enum SEEK_MOD
{
    Start = 0,
    Current = 1,
    End = 2
}

public class KeyboardBase : MonoBehaviour
{
    // Start is called before the first frame update
    [DllImport("User32.dll", EntryPoint = "keybd_event")]
    static extern void keybd_event(byte bVK, byte bScan, int dwFlags, int dwExtraInfo);

    //Keyboard action set
    public SteamVR_ActionSet keyboardActionSet;

    // fetch actions.
    public SteamVR_Action_Boolean DeleteKey = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("keyboard", "delete");//ɾ����-ɾ���ַ�
    public SteamVR_Action_Boolean SelectKey = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("keyboard", "select");//�����-�ƶ����
    public SteamVR_Action_Boolean PadTouch = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("keyboard", "touch");  //touchpad����
    public SteamVR_Action_Boolean PadPress = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("keyboard", "press");  //touchpad����
    public SteamVR_Action_Vector2 PadSlide = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("keyboard", "slide");  //��touchpad�ϻ���

    // text input field. 
    public TextMeshProUGUI inputText;
    public TMP_InputField inputField;

    protected bool selected = false, deleted = false, touched = false, longHolding = false;
    protected float last_delete_time, hold_time_start;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {
        Debug.Log("Enable keyboard action set and relative callback functions!");
        keyboardActionSet.Activate();
        DeleteKey.onStateDown += OnDeleteKeyDown;
        DeleteKey.onStateUp += OnDeleteKeyUp;
        DeleteKey.onState += OnDeleteKeyHolding;
        SelectKey.onStateDown += OnSelectKeyDown;
        SelectKey.onStateUp += OnSelectKeyUp;
        PadTouch.onStateDown += OnTouchDown;
        PadTouch.onStateUp += OnTouchUp;
        PadPress.onStateUp += OnPressUp;
        PadPress.onStateDown += OnPressDown;
        PadSlide.onChange += OnPadSlide;
    }

    private void OnDisable()
    {
        Debug.Log("Disable keyboard action set and relative callback functions!");
        keyboardActionSet.Deactivate();
        DeleteKey.onStateDown -= OnDeleteKeyDown;
        DeleteKey.onStateUp -= OnDeleteKeyUp;
        DeleteKey.onState -= OnDeleteKeyHolding;
        SelectKey.onStateDown -= OnSelectKeyDown;
        SelectKey.onStateUp -= OnSelectKeyUp;
        PadTouch.onStateDown -= OnTouchDown;
        PadTouch.onStateUp -= OnTouchUp;
        PadPress.onStateUp -= OnPressUp;
        PadPress.onStateDown -= OnPressDown;
        PadSlide.onChange -= OnPadSlide;
    }

    // �ƶ�����ɾ���߼�����Щ�����м����ж���һ����.
    public void OnSelectKeyUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        selected = false;
    }

    public void OnSelectKeyDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        selected = true;
    }

    public void OnDeleteKeyUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        /* �ɿ�ɾ���� */
        deleted = false;
    }

    public void OnDeleteKeyDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        /* ����ɾ���� */
        deleted = true;
        last_delete_time = Time.time;
        do_delete_char();
    }

    public void OnDeleteKeyHolding(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        /* ����ɾ��������onState�Ļص���������ΪonState����Ҫ��true�Ŵ��������Բ����ж��Ƿ�true. */
        // ����ɾ��̫�죬�������0.2s��ɾ.
        // TODO
        if(Time.time - last_delete_time > 0.2f)
        {
            last_delete_time = Time.time;
            do_delete_char();
        }
    }

    // �����Ǵ�������صģ�ÿ�����̲�̫һ��.��������������!
    virtual public void OnTouchDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        touched = true;
    }

    virtual public void OnTouchUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        touched = false;
    }

    virtual public void OnPressDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        /* ��SlideKeyboard�У�Ҫ�������ʼ�жϳ���. */
    }

    virtual public void OnPressUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        /* ������ʲô���̣����ﶼ��Ҫ����ַ���! */        
    }

    virtual public void OnPadSlide(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        /* 
         ��ָ�ڴ������ϻ�����һ������һ���Ĺ����ƶ��ı������һ���ڰ��°����ʱ���ƶ����.
         */

    }

    // Core
    virtual public int Axis2Letter(Vector2 axis, SteamVR_Input_Sources hand, int mode, ref GameObject key)
    {
        // ����ǰλ�ô������λ��ת��ΪҪ�����Ascii�ַ�. ���Ұѵ�ǰ�����ļ���λ�õİ�����GameObject
        // mode: ����״̬�������д��״̬��������ŵ�״̬. 0- Сд�� 1-��д�� 2-�������.
        return 0;
    }

    public void OutputLetter(int ascii)
    {
        // ��Ascii���ŷ���Ϊ����.
        // TODO
    }
    
    void do_delete_char()
    {
        // ��inputField�ĵ�ǰλ��ɾ��һ���ַ�.
        // ֱ��������һ��backspaceʵ��ɾ��.
        PushKey((byte)VKCode.Back);
        ReleaseKey((byte)VKCode.Back);
    }

    void Seek(SEEK_MOD mode, int offset)
    {
        // �ƶ� inputField �Ĺ��.
        // ��Զ��offset���Ӻţ�������ƶ���ĩβ��offset��Ӧ���Ǹ���!
        if(mode == SEEK_MOD.Start)
        {
            inputField.caretPosition = offset;
        }
        else if(mode == SEEK_MOD.Current)
        {
            inputField.caretPosition += offset;
        }
        else
        {
            inputField.MoveTextEnd(false);  //ֱ���ƶ���ĩβ.
            inputField.caretPosition += offset;
        }
    }

    void PushKey(byte bVK)
    {
        keybd_event(bVK, 0, (int)Keybd_Flags.KeyDown, 0);
    }

    void ReleaseKey(byte bVK)
    {
        keybd_event(bVK, 0, (int)Keybd_Flags.KeyUp, 0);
    }
}