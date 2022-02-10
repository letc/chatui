using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChatManager : MonoBehaviour {

	public Transform content;

	public GameObject chatBarPrefab;

	public List<string> chatData = new List<string>();

	public Sprite user1Sprite;
	public Sprite user2Sprite;

	public Color user1ImageColor;
	public Color user2ImageColor;

	public Sprite user1ChatBarSprite;
	public Sprite user2ChatBarSprite;

	public Color textColor;
	public int fontSize;

	private VerticalLayoutGroup verticalLayoutGroup;

	public InputField inputField;

	// ratio = heightinoriginalscreenheight/originalscreenheight
	// Use this for initialization
	void Start () {
		string[] chats = new string[]{
			"Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
			"Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
			"It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.",
			"It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
			"Where does it come from?",
			"Contrary to popular belief, Lorem Ipsum is not simply random text.",
			"It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source."};
		for(int i = 0; i < 10; i++) {

			if(Random.Range(0,2) == 0)
				chatData.Add(chats[Random.Range(0,chats.Length)]+"~0");
			else
				chatData.Add(chats[Random.Range(0,chats.Length)]+"~1");
		}

		ShowUserMsg ();
		verticalLayoutGroup = content.GetComponent<VerticalLayoutGroup>();
	
	}


	public void ShowUserMsg () {
		
		for(int i = 0; i < chatData.Count; i++) {
			StartCoroutine(ShowUserMsgCoroutine (chatData[i]));
		}
	}

	public void SendMsgFromInputfield()
    {
        if (inputField && !string.IsNullOrWhiteSpace(inputField.text))
        {
			chatData.Clear();
			chatData.Add (inputField.text + "~0");
			StartCoroutine(ShowUserMsgCoroutine(chatData[0]));
			inputField.text = "";
		}
    }


	private string lastUser;
	IEnumerator ShowUserMsgCoroutine (string msg) {

		GameObject chatObj = Instantiate(chatBarPrefab,Vector3.zero, Quaternion.identity) as GameObject;
		chatObj.transform.SetParent(content.transform,false);

		chatObj.SetActive(true);

		ChatListObject clb = chatObj.GetComponent<ChatListObject>();

		string[] split  = msg.Split('~');
		msg = split[0];
		/*
		*0.03f = 16/480  (16 is the original font size in 320/480 resolution screen so to increase font size automatically with increase resolution screen we should multiply 0.03 with screen height)
		*this not required when canvas scale mode is set to scale with screen size.
		*/
		fontSize = (int)(Screen.height*0.03f);

		clb.parentText.fontSize = fontSize;
		clb.childText.fontSize = fontSize;
			
		clb.parentText.text = msg;

		clb.childText.color = Color.black;

		yield return new WaitForEndOfFrame();

		float height = chatObj.GetComponent<RectTransform>().rect.height;
		float width = chatObj.GetComponent<RectTransform>().rect.width;

		clb.chatbarImage.rectTransform.sizeDelta = new Vector2(width+5,height+6);
		clb.childText.rectTransform.sizeDelta = new Vector2(width,height);


		clb.childText.text = msg;

		if (split[1] == "0") {

			if(lastUser == "1") {
				clb.userImage.enabled = true;
			} else if(lastUser == "0"){
				clb.userImage.enabled = false;
			}

			clb.chatbarImage.color = new Color(user1ImageColor.r,user1ImageColor.g,user1ImageColor.b,1);
			clb.userImage.sprite = user1Sprite;

			clb.chatbarImage.sprite = user1ChatBarSprite;

			clb.userImage.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-26f,clb.userImage.transform.parent.GetComponent<RectTransform>().anchoredPosition.y);
			clb.chatbarImage.rectTransform.anchoredPosition = new Vector2(-3f,clb.chatbarImage.rectTransform.anchoredPosition.y);

			lastUser = "0";

		} else if (split[1] == "1") {

			if(lastUser == "0") {
				clb.userImage.enabled = true;
			} else if(lastUser == "1"){
				clb.userImage.enabled = false;
			}

			clb.chatbarImage.color = new Color(user2ImageColor.r,user2ImageColor.g,user2ImageColor.b,1);
			clb.userImage.sprite = user2Sprite;

			clb.chatbarImage.sprite = user2ChatBarSprite;

			clb.chatbarImage.rectTransform.anchoredPosition = new Vector2(((content.GetComponent<RectTransform>().rect.width-(verticalLayoutGroup.padding.left+verticalLayoutGroup.padding.right))-chatObj.GetComponent<RectTransform>().rect.width),clb.chatbarImage.rectTransform.anchoredPosition.y);
			clb.userImage.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(clb.chatbarImage.rectTransform.anchoredPosition.x+clb.parentText.rectTransform.rect.width+27,clb.userImage.transform.parent.GetComponent<RectTransform>().anchoredPosition.y);
			lastUser = "1";

		}

		content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, content.GetComponent<RectTransform>().sizeDelta.y);
	}
}
