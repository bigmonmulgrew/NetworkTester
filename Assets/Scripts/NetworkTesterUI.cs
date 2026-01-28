using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkTesterUI : MonoBehaviour
{
    #region Config
    [Header("Console Config")]
    [SerializeField] TMP_Text consoleLog;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] int maxLines = 200;


    [Header("Ping Config Inputs")]
    [SerializeField] TMP_InputField addressInput;
    [SerializeField] TMP_InputField timeoutInput;
    [SerializeField] TMP_InputField bufferSizeInput;
    [SerializeField] TMP_InputField ttlInput;
    [SerializeField] TMP_InputField dontFragmentInput; // 0 or 1
    #endregion

    #region Runtime Variables
    Queue<string> lines = new();

    StringBuilder sb = new();
    #endregion

    // ---------- Typed Getters ----------

    public string Address => string.IsNullOrWhiteSpace(addressInput.text) ? "8.8.8.8" : addressInput.text;
    public int Timeout => int.TryParse(timeoutInput.text, out int v) ? v : 1000;
    public int BufferSize => int.TryParse(bufferSizeInput.text, out int v) ? v : 32;
    public int TTL => int.TryParse(ttlInput.text, out int v) ? v : 128;
    public bool DontFragment => dontFragmentInput.text == "1" || dontFragmentInput.text.ToLower() == "true";

    void Awake()
    {
        sb.AppendLine("Network Analysis ready to Run");
        sb.AppendLine("Setup configuration options and then hit \"Run\"\n");

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
