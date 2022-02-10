using UnityEngine;

public class InputfieldSlideScreen : MonoBehaviour {

	// Assign canvas here in editor
	public Canvas canvas;


	// Used internally - set by InputfieldFocused.cs
	public bool InputFieldActive = false;
	public RectTransform childRectTransform;


	void LateUpdate () {
		
		if ((InputFieldActive)&&((TouchScreenKeyboard.visible)))
		{
			transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

			Rect rect = RectTransformExtension.GetScreenRect(childRectTransform, canvas);
			float keyboardHeight = (float)GetKeyboardHeight();
			float heightPercentOfKeyboard = keyboardHeight/Screen.height*100f;
			float heightPercentOfInput = (Screen.height-(rect.y+rect.height))/Screen.height*100f;


			if (heightPercentOfKeyboard>heightPercentOfInput)
			{
				// keyboard covers input field so move screen up to show keyboard
				float differenceHeightPercent = heightPercentOfKeyboard - heightPercentOfInput;
				float newYPos = transform.GetComponent<RectTransform>().rect.height /100f*differenceHeightPercent;

				Vector2 newAnchorPosition = Vector2.zero;
				#if UNITY_ANDROID
				newAnchorPosition.y = newYPos+40;
				#elif UNITY_IOS
				newAnchorPosition.y = newYPos;
				#endif

				transform.GetComponent<RectTransform>().anchoredPosition = newAnchorPosition;
			} else {
				// Keyboard top is below the position of the input field, so leave screen anchored at zero
				transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			}
		} else {
			// No focus or touchscreen invisible, set screen anchor to zero
			transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		}
		//InputFieldActive = false;
	}

	public int GetKeyboardHeight () {

		#if UNITY_ANDROID
		using(AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject View = UnityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");

			using(AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect"))
			{
				View.Call("getWindowVisibleDisplayFrame", Rct);

				return Screen.height - Rct.Call<int>("height");
			}
		}
		#elif UNITY_IOS
		return (int)TouchScreenKeyboard.area.height;
		#else
		return 0;
		#endif
	}

}

public static class RectTransformExtension {

	public static Rect GetScreenRect(this RectTransform rectTransform, Canvas canvas) {

		Vector3[] corners = new Vector3[4];
		Vector3[] screenCorners = new Vector3[2];

		rectTransform.GetWorldCorners(corners);

		if (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace)
		{
			screenCorners[0] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners[1]);
			screenCorners[1] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners[3]);
		}
		else
		{
			screenCorners[0] = RectTransformUtility.WorldToScreenPoint(null, corners[1]);
			screenCorners[1] = RectTransformUtility.WorldToScreenPoint(null, corners[3]);
		}

		screenCorners[0].y = Screen.height - screenCorners[0].y;
		screenCorners[1].y = Screen.height - screenCorners[1].y;

		return new Rect(screenCorners[0], screenCorners[1] - screenCorners[0]);
	}
}