using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using Valve.Newtonsoft.Json;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.Animations;
using System.IO;
using System.Linq;
using RANDOM = UnityEngine.Random;
public class Experiment : MonoBehaviour
{
    public int asciiCodeForEnterKey;  // will be used in condition to check when enter key is hit.
    public bool OnExperiment = true;
    public int studentID;
    public int keyboardType = 3;
    
    public TextMeshProUGUI answerSentence;  //那块黑板上的字.
    public TMP_InputField inputField;   //用于获取输入的内容.

    // #Start_of_TypeData_Logs

    

    
    public bool enterKeyPressedExp = false;
    public string LogDir = "Assets/ExpLog/";  //存储实验记录的文件夹路径.  // filepath
    public string fileName;

    public int questionIndex = 0;
    private string[] questions = {"practice makes perfect",
     "the future is here",
     "better things are coming",
     "my preferred treat is chocolate",
     "physics and chemistry are hard",
     "we are subjects and must obey",
     "this is a very good idea",
     "movie about a nutty professor",
     "my bank account is overdrawn",
     "the king sends you to the tower",
     "everyday is a second chance",
 };
    TypeData typeData;
    private string inputStream = string.Empty;

    private int round = 1;
    [SerializeField] private int toolID;
    private float displayQuestionTime = -1f;
    private float curTypeTime = -1f;
    private float preTypeTime = -1f;
    private const int testingQuestionNum = 10;
    private int toolNum = 4;

    public bool isLeftControllerGripPressed = false;
    // #End_of_TypeData_Logs

    public SteamVR_Action_Boolean PadTouch = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("keyboard", "touch");
    public SteamVR_Action_Boolean LeftGripClick = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("keyboard", "gripclick");
    public SteamVR_Action_Vector2 PadSlide = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("keyboard", "slide");
    public SteamVR_Action_Boolean Fitting = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("keyboard", "fitting");



   
    

    Record record;

    float startTime;
    bool touched = false, start = false, fitting = false;

    // FirstTouch firstTouch = new FirstTouch();
    public AudioSource endaudio;  // 结束音效
    // 记录最后一次滑动点所用.
    Vector2 lastSlideAxis = new Vector2(), lastSlideDelta = new Vector2();


    
    // Start is called before the first frame update


    // Awake function

    private void Awake()
    {
        typeData = new TypeData(studentID, toolID, questionIndex);
    }
    void Start()
    {


        endaudio = GetComponent<AudioSource>();

        LogDir = LogDir.Insert(LogDir.Length, fileName);

        if (!File.Exists(LogDir))
        {
            using (StreamWriter outStream = new StreamWriter(LogDir, true, System.Text.Encoding.GetEncoding("utf-8")))
            {
                outStream.WriteLine("StudentName,ToolID,TranscribedLength,Seconds,Correct,IF,INF,WPM,TotalER,CorrectedER,UncorrectedER,InputStream,TranscribedText,F,CorrectionEfficiency,Participant,Utilised,Wasted");
            }
        }

        inputField.ActivateInputField();
        questionIndex = 0;
    }


    // Update is called once per frame
    void Update()
    {
        //inputField.ActivateInputField();
        answerSentence.text = questions[questionIndex];

    }

    private void LateUpdate()
    {
        ProcessData();
    }


    void OnEnable()
    {
        if (OnExperiment)
        {
            Debug.Log("OnEnable is being called");
            SteamVR_Input_Sources[] tmp = new SteamVR_Input_Sources[] { SteamVR_Input_Sources.LeftHand, SteamVR_Input_Sources.RightHand };
            foreach (var hand in tmp)
            {
                PadTouch[hand].onStateDown += OnTouchDown;
                PadTouch[hand].onStateUp += OnTouchUp;
                PadSlide[hand].onChange += OnPadSlide;
                LeftGripClick[hand].onStateDown += OnGripButtonClicked;
                if (keyboardType == 2 || keyboardType == 3)
                {
                    Fitting[hand].onStateDown += OnFitting;
                }
            }
        }
    }

    void OnDisable()
    {
        if (OnExperiment)
        {
            SteamVR_Input_Sources[] tmp = new SteamVR_Input_Sources[] { SteamVR_Input_Sources.LeftHand, SteamVR_Input_Sources.RightHand };
            foreach (var hand in tmp)
            {
                PadTouch[hand].onStateDown -= OnTouchDown;
                PadTouch[hand].onStateUp -= OnTouchUp;
                PadSlide[hand].onChange -= OnPadSlide;
                LeftGripClick[hand].onStateDown -= OnGripButtonClicked;
                if (keyboardType == 2 || keyboardType == 3)
                {
                    Fitting[hand].onStateDown += OnFitting;
                }
            }
        }
    }

