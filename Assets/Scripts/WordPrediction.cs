using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* WordPredictionֻ���ܼ����������룬���ҷ����Ƽ��ĵ��ʡ����������κδ�����Ӧ�ô���side effect. */

public class WordPrediction
{
    public int initialCapacity = 82765;
    public int maxEditDistanceDictionary = 2; //maximum edit distance per dictionary precalculation

    public int maxEditDistanceLookup = 1; //max edit distance per lookup (maxEditDistanceLookup<=maxEditDistanceDictionary)
    public SymSpell.Verbosity suggestionVerbosity = SymSpell.Verbosity.Closest; //Top, Closest, All

    string curWord { get; set; } = string.Empty;
    int curLength { get; set; } = 0;   //��ǰ�Ѿ�����ĳ���
    List<SymSpell.SuggestItem> suggestions;

    SymSpell symSpell;

    public WordPrediction()
    {
        symSpell = new SymSpell(initialCapacity, maxEditDistanceDictionary);
        string dictDir = "Assets/SymSpell/frequency_dictionary_en_82_765.txt";
        int termIndex = 0, countIndex = 1;
        if(!symSpell.LoadDictionary(dictDir, termIndex, countIndex))
        {
            Debug.LogError("Cannot find the dictionary for SymSpell.");
        }
    }

    // �����������Ԥ����.
    public void refresh()
    {
        curLength = 0;
        curWord = string.Empty;
    }

    // ��ȡ��ǰ�����˵Ķ���������״̬.
    public void next(int ascii)
    {
        // ����ĸ
        if(('a'<=ascii && ascii<='z') || ('A'<=ascii && 'Z' <= ascii))
        {
            curLength++;
            curWord += Convert.ToString(ascii);
            suggestions = symSpell.Lookup(curWord, suggestionVerbosity, maxEditDistanceLookup);
        }
        // �˸���������
        else if(ascii == (int)VKCode.Back)  //VKCode��Back��ascii���˸��һ����8
        {
            if(curLength > 0)
            {
                curLength--;
                curWord.Remove(curLength);
                if (curLength != 0)
                    suggestions = symSpell.Lookup(curWord, suggestionVerbosity, maxEditDistanceLookup);
                else
                    suggestions.Clear();
            }
        }
        // �����ǿո� ��� ���ֵ�, ��ʱ����Ԥ����
        else
        {
            refresh();
        }
    }

    // ��ȡ״̬.
    public string getCurWord()
    {
        return curWord;
    }

    public int getCurLength()
    {
        return curLength;
    }

    public string[] getSuggestions()
    {
        string[] strSuggest = new string[5];
        int i = 0;
        foreach(var suggest in suggestions)
        {
            strSuggest[i++] = suggest.term;
            if (i >= 5)
                break;
        }
        for(; i<5; ++i)
        {
            strSuggest[i] = string.Empty;
        }
        return strSuggest;
    }

}
