using UnityEngine;
using System.Net.NetworkInformation;
using Ping = System.Net.NetworkInformation.Ping;

public class NetworkTester : MonoBehaviour
{
    NetworkTesterUI networkTesterUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        networkTesterUI = FindFirstObjectByType<NetworkTesterUI>();

        string output = PingHost("192.168.0.1");
        networkTesterUI.Print(output);
        Debug.Log(output);
    }

   string PingHost(string nameOrAddress)
    {
        try
        {
            using (Ping pinger = new Ping())
            {
                PingReply reply = pinger.Send(nameOrAddress);
                return reply.Status.ToString();
            }
        }
        catch (PingException)
        {
            return "Failed";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
