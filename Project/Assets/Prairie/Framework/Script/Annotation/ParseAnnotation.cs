using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class ParseAnnotation {

    private static readonly string IMAGE_TEXT_FULL = "<\\s*img\\s*src\\s*=\\s*\"?.*?\"?\\s*>";
    private static readonly string IMAGE_TEXT_SPLIT = "(<\\s*img\\s*src\\s*=\\s*\"?)|(\"?\\s*>)";

    /// <summary>
    /// Takes a string and parses text so that textOut gets populated by strings, separated by image paths that are stored in imgOut
    /// </summary>
    /// <param name="text"></param>
    /// <param name="textOut"></param>
    /// <param name="imgOut"></param>
    public static void ParseAnnotationText(string text, AnnotationContent content)
    {
        string[] splitText = Regex.Split(text, IMAGE_TEXT_FULL);
        content.parsedText.AddRange(splitText);

        MatchCollection imgCollection = Regex.Matches(text, IMAGE_TEXT_FULL);

        string replacement = "";
        Regex rgx = new Regex(IMAGE_TEXT_SPLIT);

        foreach (Match i in imgCollection)
        {
            string x = rgx.Replace(i.ToString(), replacement);
            content.imagePaths.Add(x);
        }
    }
}


