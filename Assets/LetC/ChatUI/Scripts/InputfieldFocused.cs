using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputfieldFocused : MonoBehaviour {

	public InputfieldSlideScreen slideScreen;
	public InputField inputField;


	void Start () {
//		slideScreen = GameObject.Find ("MainContainer").GetComponent<InputfieldSlideScreen>();
		slideScreen = gameObject.GetComponentInParent<InputfieldSlideScreen>();
		inputField = transform.GetComponent<InputField>();
		//#if UNITY_IOS
		inputField.shouldHideMobileInput=true;
		//#endif
	}

	//#if UNITY_IOS

	void Update () {
		
//		if (inputField.isFocused)
//		{
			// Input field focused, let the slide screen script know about it.
//			slideScreen.InputFieldActive = true;
//			slideScreen.childRectTransform = transform.GetComponent<RectTransform>();

			//slideScreen.StartCoroutine(slideScreen.KeyboardOperation());
//		}
	}

	public void OnInputFieldActive () {
		slideScreen.InputFieldActive = true;
		slideScreen.childRectTransform = transform.GetComponent<RectTransform>();
	}

	public void OnInputFieldDeActive () {
		inputField.DeactivateInputField();
		slideScreen.InputFieldActive = false;
	}
	//#endif
}
