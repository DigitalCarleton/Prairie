using UnityEngine;
using UnityEditor;
using System.IO;
using System;

[CustomEditor(typeof(Annotation))]
public class AnnotationInteractionEditor : Editor {

    bool showRichText = true;
    Annotation annotation;


    public override void OnInspectorGUI()
    {
        annotation = (Annotation)target;


        string[] typeOptions = new string[] { "Summary Annotation", "Area Annotation" };
        annotation.annotationType= EditorGUILayout.Popup("Annotation Type", annotation.annotationType, typeOptions);

        if(annotation.annotationType == (int)AnnotationTypes.SUMMARY)
        {
            DisplaySetSummary();
        }
        else
        {
            ///Stuff for Area Annotations here
        }

        int previousType = annotation.importType;

        string[] importOptions = new string[] { "No Full Annotation", "Import Text File", "Write in Inspector" };
        annotation.importType = EditorGUILayout.Popup("Import Method", annotation.importType, importOptions);

        //checks with user, then resets full annotation information
        if (previousType != 0 && annotation.importType == (int)ImportTypes.NONE)
        {
            if (EditorUtility.DisplayDialog("Reset", "WARNING: Switching back to 'No Full Annotation' will cause any curent information to be lost...",
                "Continue", "Cancel"))
            {
                annotation.imagePath = "";
                annotation.text = "";
                annotation.textFilePath = "";
                annotation.textFile = null;
            }
            else
            {
                annotation.importType = previousType;
            }
        }

        //Processing if importing a text file
        if (annotation.importType == (int)ImportTypes.IMPORT)
        {
            DisplayImportFile();
        }
        else if (annotation.importType == (int)ImportTypes.INSPECTOR) //Process text written in inspector
        {
            DisplayWriteInInspector();
        }
    }

    void DisplaySetSummary()
    {
        GUIStyle textAreaStyle = new GUIStyle(GUI.skin.textArea);
        textAreaStyle.wordWrap = true;
        GUILayout.Label("Annotation Summary Text:");
        annotation.summary = GUILayout.TextArea(annotation.summary, 250, textAreaStyle, GUILayout.Height(75),
            GUILayout.Width(EditorGUIUtility.currentViewWidth - 40), GUILayout.ExpandWidth(false));
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
        if (!string.IsNullOrEmpty(annotation.textFilePath.Trim()))
        {
            prompt = annotation.textFilePath;
        }
        if (GUILayout.Button(prompt, GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth / 2), GUILayout.ExpandWidth(false)))
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

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        SaveTextFileButton();
        GUILayout.EndHorizontal();

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
        if (!string.IsNullOrEmpty(annotation.imagePath.Trim()))
        {
            imagePrompt = annotation.imagePath;
        }

        if (GUILayout.Button(imagePrompt, GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth / 2), GUILayout.ExpandWidth(false)))
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


    /// <summary>
    /// Allows the user to save any edits made to the full annotation to a text file
    /// </summary>
    void SaveTextFileButton()
    {
        if (GUILayout.Button("Save to text file", GUILayout.ExpandWidth(false)))
        {
            string path = EditorUtility.SaveFilePanel("Save File As", "", "", "txt");
            if (path.Length != 0)
            {
                if (path.StartsWith(Application.dataPath))
                {
                    try
                    {
                        File.WriteAllText(path, annotation.text);

                        //Links annotation file to newly saved file
                        annotation.textFilePath = "Assets" + path.Substring(Application.dataPath.Length);
                        annotation.textFile = AssetDatabase.LoadAssetAtPath<TextAsset>(annotation.textFilePath);

                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                        EditorUtility.DisplayDialog("Error", "Failed to save file.", "OK");
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("Save to Assets Directory", "Please save the file within the Assets directory of the Unity Project", "OK");
                }
                    
            }
        }
    }
}
