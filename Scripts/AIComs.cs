using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class AICommunication : MonoBehaviour
{
    // The URL of the Python backend server. Modify this if the server is hosted on a different machine or port.
    private string serverUrl = "http://localhost:5000/get_response";

    /// <summary>
    /// Sends user input to the Python backend server and invokes a callback with the server's response.
    /// </summary>
    /// <param name="userInput">The user's query as a string.</param>
    public IEnumerator SendUserInput(string userInput, System.Action<string> callback)
    {
        // Prepare the JSON payload by escaping any special characters in the user input.
        string jsonPayload = "{\"user_input\":\"" + EscapeJson(userInput) + "\"}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);

        // Create a new UnityWebRequest to send a POST request to the server.
        using (UnityWebRequest www = new UnityWebRequest(serverUrl, "POST"))
        {
            // Attach the JSON payload to the request.
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || 
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
                callback?.Invoke(null);
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;
                string responseText = ParseResponse(jsonResponse);
                callback?.Invoke(responseText);
            }
        }
    }

    /// <summary>
    /// Extracts the value of the "response" field from a JSON string.
    /// </summary>
    /// <returns>The extracted response text, or an empty string if parsing fails.</returns>
    private string ParseResponse(string jsonResponse)
    {
        // Locate the start of the "response" field in the JSON string.
        int startIndex = jsonResponse.IndexOf("\"response\":\"");
        if (startIndex != -1)
        {
            startIndex += "\"response\":\"".Length; // Move the index to the start of the actual response value.
            int endIndex = jsonResponse.IndexOf("\"", startIndex); // Find the closing quote of the response value.
            if (endIndex > startIndex)
            {
                // Extract and return the response value from the JSON string.
                return jsonResponse.Substring(startIndex, endIndex - startIndex);
            }
        }
        // Return an empty string if parsing fails or if the field is missing.
        return "";
    }

    /// <summary>
    /// Escapes special characters in a string to make it JSON-safe.
    /// </summary>
    /// <param name="s">The string to be escaped.</param>
    /// <returns>The escaped string.</returns>
    private string EscapeJson(string s)
    {
        // Replace backslashes and quotes with their escaped equivalents.
        return s.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }
}
