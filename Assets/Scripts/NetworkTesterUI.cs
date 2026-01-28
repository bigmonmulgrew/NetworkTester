using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkTesterUI : MonoBehaviour
{
    [SerializeField] TMP_Text consoleLog;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] int maxLines = 200;

    Queue<string> lines = new();

    StringBuilder sb = new();

    void Awake()
    {
        sb.AppendLine("Microsoft Windows [Version 10.0.19045]");
        sb.AppendLine("(C) Microsoft Corporation. All rights reserved.\n");
        sb.AppendLine("C:\\>");


        consoleLog.text = sb.ToString();

        //StartCoroutine(PrintLine());
    }

    public void Print(string line)
    {
        lines.Enqueue(line);

        if (lines.Count > maxLines && maxLines > 0)
            lines.Dequeue();

        sb.Clear();
        foreach (var l in lines)
            sb.AppendLine(l);

        consoleLog.text = sb.ToString();
    }

    IEnumerator PrintLine()
    {
        while (true) 
        {
            Print($"Time : {Time.time}");
            yield return new WaitForSeconds(1.0f);
        }
    }
}
