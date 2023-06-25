using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using Valve.VR;

/* �������������ڻ����ķ����������в�̫һ�����߼����������KeyboardBase�Ļ�����ʵ�ֵ���������. */
public class SlideKeyboard : KeyboardBase
{
    Dictionary<string,char> ALT1 = new Dictionary<string,char>();
    Dictionary<string,char> ALT2 = new Dictionary<string, char>();

    bool have_slide = false;
    Vector3 last_move;
    // Start is called before the first frame update
    void Start()
    {
        /*  ALT1.Add("a",97);
           ALT1.Add("b", 98);
           ALT1.Add("c", 99);
           ALT1.Add("d", 100);
   .        ALT1.Add("a", 97);
           ALT1.Add("b", 98);
           ALT1.Add("c", 99);
         ALT1.Add("a",97);
           ALT1.Add("b", 98);
           ALT1.Add("c", 99);
           ALT1.Add("d", 100);
   .        ALT1.Add("a", 97);
           ALT1.Add("b", 98);
           ALT1.Add("c", 99);
         ALT1.Add("a",97);
        ALT1.Add("b", 98);
        ALT1.Add("c", 99);
        ALT1.Add("d", 100);
.        ALT1.Add("a", 97);
        ALT1.Add("b", 98);
         ALT1.Add("a",97);
        ALT1.Add("b", 98);
        ALT1.Add("c", 99);
        ALT1.Add("d", 100);
.        ALT1.Add("a", 97);
        ALT1.Add("b", 98);
         ALT1.Add("a",97);
        ALT1.Add("b", 98);
        ALT1.Add("c", 99);
        ALT1.Add("d", 100);
.        ALT1.Add("a", 97);
        ALT1.Add("b", 98);
         ALT1.Add("a",97);
        ALT1.Add("b", 98);
        ALT1.Add("c", 99);
        ALT1.Add("d", 100);
.        ALT1.Add("a", 97);
        ALT1.Add("b", 98);
         ALT1.Add("a",97);
        ALT1.Add("b", 98);
        ALT1.Add("c", 99);
        ALT1.Add("d", 100);
.        ALT1.Add("a", 97);
        ALT1.Add("b", 98);
        ALT1.Add("c", 99); 
        ALT1.Add("a",97);
        ALT1.Add("b", 98);
        ALT1.Add("c", 99);
        ALT1.Add("d", 100);
.        ALT1.Add("a", 97);
        ALT1.Add("b", 98);

        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
        ALT2.Add("c", 99);
          */

    }

    // Update is called once per frame
    void Update()
    {
        if(press == true && Time.time-begintime > deltatime)
        {
            Alt.SetActive(true);
            longHolding = true;

        }
        else
        {
            longHolding = false;
            Alt.SetActive(false);
        }



    }
    
    public GameObject controller;
    private Collider target;
    
    private float deltatime = 0.3f;
    private float begintime;
  
    private bool press = false;
    public GameObject Alt;
    private string choose = "";
    private Vector3 EndPos;
    override public void OnTouchDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {

        touched = true;
        //���������ƶ�
     
    }

    override public void OnTouchUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        touched = false;
        have_slide = false;
        //Debug.Log("Touch Up");
        // ���һ���ɿ���˲����Ϊ������ȡ����Ӧ�ð����һ���ƶ����򷵻�ȥ.
        controller.transform.localPosition = controller.transform.localPosition - last_move;
    }

    override public void OnPressDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        /* ��SlideKeyboard�У�Ҫ�������ʼ�жϳ���. */
        press = true;
        if (target != null)
           target.GetComponent<MeshRenderer>().material.color = Color.yellow;
        begintime = Time.time;
   
    }
    private void OnTriggerEnter(Collider other)
    { 
        Debug.Log("enter the"+other.name);
        target = other;
        other.GetComponent<MeshRenderer>().material.color = Color.yellow;

    }
    void OnTriggerExit(Collider other)      //  ��������������  
    {
       
        other.GetComponent<MeshRenderer>().material.color = Color.white;
        target = null;
        Debug.Log("Exit");//ͬ����print("")���
        
    }

    override public void OnPressUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        /* ������ʲô���̣����ﶼ��Ҫ����ַ���! */
       

        if(longHolding)
        {
            if(choose ==  "Right")
            OutputLetter(ALT1[target.name]);
            else if (choose == "Left")
            OutputLetter(ALT2[target.name]);
        }
        else
        {
            OutputLetter(target.name[0]);
        }
        if(target!=null)
        target.GetComponent<MeshRenderer>().material.color = Color.white;
        press = false;
    }

    override public void OnPadSlide(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        /* 
         ��ָ�ڴ������ϻ�����һ������һ���Ĺ����ƶ��ı������һ���ڰ��°����ʱ���ƶ����.
         */
        
        
        if (controller == null) return;

        if (!have_slide)
        {
            have_slide = true;
            return;
        }
        
        else if(selected == false)
        {
            Vector3  move = new Vector3(delta.x*3, 0,delta.y*3);
            //ӳ���߼�
            Debug.Log(move);
            controller.transform.localPosition = controller.transform.localPosition + move;
            last_move = move;
          // EndPos
            if(longHolding)
            {

                if (delta[0]>0)
                {
                    choose = "Right";
                }
                else if (delta[0]<0)
                {
                    choose = "Left";
                }
            }
            else
            {
                choose = "s";
            }
        }
        else
        {   //int move = 0;
            //if (delta[0] > 0)
            //{
            //    move =(int) (delta[0] / 2 * 5);
            //    inputField.caretPosition = inputField.caretPosition + move;
            //}
            //else if (delta[0] < 0)
            //{
            //    move = (int)((-delta[0]) / 2 * 5);
            //    inputField.caretPosition = inputField.caretPosition - move;
            //}
            do_caret_move(axis);
        }

    }



  



}

