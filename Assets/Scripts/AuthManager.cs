using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using TMPro;

public class AuthManager : MonoBehaviour
{
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_InputField passwordInput;

    Firebase.Auth.FirebaseAuth auth;

    void Awake()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    public void Signup()
    {
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
