using System;
using System.Globalization;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private ClientWebSocket ws;
    private CancellationTokenSource cts;

    public string LatestMessage;

    public HandPacket handData;

    async void Start()
    {
        cts = new CancellationTokenSource();

        await Connect();
    }

    async Task Connect()
    {
        ws = new ClientWebSocket();

        Uri uri = new Uri("ws://127.0.0.1:8765");

        try
        {
            Debug.Log($"[WS] 연결 시도: {uri}");

            await ws.ConnectAsync(uri, cts.Token);

            Debug.Log("[WS] 연결 성공");

            _ = ReceiveLoop();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[WS] 연결 실패: {ex}");
        }
    }

    async Task ReceiveLoop()
    {
        byte[] buffer = new byte[4096];

        while (ws != null && ws.State == WebSocketState.Open)
        {
            try
            {
                WebSocketReceiveResult result =
                    await ws.ReceiveAsync(
                        new ArraySegment<byte>(buffer),
                        cts.Token
                    );

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Debug.LogWarning("[WS] 서버가 연결 종료");

                    await ws.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "close",
                        CancellationToken.None
                    );

                    break;
                }

                string msg = Encoding.UTF8.GetString(
                    buffer,
                    0,
                    result.Count
                );

                LatestMessage = msg;

                //Debug.Log($"[WS RECV] {msg}");

                HandPacket packet = ParsePacket(msg);
                handData = packet;

                if (packet != null)
                {
                    //Debug.Log($"Gesture: {packet.gesture}");
                    //Debug.Log($"Palm Pixel: {packet.palmPixel}");
                    //Debug.Log($"Depth: {packet.depthCm}");
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[WS 오류] {ex}");
                break;
            }
            LatestMessage = "";
        }
    }

    HandPacket ParsePacket(string msg)
    {
        try
        {
            // gesture|(nx,ny,nz)|(wx,wy,wz)|rot|hand|depth|(px,py)

            string[] split = msg.Split('|');

            if (split.Length < 7)
            {
                Debug.LogWarning("[PARSE] 필드 부족");
                return null;
            }

            HandPacket p = new HandPacket();

            p.gesture = split[0];

            p.palmNormal = ParseVector3(split[1]);

            p.wrist = ParseVector3(split[2]);

            p.rotation =
                float.Parse(
                    split[3],
                    CultureInfo.InvariantCulture
                );

            p.handedness = split[4];

            p.depthCm =
                float.Parse(
                    split[5],
                    CultureInfo.InvariantCulture
                );

            p.palmPixel = ParseVector2(split[6]);

            return p;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[PARSE ERROR] {ex}");
            return null;
        }
    }

    Vector3 ParseVector3(string s)
    {
        // "(1,2,3)"

        s = s.Replace("(", "").Replace(")", "");

        string[] v = s.Split(',');

        return new Vector3(
            float.Parse(v[0], CultureInfo.InvariantCulture),
            float.Parse(v[1], CultureInfo.InvariantCulture),
            float.Parse(v[2], CultureInfo.InvariantCulture)
        );
    }

    Vector2 ParseVector2(string s)
    {
        // "(100,200)"

        s = s.Replace("(", "").Replace(")", "");

        string[] v = s.Split(',');

        return new Vector2(
            float.Parse(v[0], CultureInfo.InvariantCulture),
            float.Parse(v[1], CultureInfo.InvariantCulture)
        );
    }

    async void OnApplicationQuit()
    {
        try
        {
            cts?.Cancel();

            if (ws != null)
            {
                if (ws.State == WebSocketState.Open)
                {
                    await ws.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "quit",
                        CancellationToken.None
                    );
                }

                ws.Dispose();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }
}

[Serializable]
public class HandPacket
{
    public string gesture;

    public Vector3 palmNormal;

    public Vector3 wrist;

    public float rotation;

    public string handedness;

    public float depthCm;

    public Vector2 palmPixel;
}