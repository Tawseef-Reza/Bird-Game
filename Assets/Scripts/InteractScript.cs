using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractScript : MonoBehaviour
{
    [SerializeField] private Button Interact;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject[] Notes;
    private GameObject CurrentNote;
    [SerializeField] GameObject NoteImage;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Notes.Length; i++)
        {
            if (Vector3.Distance(Notes[i].transform.position, Player.transform.position) < 5)
            {
                CurrentNote = Notes[i];
                break;
            }
            else
            {
                CurrentNote = null;
            }
        }
        Interact.interactable = (CurrentNote != null);
        
        if (Input.GetKeyDown(KeyCode.I) && Interact.interactable && !NoteImage.activeInHierarchy) 
        {
            Interact.onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Return) && NoteImage.activeInHierarchy)
        {
            NoteImage.transform.GetChild(1).GetComponent<Button>().onClick.Invoke();
        }

        
    }
    
    public void ReadLetter()
    {
        Player.GetComponent<FirstPersonController>().enabled = false;
        NoteImage.SetActive(true);
        Interact.gameObject.SetActive(false);
        string message = "";
        switch (CurrentNote.name)
        {
            case "Introduction":
                message = "Hey, \n\n If you are reading this, then there's something big that needs to be done. I've just been captured by the Autournal Unception Committee, but there's no time to explain. Near the water, there's something you will find. Hurry! \n\n - Anonymous";
                break;
            case "Hello":
                message = "Test";
                break;
        }
        NoteImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
        print("Button Called");
    }

    public void CloseLetter()
    {
        Interact.gameObject.SetActive(true);
        Player.GetComponent<FirstPersonController>().enabled = true;
        NoteImage.SetActive(false);
    }

}
