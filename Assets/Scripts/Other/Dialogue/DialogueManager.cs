using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Animator anim;
    public GameObject player;
    public float sentenceDelayTime;

    bool canDisplay = true;
    bool talking = false;
    Queue<string> sentences;
    PlayerController playerController;
    
    void Start()
    {
        sentences = new Queue<string>();
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetAxisRaw("Fire1") > 0 && canDisplay && talking)
        {
            StartCoroutine(SentenceDelay());
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        playerController.enabled = false;
        talking = true;        
        player.GetComponent<Animator>().SetFloat("Speed", 0);
        player.GetComponent<Animator>().SetBool("Grounded", true);
        anim.SetBool("isOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(SentenceDelay());
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.03f);
        }
    }

    IEnumerator SentenceDelay()
    {
        canDisplay = false;
        yield return new WaitForSeconds(0.25f);
        canDisplay = true;
    }

    void EndDialogue()
    {
        talking = false;
        anim.SetBool("isOpen", false);
        playerController.enabled = true;
    }
}
