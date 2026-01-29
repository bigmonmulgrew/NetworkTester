using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NetworkTesterUI : MonoBehaviour
{
    public event Action OnBeginTest;

    #region Config
    [Header("Console Config")]
    [SerializeField] TMP_Text consoleLog;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] int maxLines = 200;


    [Header("Ping Config Inputs")]
    [SerializeField] TMP_InputField addressInput;

    [SerializeField] TMP_InputField timeoutInput;
    [SerializeField] Slider timeoutSlider;
    
    [SerializeField] TMP_InputField bufferSizeInput;
    [SerializeField] Slider bufferSizeSlider;

    [SerializeField] TMP_InputField ttlInput;
    [SerializeField] Slider ttlSlider;

    [SerializeField] Toggle dontFragmentToggle;

    [SerializeField] TMP_InputField testCountInput;
    [SerializeField] Slider testCountSlider;

    [Header("Stress testing settings")]
    [SerializeField] int repeatCount = 1;
    #endregion

    #region Runtime Variables
    Queue<string> lines = new();

    StringBuilder sb = new();
    #endregion

    #region Properties

    public string Address => string.IsNullOrWhiteSpace(addressInput.text) ? "8.8.8.8" : addressInput.text;
    public int Timeout => int.TryParse(timeoutInput.text, out int v) ? v : 1000;
    public int BufferSize => int.TryParse(bufferSizeInput.text, out int v) ? v : 32;
    public int TTL => int.TryParse(ttlInput.text, out int v) ? v : 128;
    public bool DontFragment => dontFragmentToggle.isOn;
    public int TestCount => int.TryParse(testCountInput.text, out int v) ? v : 1;
    #endregion

    void Awake()
    {
        SetupInputs();
        PrintConsoleStart();

    }

    private void PrintConsoleStart()
    {
        sb.AppendLine("Network Analysis ready to Run");
        sb.AppendLine("Setup configuration options and then hit \"Run\"\n");

        consoleLog.text = sb.ToString();
    }

    void SetupInputs()
    {
        timeoutInput.onValueChanged.AddListener(OnTimeoutInputChanged);
        timeoutSlider.onValueChanged.AddListener(OnTimeoutSliderChanged);

        bufferSizeInput.onValueChanged.AddListener(OnBufferSizeInputChanged);
        bufferSizeSlider.onValueChanged.AddListener(OnBufferSizeSliderChanged);

        ttlInput.onValueChanged.AddListener(OnTTLInputChanged);
        ttlSlider.onValueChanged.AddListener(OnTTLSliderChanged);

        testCountInput.onValueChanged.AddListener(OnTestCountInputChanged);
        testCountSlider.onValueChanged.AddListener(OnTestCountSliderChanged);
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

    public void BeginTestButtonPressed()
    {
        OnBeginTest?.Invoke();
    }
    void OnTimeoutInputChanged(string value) 
    {
        int output = int.TryParse(value, out int i) ? i : 1000;
        if (output > timeoutSlider.maxValue) output = (int)timeoutSlider.maxValue;
        timeoutSlider.value = output;
        timeoutInput.text = output.ToString();
    }
    void OnTimeoutSliderChanged(float value) { timeoutInput.text = value.ToString(); }

    void OnBufferSizeInputChanged(string value)
    {
        int output = int.TryParse(value, out int i) ? i : 32;
        if (output > bufferSizeSlider.maxValue) output = (int)bufferSizeSlider.maxValue;
        bufferSizeSlider.value = output;
        bufferSizeInput.text = output.ToString();
    }
    void OnBufferSizeSliderChanged(float value) { bufferSizeInput.text = value.ToString(); }

    void OnTTLInputChanged(string value)
    {
        int output = int.TryParse(value, out int i) ? i : 128;
        if (output > ttlSlider.maxValue) output = (int)ttlSlider.maxValue;
        ttlSlider.value = output;
        ttlInput.text = output.ToString();
    }
    void OnTTLSliderChanged(float value) { ttlInput.text = value.ToString(); }

    void OnTestCountInputChanged(string value)
    {
        int output = int.TryParse(value, out int i) ? i : 128;
        if (output > testCountSlider.maxValue) output = (int)testCountSlider.maxValue;
        testCountSlider.value = output;
        testCountInput.text = output.ToString();
    }
    void OnTestCountSliderChanged(float value) { testCountInput.text = value.ToString(); }

}
