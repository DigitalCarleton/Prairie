using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;


public class AnnotationContent
{
    public List<string> parsedText;
    public List<string> imagePaths;

    public AnnotationContent()
    {
        parsedText = new List<string>();
        imagePaths = new List<string>();
    }

}


public class AnnotationInteraction : Interaction
{
    public AnnotationContent content;

    public string textFilePath; //saved for use in the editor
    public int selected = 0; //uses int for editor purposes
    public bool includeImages = false;
    public bool sharedFile = true; //whether images and text are in the same file
    public string imagePath;

    public TextAsset textFile;
    public string text;
    public List<Texture2D> images;

    private bool active = false;

    private GUIStyle style;
    private Rect rectangle;
    private Vector2 scrollPosition;

    private readonly float BOX_X = Screen.width / 4;
    private readonly float BOX_Y = 10;
    private readonly float BOX_WIDTH = Screen.width / 2;
    private readonly float BOX_HEIGHT = Screen.height - 20;

    void Start()
    {
        content = new AnnotationContent();

        scrollPosition = new Vector2(0, 0);

        rectangle = new Rect(BOX_X, BOX_Y, BOX_WIDTH, BOX_HEIGHT);
        //setting up style for text
        style = new GUIStyle();
        style.wordWrap = true;
        style.richText = true;
        style.normal.textColor = Color.white;
        style.padding.bottom = 15;
        style.padding.top = 15;

        images = new List<Texture2D>();

        //this changes parsedText and imgPaths
        ParseAnnotation.ParseAnnotationText(text, content);
        
        if (includeImages)
        {
            foreach (string path in content.imagePaths)
            {
                //grabbing byte data from file so we can convert it into a texture
                try {
                    byte[] fileData = File.ReadAllBytes(imagePath + path);
                    Texture2D img = new Texture2D(2, 2);
                    img.LoadImage(fileData);
                    images.Add(img);
                } catch (Exception e)
                {
                    Debug.Log(e);
                    images.Add(null);
                }
            }
        } 
    }

    protected override void PerformAction()
    {
        active = true;
		this.SetPlayerIsFrozen (true);
    }

    void OnGUI()
    {
        if (active)
        {
            //Allow the player to see and move the cursor (so they can scroll)
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            //hacky way to increase the opacity of the background
            //you would think this would be simple
            //it isn't simple
            GUI.Box(rectangle, Texture2D.blackTexture);
            GUI.Box(rectangle, Texture2D.blackTexture);
            GUI.Box(rectangle, Texture2D.blackTexture);

            GUI.BeginGroup(new Rect(BOX_X + 10, BOX_Y, BOX_WIDTH, BOX_HEIGHT));

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(BOX_WIDTH - 10),
                GUILayout.Height(BOX_HEIGHT));

            DisplayAnnotation();

            GUILayout.EndScrollView();
            GUI.EndGroup();
        }
    }

    /// <summary>
    /// Displays then annotation text and images
    /// </summary>
    private void DisplayAnnotation()
    {
        for (int i = 0; i < Math.Max(images.Count, content.parsedText.Count); i++)
        {
            if (i < content.parsedText.Count && content.parsedText[i] != "")
            {
                GUILayout.Label(new GUIContent(content.parsedText[i]), style, GUILayout.MaxWidth(BOX_WIDTH - 40));
            }
            if (i < images.Count)
            {
                DisplayImage(images[i]);
            }
        }
    }

    /// <summary>
    /// Formats and displays a texture
    /// </summary>
    /// <param name="tex">Texture to display</param>
    void DisplayImage(Texture tex)
    {
        if (tex != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (tex.width > BOX_WIDTH - 40)
            {
                //resize image if it is wider than the scrollbox
                float newHeight = ((BOX_WIDTH - 40) / tex.width) * ((float)tex.height);
                GUILayout.Label(new GUIContent(tex), GUILayout.Width(BOX_WIDTH - 40), GUILayout.Height(newHeight));
            }
            else
            {
                GUILayout.Label(new GUIContent(tex));
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }


    void Update()
    {
        if (active)
        {
            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.Escape))
            {
                active = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                
				this.SetPlayerIsFrozen (false);
            }
        }
    }
}
