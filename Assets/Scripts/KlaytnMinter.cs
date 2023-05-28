using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro; 

public class KlaytnMinter : MonoBehaviour
{   
    public string serverUrl = "http://localhost:5000";  

    public TMP_InputField fromAddressInputField;
    public TMP_InputField privateKeyInputField;
    public TMP_InputField toAddressInputField;
    
    public LoadGallery loadGalleryScript; 
    
    public void OnMintButtonClicked()
    {
        string fromAddress = fromAddressInputField.text;
        string privateKey = privateKeyInputField.text;
        string toAddress = toAddressInputField.text;

        StartCoroutine(WaitForImageLoadAndMint(fromAddress, privateKey, toAddress));
    }

    private IEnumerator WaitForImageLoadAndMint(string fromAddress, string privateKey, string toAddress)
    {
        loadGalleryScript.OnClickImageLoad();

        while (!loadGalleryScript.IsImageLoaded)
        {
            yield return null;
        }

        string tokenURI = loadGalleryScript.img.texture.name;

        StartCoroutine(MintAndAirdrop(fromAddress, privateKey, toAddress, tokenURI));
    }

    public IEnumerator MintAndAirdrop(string fromAddress, string privateKey, string toAddress, string tokenURI)
    {
        var mintWwwForm = new WWWForm();
        mintWwwForm.AddField("from_address", fromAddress);
        mintWwwForm.AddField("private_key", privateKey);
        mintWwwForm.AddField("tokenURI", tokenURI);

        using (UnityWebRequest mintWww = UnityWebRequest.Post(serverUrl + "/mint", mintWwwForm))
        {
            yield return mintWww.SendWebRequest();

            if (mintWww.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(mintWww.error);
            }
            else
            {
                var response = mintWww.downloadHandler.text;
                var jsonData = JsonUtility.FromJson<NFTMintResponse>(response);

                Debug.Log("Mint successful! Transaction hash: " + jsonData.tx_hash);
                Debug.Log("Token ID: " + jsonData.token_id);
                Debug.Log("Owner: " + jsonData.owner);

                var airdropWwwForm = new WWWForm();
                airdropWwwForm.AddField("from_address", fromAddress);
                airdropWwwForm.AddField("private_key", privateKey);
                airdropWwwForm.AddField("to_address", toAddress);
                airdropWwwForm.AddField("token_id", jsonData.token_id);

                using (UnityWebRequest airdropWww = UnityWebRequest.Post(serverUrl + "/airdrop", airdropWwwForm))
                {
                    yield return airdropWww.SendWebRequest();

                    if (airdropWww.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log(airdropWww.error);
                    }
                    else
                    {
                        Debug.Log("Airdrop successful! Transaction hash: " + airdropWww.downloadHandler.text);
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class NFTMintResponse
    {
        public string token_id;
        public string tx_hash;
        public string owner;
    }
}