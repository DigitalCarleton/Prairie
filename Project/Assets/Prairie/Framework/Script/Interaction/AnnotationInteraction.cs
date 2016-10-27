using UnityEngine;
using System.Collections;

public class AnnotationInteraction : Interaction
{
    
    public bool UseTextFile = false;
    public TextAsset TextFile;
    [Multiline]
    public string Text;
    public Texture[] Images;

    private bool Active = false;
    private GUIContent Content;
    private GUIStyle Style;
    private Rect Rectangle;
    private Vector2 ScrollPosition;

    private readonly float BOX_X = Screen.width / 4;
    private readonly float BOX_Y = 10;
    private readonly float BOX_WIDTH = Screen.width / 2;
    private readonly float BOX_HEIGHT = Screen.height - 20;

    void Start()
    {
        if (UseTextFile == true && TextFile != null)
        {
            Content = new GUIContent(TextFile.text);
        } else
        {
            Content = new GUIContent(Text);
        }
        
        ScrollPosition = new Vector2(0, 0);
        Rectangle = new Rect(BOX_X, BOX_Y, BOX_WIDTH, BOX_HEIGHT);
        //setting up style for text
        Style = new GUIStyle();
        Style.wordWrap = true;
        Style.richText = true;
        Style.normal.textColor = Color.white;
        Style.padding.bottom = 15;
        Style.padding.top = 15;
    }

    protected override void PerformAction()
    {
        Active = true;
		this.SetPlayerIsFrozen (true);
    }

    void OnGUI()
    {
        if (Active)
        {
            //Allow the player to see and move the cursor (so they can scroll)
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            //hacky way to increase the opacity of the background
            //you would think this would be simple
            //it isn't simple
            GUI.Box(Rectangle, Texture2D.blackTexture);
            GUI.Box(Rectangle, Texture2D.blackTexture);
            GUI.Box(Rectangle, Texture2D.blackTexture);

            GUI.BeginGroup(new Rect(BOX_X + 10, BOX_Y, BOX_WIDTH, BOX_HEIGHT));
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, GUILayout.Width(BOX_WIDTH - 10),
                GUILayout.Height(BOX_HEIGHT));

            if (Images.Length > 0)
            {
                for (int i = 0; i < Images.Length; i++)
                {
                    Texture image = Images[i];

                    if (image.width > BOX_WIDTH - 40)
                    {
                        //resize image if it is wider than the scrollbox
                        float newHeight = ((BOX_WIDTH - 40) / image.width) * ((float)image.height);
                        GUILayout.Label(new GUIContent(image), GUILayout.Width(BOX_WIDTH - 40), GUILayout.Height(newHeight));
                    } else
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(new GUIContent(image));
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();    
                    }

                }
            }

            GUILayout.Label(Content, Style, GUILayout.MaxWidth(BOX_WIDTH - 40));
            GUILayout.EndScrollView();
            GUI.EndGroup();
        }
    }


    void Update()
    {
        if (Active)
        {
            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.Escape))
            {
                Active = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                
				this.SetPlayerIsFrozen (false);
            }
        }
    }
}