    void ProcessData()
    {
        if (asciiCodeForEnterKey == (int)VKCode.Enter && isLeftControllerGripPressed)
        {
            Debug.Log("Enter pressed.");
            asciiCodeForEnterKey = 0; // set zero otherwise this condition will be true
            inputField.text = inputField.text.Replace("\r", "").Replace("\n", "");
            Debug.Log(inputField.text);
            Submit();
            
        }
    }


    // as user click the left controller grip button, system starts recording log data
    public void OnGripButtonClicked(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        isLeftControllerGripPressed = !isLeftControllerGripPressed;
        //OnExperiment = !OnExperiment;
        if (isLeftControllerGripPressed)
        {
            round = 1;
            questionIndex = round;
            inputStream = string.Empty;
            CleanInputField();
        }
    }
    public void RecordLogData(int ascii)
    {
        //inputData += Convert.ToChar(ascii);
        if (round != 0)
        {
            //if (isLeftControllerGripPressed)  // check either start button was pressed if true then calculate.
            //{
                curTypeTime = Time.time;
                if (displayQuestionTime != -1f)
                {
                    typeData.userResponseInterval = curTypeTime - displayQuestionTime;
                    displayQuestionTime = -1f;
                }

                if (preTypeTime != -1f)
                {
                    float typeInterval = curTypeTime - preTypeTime;
                    typeData.S += typeInterval;
                    typeData.intervals.Add(typeInterval);
                }
                preTypeTime = curTypeTime;
            //}

            // save stream
            if (ascii != (int)VKCode.Enter)
            {
                inputStream += Convert.ToChar(ascii);
            }
            
        }

    }





