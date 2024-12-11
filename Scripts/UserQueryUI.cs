using UnityEngine;
using TMPro;

public class UserQueryUI : MonoBehaviour
{
    // Reference to the TMP_InputField where the user types their query.
    public TMP_InputField userInputField;

    // Reference to the TextMeshProUGUI element for displaying the AI's response.
    public TextMeshProUGUI aiResponseText;

    // Reference to the TestAICommunication script for handling backend communication.
    public AIResponseHandler aiResponseHandler;

    /// <summary>
    /// Called when the submit button is pressed.
    /// Retrieves the user's input and sends it to the AI backend for processing.
    /// </summary>
    public void OnSubmitButtonPressed()
    {
        // Retrieve the text entered by the user in the input field.
        string userInput = userInputField.text;

        Debug.Log("User Input: " + userInput);

        // Send the user's query to the AI backend via TestAICommunication.
        aiResponseHandler.SendQueryToAI(userInput);
    }

    /// <summary>
    /// Updates the response text UI element with the AI's response.
    /// </summary>
    /// <param name="response">The response received from the AI backend.</param>
    public void DisplayAIResponse(string response)
    {
        if (aiResponseText != null)
        {
            // Update the text UI element with the AI's response.
            aiResponseText.text = "AI Response: " + response;
        }
    }
}
