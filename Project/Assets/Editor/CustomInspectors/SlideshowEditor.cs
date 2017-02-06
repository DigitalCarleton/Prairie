using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Slideshow))]
public class SlideshowEditor : Editor {

	Slideshow slideshow;

	public override void OnInspectorGUI()
	{
		slideshow = (Slideshow)target;
		slideshow.Slides = PrairieGUI.drawObjectList ("Slides", slideshow.Slides);

		for (int i = 0; i < slideshow.Slides.Length; i++) 
		{
			if (slideshow.Slides [i] == null) 
			{
				DrawWarnings ();
				break;
			}
		}
	}

	public void DrawWarnings()
	{
		PrairieGUI.warningLabel ("One or more of the slides in your slideshow is empty.  Please fill the empty slides or remove them.");
	}
}
