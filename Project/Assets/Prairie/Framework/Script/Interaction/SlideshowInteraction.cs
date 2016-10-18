using UnityEngine;
using System.Collections;

public class SlideshowInteraction : Interaction
{
    public Texture2D[] slides = new Texture2D[7];
    private int currentSlide;
    private bool active = false;

    private GUIContent content;
    private GUIStyle style;

    void Start()
    {
        // Insert pictures into GUI
        content = new GUIContent(slides[currentSlide]);

        style = new GUIStyle();
        //style.normal.background = Texture2D.blackTexture;
        style.normal.background.height = Screen.height;
        style.normal.background.width = Screen.width;
    }

    protected override void PerformAction()
    {
        currentSlide = 0;
        active = true;
        // Disable player movement/interactor upon interacting
        GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().enabled = false;
        GameObject.Find("Player").GetComponent<FirstPersonInteractor>().enabled = false;
    }

    void OnGUI()
    {
        if (active)
        {
            // "Blank" camera and display image only
            GameObject.Find("MainCamera").GetComponent<Camera>().enabled = false;
            GameObject.Find("Camera").GetComponent<Camera>().enabled = true;

            GUI.Box(new Rect(Screen.width / 2 - content.image.width / 2,
            Screen.height / 2 - content.image.height / 2, content.image.width,
            content.image.height), content, style);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            // Advance w/ --> or D; loop back to start when on last image
            if (currentSlide < slides.Length - 1)
            {
                currentSlide++;
                content.image = slides[currentSlide];
            } else
            {
                currentSlide = 0;
                content.image = slides[currentSlide];
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            // Retreat w/ <-- or A; loop back to last image when on first
            if (currentSlide > 0)
            {
                currentSlide--;
                content.image = slides[currentSlide];
            } else
            {
                currentSlide = slides.Length - 1;
                content.image = slides[currentSlide];
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Upon ESC, exit from GUI and reenable player control
            active = false;
            GameObject.Find("Camera").GetComponent<Camera>().enabled = false;
            GameObject.Find("MainCamera").GetComponent<Camera>().enabled = true;
            GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().enabled = true;
            GameObject.Find("Player").GetComponent<FirstPersonInteractor>().enabled = true;
        }
    }
}
