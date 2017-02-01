using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Slideshow))]
public class SlideshowEditor : Editor {

	Slideshow slideshow;

	public override void OnInspectorGUI()
	{
		slideshow = (Slideshow)target;
		// Enable add/removal of slides TODO: method to switch around order of slides?
		slideshow.Slides = PrairieGUI.drawObjectList ("Slides", slideshow.Slides);
	}
}
