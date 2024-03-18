using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Make sure to include the TextMeshPro namespace

[System.Serializable]
public class TextBoxClass
{
    /// <summary>
    /// message of the text
    /// </summary>
    public string message;
    /// <summary>
    /// Author of the message, can be anything. Like "Thought"
    /// </summary>
    public string author;
}

public class TextBoxMaster : MonoBehaviour
{
    public static TextBoxMaster instance { get; private set; }
    /// <summary>
    /// Message text
    /// </summary>
    [SerializeField] private TMP_Text messageText;
    /// <summary>
    /// Source of the message text
    /// </summary>
    [SerializeField] private TMP_Text authorText;
    /// <summary>
    /// Hint for the player to skip to next message
    /// </summary>
    [SerializeField] private TMP_Text skipHintText;
    private GameObject textBox;
    /// <summary>
    /// Queue of the messages about to be displayed
    /// </summary>
    private Queue<TextBoxClass> messagesQueue = new Queue<TextBoxClass>();
    /// <summary>
    /// Are we displaying the text box currently?
    /// </summary>
    private bool isDisplaying = false;
    /// <summary>
    /// Are we showing the hint to skip to next message?
    /// </summary>
    private bool showBlinkingSkipText = true;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //disable on start
        textBox = transform.GetChild(0).gameObject;
        textBox.SetActive(false);
    }

    private void Update()
    {
        CheckBlinkingText();
        TestCheats();
    }
    void CheckBlinkingText()
    {
        skipHintText.alpha = 0;
        //keeps it blinking
        if (showBlinkingSkipText)
            skipHintText.alpha = ((Time.time * 2f) % 2f < 1f) ? 1f : 0.5f;
    }
    void TestCheats()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            //test message
            ShowTextBox(new TextBoxClass
            {
                message = "The text master is functional",
                author = "Game"
            });
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            //test multiple message
            List<TextBoxClass> textsToShow = new()
            {
                new TextBoxClass
                {
                    message = "The first message works...",
                    author = "Game"
                },
                new TextBoxClass
                {
                    message = "and of course, the second message as well!",
                    author = "Game"
                }
            };
            ShowTextBox(textsToShow);
        }
    }
    /// <summary>
    /// Show a text message List in-game, wont work if a text is already showing
    /// </summary>
    /// <param name="textToShow"></param>
    public void ShowTextBox(List<TextBoxClass> textToShow)
    {
        foreach (var text in textToShow)
        {
            messagesQueue.Enqueue(text);
        }

        if (!isDisplaying)
        {
            StartCoroutine(DisplayMessages());
        }
    }
    /// <summary>
    /// Show a singular text message in-game, wont work if a text is already showing
    /// </summary>
    /// <param name="textToShow"></param>
    public void ShowTextBox(TextBoxClass textToShow, bool autoSkip = false)
    {
        messagesQueue.Enqueue(textToShow);


        if (!isDisplaying)
        {
            StartCoroutine(DisplayMessages(autoSkip));
        }
    }
    private IEnumerator DisplayMessages(bool autoSkip = false)
    {
        showBlinkingSkipText = !autoSkip;
        isDisplaying = true;
        textBox.SetActive(true);
        while (messagesQueue.Count > 0)
        {
            var currentMessage = messagesQueue.Dequeue();
            authorText.text = currentMessage.author;
            messageText.text = ""; // Clear previous message

            // Display message letter by letter
            foreach (char letter in currentMessage.message.ToCharArray())
            {
                messageText.text += letter;
                yield return null; // Wait for the next frame before continuing
            }
            if (autoSkip)
                yield return new WaitForSeconds(2f);
            else
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return)); // Wait for ENTER key to continue
        }
        textBox.SetActive(false);
        isDisplaying = false;
    }
}
