using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Slideshow))]
public class SlideshowInspector : Editor {

	Slideshow slideshow;

	public override void OnInspectorGUI()
	{
		slideshow = (Slideshow)target;
		slideshow.Slides = PrairieGUI.drawObjectList ("Slides", slideshow.Slides);
	}
}
