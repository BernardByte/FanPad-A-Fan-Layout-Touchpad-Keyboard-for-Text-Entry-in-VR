using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeData
{
    public int studentID;
    public int toolID;
    public int questionIndex;
    public float userResponseInterval;
    public List<float> intervals;
    public int T;   //transcribed string
    public float S;
    public int C;
    public int F;
    public int IF;
    public int INF;
    public float WMP;  //words per minute
    public float TotalER;
    public float CorrectedER;
    public float UncorrectedER;
    public string inputStream;
    public string transribedText;
    public float CorrectionEfficiency;
    public float Participant;
    public float Utilised;
    public float Wasted;

    public TypeData(int sID, int tID, int question)
    {
        studentID = sID;
        toolID = tID;
        questionIndex = question;
        userResponseInterval = 0f;
        intervals = new List<float>();
        T = 0;
        S = 0f;
        C = 0;
        F = 0;
        IF = 0;
        INF = 0;
        WMP = 0f;
        TotalER = 0f;
        CorrectedER = 0f;
        UncorrectedER = 0f;
        inputStream = string.Empty;
        transribedText = string.Empty;
        CorrectionEfficiency = 0f;
        Participant = 0f;
        Utilised = 0f;
        Wasted = 0f;
    }
}
