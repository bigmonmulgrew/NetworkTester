using UnityEngine;
using System.Net.NetworkInformation;
using Ping = System.Net.NetworkInformation.Ping;

public class NetworkTester : MonoBehaviour
{
    #region Constants
    const string DEFAULT_ADDRESS = "127.0.0.1";
    const int DEFAULT_TIMEOUT = 1000;
    const int DEFAULT_BUFFER_SIZE = 32;
    const int DEFAULT_TTL = 128;
    const bool DEFAULT_DONT_FRAGMENT = false;
    #endregion

    NetworkTesterUI networkTesterUI;
    private void Awake()
    {
        networkTesterUI = FindFirstObjectByType<NetworkTesterUI>();
    }
    void Start()
    {
      
    }

    void OnEnable()
    {
        if (networkTesterUI != null) networkTesterUI.OnBeginTest += HandleBeginTests;
    }

    void OnDisable()
    {
        if (networkTesterUI != null) networkTesterUI.OnBeginTest -= HandleBeginTests;
    }

    void HandleBeginTests()
    {
        Debug.Log("Begin test requested from UI");
        BeginTests();

    }

    void BeginTests()
    {
        PingHost(networkTesterUI.Address);
        string output = PingHost(networkTesterUI.Address);
        networkTesterUI.Print(output);
    }
    string PingHost(string address, int timeout, int bufferSize, int ttl, bool dontFragment)
    {
        try
        {
            using Ping pinger = new Ping();

            byte[] buffer = new byte[bufferSize];
            PingOptions options = new PingOptions(ttl, dontFragment);

            PingReply reply = pinger.Send(address, timeout, buffer, options);

            return FormatReply(reply);
        }
        catch (PingException ex)
        {
            return $"Ping Failed: {ex.Message}";
        }
    }
    #region PingHost overlaods
    string PingHost(string address) { return PingHost(address, DEFAULT_TIMEOUT, DEFAULT_BUFFER_SIZE, DEFAULT_TTL, DEFAULT_DONT_FRAGMENT); }
    
    #endregion

    string FormatReply(PingReply reply)
    {
        if (reply.Status != IPStatus.Success) return $"Ping Status: {reply.Status}";

        return $"Reply from {reply.Address} " +
               $"time={reply.RoundtripTime}ms " +
               $"TTL={reply.Options.Ttl} " +
               $"Size={reply.Buffer.Length}";
    }
}