    public void OnTouchDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        // 记录开始时刻.
        if (!start)
        {
            start = true;
            startTime = Time.time;
        }


    }
    // 松手标志
    public void OnTouchUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        touched = false;
    }
    // 记录firsttouch! 

    public void OnFitting(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        fitting = !fitting;
    }
    public void OnPadSlide(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        lastSlideAxis = axis;
        lastSlideDelta = delta;
        // delta太大的不要.这个阈值是跟着clickboard来的.
        if (Mathf.Sqrt(delta.x * delta.x + delta.y * delta.y) > 0.2)
        {
            return;
        }
        // 希望记忆抬手点，所以应该仿照clickkeyboard, 记忆lastSlidePoint 和 lastSlideDelta. 并且舍弃delta向量长度大于0.2的滑动输入.
        if (!fitting && !touched)
        {
            touched = true;
            int lr = fromSource == SteamVR_Input_Sources.LeftHand ? 0 : 1;
            //record.firstTouches.Add(new FirstTouch(phrases[index].ToString(), lr, axis));
            // firstTouch = new FirstTouch("spaceholder", lr, axis, axis, Time.time, Time.time);
        }
    }

    public void setThetaR(float theta, float r)
    {
        record.theta = theta;
        record.r = r;
    }


    void Submit()
    {
        if (inputStream != string.Empty)
        {
            if (round != 0)
            {
                // same as Typing, can reduce code
                curTypeTime = Time.time;
                if (preTypeTime != -1f)
                {
                    float typeInterval = curTypeTime - preTypeTime;
                    typeData.S += typeInterval;
                    typeData.intervals.Add(typeInterval);
                }

                //Compare whole answer & save to log file
                typeData.inputStream = inputStream;
                string transribedText = inputField.text;
                typeData.transribedText = transribedText;
                typeData.T = inputField.text.Length;
                typeData.INF = MSD(questions[questionIndex], transribedText);
                typeData.C = Mathf.Max(questions[questionIndex].Length, transribedText.Length) - typeData.INF;

                for (int i = 0; i < inputStream.Length; i++)
                {
                    if (inputStream[i] == '%')
                    {
                        typeData.F++;
                    }
                }

                typeData.IF = inputStream.Length - transribedText.Length - typeData.F;

                typeData.WMP = (float)(typeData.T - 1) / typeData.S * 60f * 0.2f;
                typeData.TotalER = (float)(typeData.INF + typeData.IF) / (float)(typeData.C + typeData.INF + typeData.IF) * 100f;
                typeData.CorrectedER = (float)typeData.IF / (float)(typeData.C + typeData.INF + typeData.IF) * 100f;
                typeData.UncorrectedER = (float)typeData.INF / (float)(typeData.C + typeData.INF + typeData.IF) * 100f;
                typeData.CorrectionEfficiency = (typeData.F == 0f) ? 0f : (float)typeData.IF / (float)typeData.F;
                typeData.Participant = (typeData.IF + typeData.INF == 0f) ? 0f : (float)typeData.IF / ((float)typeData.IF + (float)typeData.INF);
                typeData.Utilised = (float)typeData.C / ((float)typeData.C + (float)typeData.INF + (float)typeData.IF + (float)typeData.F);
                typeData.Wasted = ((float)typeData.INF + (float)typeData.IF + (float)typeData.F) / ((float)typeData.C + (float)typeData.INF + (float)typeData.IF + (float)typeData.F);
                NextRound();
                //preTypeTime = -1f;
            }
        }
        CleanInputField();
        endaudio.Play();
    }



    int r(char x, char y)
    {
        if (x == y)
            return 0;
        else
            return 1;
    }

    int MSD(string A, string B)
    {
        int[,] D = new int[A.Length + 1, B.Length + 1];
        for (int i = 0; i <= A.Length; i++)
        {
            D[i, 0] = i;
        }
        for (int j = 0; j <= B.Length; j++)
        {
            D[0, j] = j;
        }
        for (int i = 1; i <= A.Length; i++)
            for (int j = 1; j <= B.Length; j++)
            {
                D[i, j] = Mathf.Min(D[i - 1, j] + 1, D[i, j - 1] + 1, D[i - 1, j - 1] + r(A[i - 1], B[j - 1]));
            }
        return D[A.Length, B.Length];

    }


    void NextRound()
    {
        displayQuestionTime = Time.time;
        if (round != 0)
        {
            // Save Data
            using (StreamWriter outStream = new StreamWriter(LogDir, true, System.Text.Encoding.GetEncoding("utf-8")))
            {
                //outStream.WriteLine("StudentID, ToolID, QuestionID,ResponseTime,Intervals,TranscribedLength,Seconds,Correct,IF,INF,WPM,TotalER,CorrectedER,UncorrectedER,InputStream,TranscribedText");
                string timeList = "\"";
                for (int i = 0; i < typeData.intervals.Count; i++)
                {
                    timeList += typeData.intervals[i].ToString();
                    if (i != typeData.intervals.Count - 1)
                    {
                        timeList += ',';
                    }
                }
                timeList += "\"";
                outStream.WriteLine(typeData.studentID.ToString() +
                    "," + typeData.toolID.ToString() +
                    //"," + typeData.questionIndex.ToString() +
                    //"," + typeData.userResponseInterval.ToString() +
                    //"," + timeList +
                    "," + typeData.T.ToString() +
                    "," + typeData.S.ToString() +
                    "," + typeData.C.ToString() +
                    "," + typeData.IF.ToString() +
                    "," + typeData.INF.ToString() +
                    "," + typeData.WMP.ToString() +
                    "," + typeData.TotalER.ToString() +
                    "," + typeData.CorrectedER.ToString() +
                    "," + typeData.UncorrectedER.ToString() +
                    ",\"" + typeData.inputStream.ToString() + "\"" +
                    ",\"" + typeData.transribedText.ToString() + "\"" +
                    "," + typeData.F.ToString() +
                    "," + typeData.CorrectionEfficiency.ToString() +
                    "," + typeData.Participant.ToString() +
                    "," + typeData.Utilised.ToString() +
                    "," + typeData.Wasted.ToString());
            }
        }
        //round = round%questionsPerRound + 1;
        round = round + 1;
        switch (round)
        {
            case testingQuestionNum + 1://end phase
                round = 0;
                questionIndex = 0;
                isLeftControllerGripPressed = false;
                //startButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start";
                toolID = (toolID + 1) % toolNum;
                break;
            default:
                int cur = RANDOM.Range(1, questions.Length);
                while (questionIndex == cur)
                {
                    cur = RANDOM.Range(1, questions.Length);
                }
                questionIndex = cur;
                questionIndex = round;
                break;
        }

        typeData = new TypeData(studentID, toolID, questionIndex);
        inputStream = string.Empty;
        preTypeTime = -1f;
    }


    public void CleanInputField()
    {
        inputField.text = string.Empty;
        inputField.ActivateInputField();
    }
}
