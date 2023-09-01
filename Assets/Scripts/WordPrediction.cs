using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* WordPredictionֻ���ܼ����������룬���ҷ����Ƽ��ĵ��ʡ����������κδ�����Ӧ�ô���side effect. */

public class WordPrediction
{
    public int initialCapacity = 82765;
    public int maxEditDistanceDictionary = 4; //maximum edit distance per dictionary precalculation

    public int maxEditDistanceLookup = 4; //max edit distance per lookup (maxEditDistanceLookup<=maxEditDistanceDictionary)
    public SymSpell.Verbosity suggestionVerbosity = SymSpell.Verbosity.All; //Top, Closest, All

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

    public WordPrediction(int _maxEditDistanceDictionary, int _maxEditDistanceLookup)
    {
        maxEditDistanceDictionary = _maxEditDistanceDictionary;
        maxEditDistanceLookup = _maxEditDistanceLookup;
        symSpell = new SymSpell(initialCapacity, maxEditDistanceDictionary);
        string dictDir = "Assets/SymSpell/frequency_dictionary_en_82_765.txt";
        int termIndex = 0, countIndex = 1;
        if (!symSpell.LoadDictionary(dictDir, termIndex, countIndex))
        {
            Debug.LogError("Cannot find the dictionary for SymSpell.");
        }
    }

    // �����������Ԥ����.
    public void refresh()
    {
        curLength = 0;
        curWord = string.Empty;
        if(suggestions != null)
            suggestions.Clear();
    }

    // ��ȡ��ǰ�����˵Ķ���������״̬.
    public void next(int ascii)
    {
        // ����ĸ
        if(('a'<=ascii && ascii<='z') || ('A'<=ascii && ascii <= 'Z'))
        {
            curLength++;
            byte[] letterArray = new byte[1]{(byte)ascii};
            curWord += System.Text.Encoding.ASCII.GetString(letterArray);
            suggestions = symSpell.Lookup(curWord.ToLower(), suggestionVerbosity, maxEditDistanceLookup);//�����дҲӦ����Ч.
        }
        // �˸���������
        else if(ascii == (int)VKCode.Back)  //VKCode��Back��ascii���˸��һ����8
        {
            if(curLength > 0)
            {
                curWord = curWord.Remove(--curLength);
                if (curLength != 0)
                    suggestions = symSpell.Lookup(curWord.ToLower(), suggestionVerbosity, maxEditDistanceLookup);
                else
                    suggestions.Clear();
            }
        }
        // �����ǿո� ��� ���ֵ�, ��ʱ����Ԥ����
        else
        {
            refresh();
        }
        //Debug.LogWarning("in wordprediction: " + curLength.ToString() + ", " + curWord);
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
        if(suggestions != null){
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
        }
        else{
            for(; i<5; ++i)
            {
                strSuggest[i] = string.Empty;
            }
        }
        return strSuggest;
    }

}
