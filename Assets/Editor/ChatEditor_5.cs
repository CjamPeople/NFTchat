using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChatManager_5))]
public class ChatEditor_5 : Editor
{
    ChatManager_5 chatManager;
    string text;

    void OnEnable()
    {
        chatManager = target as ChatManager_5;
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        text = EditorGUILayout.TextArea(text);

        if(GUILayout.Button("send",GUILayout.Width(60)) && text.Trim() != "")
        {
            chatManager.Chat(true, text, "me", null);
            text = "";
            GUI.FocusControl(null);
        }

        if(GUILayout.Button("receieve",GUILayout.Width(60)) && text.Trim() != "")
        {
            chatManager.Chat(false, text, "other", null);
            text = "";
            GUI.FocusControl(null);
        }

        EditorGUILayout.EndHorizontal();
    }
}
