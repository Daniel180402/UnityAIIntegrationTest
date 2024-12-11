using UnityEngine;
using System.Text.RegularExpressions;

public class AIResponseHandler : MonoBehaviour
{
    // Reference to the AICommunication script responsible for backend communication.
    public AICommunication aiComm;

    // Reference to the UserQueryUI script for updating the UI with the AI's response.
    public UserQueryUI userQueryUI;

    /// <summary>
    /// Sends a user query to the AI backend for processing.
    /// </summary>
    /// <param name="userInput">The user's query as a string.</param>
    public void SendQueryToAI(string userInput)
    {
        Debug.Log("Sending Query to AI: " + userInput);

        // Start the coroutine to send the query to the backend and handle the response.
        StartCoroutine(aiComm.SendUserInput(userInput, OnResponseReceived));
    }

    /// <summary>
    /// Callback invoked when a response is received from the AI backend.
    /// Decodes the response and updates the UI.
    /// </summary>
    /// <param name="response">The raw response string from the AI backend.</param>
    void OnResponseReceived(string response)
    {
        if (response != null)
        {
            // Decode Unicode escape sequences in the response to produce human-readable text.
            string decodedResponse = DecodeUnicode(response);

            Debug.Log("AI Response: " + decodedResponse);

            // Display the AI's response in the UI.
            userQueryUI.DisplayAIResponse(decodedResponse);
        }
        else
        {
            Debug.LogError("Failed to get response from server");

            userQueryUI.DisplayAIResponse("Error: Could not get a response from the server.");
        }
    }

    /// <summary>
    /// Decodes Unicode escape sequences (e.g., "\u00e0") into readable characters.
    /// </summary>
    /// <param name="input">The string containing Unicode escape sequences.</param>
    /// <returns>The decoded string with readable characters.</returns>
    private string DecodeUnicode(string input)
    {
        // Use Regex.Unescape to replace Unicode escape sequences with their actual characters.
        return Regex.Unescape(input);
    }
}
