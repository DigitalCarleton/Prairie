using UnityEngine;
using UnityEditor;
using System.IO;


[CustomEditor(typeof(AnnotationInteraction))]
public class AnnotationInteractionEditor : Editor {

    bool showRichText = true;
    AnnotationInteraction annotation;


    public override void OnInspectorGUI()
    {
        annotation = (AnnotationInteraction)target;

        string[] options = new string[] { "Import Text File", "Write in Inspector" };
        annotation.selected = EditorGUILayout.Popup("Import Method", annotation.selected, options);

        //Processing if importing a text file
        if (annotation.selected == 0)
        {
            DisplayImportFile();

        } else //Process text written in inspector
        {
            DisplayWriteInInspector();
        }
    }

    /// <summary>
    /// Does the processing for displaying Inspector options when choosing to Import a file
    /// </summary>
    void DisplayImportFile()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Annotation File:");
        ImportFileButton();
        GUILayout.EndHorizontal();

        annotation.includeImages = EditorGUILayout.Toggle("Include Images:", annotation.includeImages);
        if (annotation.includeImages)
        {
            GUILayout.Label("Do the images share a directory with the text file?");
            annotation.sharedFile = EditorGUILayout.Toggle("Shared Directory:", annotation.sharedFile);

            if (!annotation.sharedFile)
            {
                GetImagePath();
            }
            else
            {
                annotation.imagePath = Path.GetDirectoryName(annotation.textFilePath) + Path.DirectorySeparatorChar;
            }
        }
    }

    /// <summary>
    /// Handles the process of importing the file, including the file explorer, and loading the textasset
    /// </summary>
    void ImportFileButton()
    {
        string prompt = "Select File";
        if (annotation.textFilePath != "")
        {
            prompt = annotation.textFilePath;
        }
        if (GUILayout.Button(prompt, GUILayout.ExpandWidth(false)))
        {
            string absolutePath = EditorUtility.OpenFilePanel("Select File to Import", "Assets", "txt");

            if (absolutePath.StartsWith(Application.dataPath))
            {
                annotation.textFilePath = "Assets" + absolutePath.Substring(Application.dataPath.Length);
                annotation.textFile = AssetDatabase.LoadAssetAtPath<TextAsset>(annotation.textFilePath);
                if (annotation.textFile != null)
                {
                    annotation.text = annotation.textFile.text; //show text from textfile in inspector editor
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Can't Load Asset", "The file must be stored as part of your Unity project's assets.", "OK");
            }
        }
    }

    /// <summary>
    /// Does the processing for displaying inspector options when writing in the editor
    /// </summary>
    void DisplayWriteInInspector()
    {
        GUILayout.Label("Annotation Text:");

        showRichText = EditorGUILayout.Toggle("Richtext in Editor:", showRichText);
        GUIStyle textAreaStyle = GUI.skin.textArea;
        textAreaStyle.wordWrap = true;
        if (showRichText)
        {
            textAreaStyle.richText = true;
        }
        else
        {
            textAreaStyle.richText = false;
        }

        annotation.text = EditorGUILayout.TextArea(annotation.text, textAreaStyle, GUILayout.Height(100),
            GUILayout.Width(EditorGUIUtility.currentViewWidth - 40), GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(false));

        annotation.includeImages = EditorGUILayout.Toggle("Include Images:", annotation.includeImages);

        if (annotation.includeImages)
        {
            GetImagePath();
        }
    }

    /// <summary>
    /// Displays the button and opens the file explorer when the user chooses a specific image folder
    /// </summary>
    void GetImagePath()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Images Folder:");
        string imagePrompt = "Select File";
        if (annotation.imagePath != "")
        {
            imagePrompt = annotation.imagePath;
        }

        if (GUILayout.Button(imagePrompt, GUILayout.ExpandWidth(false)))
        {
            string absolutePath = EditorUtility.OpenFolderPanel("Select Image Folder", "Assets", "");

            if (absolutePath.StartsWith(Application.dataPath))
            {
                annotation.imagePath = "Assets" + absolutePath.Substring(Application.dataPath.Length) + Path.DirectorySeparatorChar;
            }
            else
            {
                EditorUtility.DisplayDialog("Can't Load Asset", "The file must be stored as part of your Unity project's assets.", "OK");
            }
        }
        GUILayout.EndHorizontal();
    }
}
