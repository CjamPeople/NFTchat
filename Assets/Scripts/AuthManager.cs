using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using TMPro;

public class AuthManager : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField WalletAddress;

    FirebaseAuth auth;

    public string uname;
    public string wallet;
    
    public class Data
    {
        public string uname;
        public string wallet;

        public Data(string uname, string wallet)
        {
            this.uname = uname;
            this.wallet = wallet;
        }
    }

    private DatabaseReference databaseReference;

    void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void Signup()
    {
        uname = username.text.Trim();
        wallet = WalletAddress.text.Trim();
        
        auth.CreateUserWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).ContinueWith(
            task =>
            {
                if(!task.IsCanceled && !task.IsFaulted)
                {
                    Debug.Log("회원가입 성공");
                }
                else
                {
                    Debug.Log("회원가입 실패");
                }
            }
        );
        
        var data = new Data(uname, wallet);
        string jsonData = JsonUtility.ToJson(data);

        databaseReference.Child(emailInput.text).SetRawJsonValueAsync(jsonData);
        
    }

    public void Login()
    {
        auth.SignInWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).ContinueWith(
            task =>
            {
                if(task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
                {
                    Debug.Log("로그인 성공");
                }
                else
                {
                    Debug.Log("로그인 실패");
                }
            }
        );
    }
}
