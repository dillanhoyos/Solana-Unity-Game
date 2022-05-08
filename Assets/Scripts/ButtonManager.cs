using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using NativeCameraNamespace;
using TMPro;
using System.Net.Http;
using System.IO;

public enum AppState { Home, Reward };    


public class ButtonManager : MonoBehaviour
{
    public int count = 0;
    
    // background images
    public Image Background;

    public Sprite HomeImage;
    public Sprite RewardImageBad;
    public Sprite RewardImageGood;

    // photo will be rendered here
    public RawImage rawImage;

    //GameObject quad;
    Texture2D texture;
    byte[] byteArray;

    public void TakePicture( int maxSize )
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture( ( path ) =>
        {
            Debug.Log( "Image path: " + path );
            if( path == null )
            {
                Debug.Log( "path is null, returning.. ");
                return;
            }
            // Create a Texture2D from the captured image
            texture = NativeCamera.LoadImageAtPath( path, maxSize, false );
            Debug.Log($"Texture created from camera! texture.height = {texture.height} and texture.width = {texture.width}");

            //byte[] rawByteArray = File.ReadAllBytes(path);

            byteArray = texture.EncodeToJPG();

            //Debug.Log($"rawByteArray={rawByteArray.Length}");
            Debug.Log($"byteArray={byteArray.Length}");


            // if( texture == null )
            // {
            //     Debug.Log( "texture is null, returning.. ");
            //     return;
            // }

            // // Assign texture to a temporary quad and destroy it after 5 seconds
            // if(quad == null) {
            //     quad = GameObject.CreatePrimitive( PrimitiveType.Quad );
            // }
            // // hacky...

            // quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
            // quad.transform.forward = Camera.main.transform.forward;
            
            // //float imageRatio = 512 / (float) 384;
            // quad.transform.localScale = new Vector3( 1f, texture.height / (float) texture.width, 1f );
            
            // Material material = quad.GetComponent<Renderer>().material;
            // if( !material.shader.isSupported ) // happens when Standard shader is not included in the build
            // {
            //     //material.shader = Shader.Find( "Legacy Shaders/Diffuse" );
            //     material.shader = Shader.Find( "Sprites/Default" );
            // }
            // material.mainTexture = texture;

            // try using a rawImage
            rawImage.texture = texture;
        
            // get the food info via image recognition
            PostImage();
            
    	}, maxSize );

        
	    Debug.Log( "Permission result: " + permission );
    }

    [ContextMenu("Test Get")]
    public async void GetUsers()
    {
        // our admin token (temp; tied to Arun's free account at logmeal.es)
        var baseUrl = "https://api.logmeal.es";
        
        //var appToken = "bdaf59031c9b25a32baa11e7d119870f300d851f"; // Arun's account
        var appToken = "843fa5dd52d72b3fed8e3c2236c8f232ac1ce881"; // Dillan's account
        WebApiHandler webHandler = new WebApiHandler(baseUrl, appToken);
        HttpResponseMessage webResponse = webHandler.Get("/v2/users/APIUsers");
        if(webResponse.IsSuccessStatusCode) {
            Debug.Log($"{webResponse.Content.ReadAsStringAsync().Result}");
            
        }
        else {
            Debug.Log($"{webResponse.IsSuccessStatusCode}");
        }
    }

    [ContextMenu("Test Post")]
    public async void PostImage()
    {
        if(texture == null)
        {
            Debug.Log("texture is null, returning early");
            return;
        }

        // user token (hardcoded for now; could be fetched from 'GetUsers')
        var baseUrl = "https://api.logmeal.es";

        //var userToken = "6a080a24590aa86bbbcfac6efd04b43d8d446e0c"; // Arun's account / Arun's token
        var userToken = "aa9298f9f55f66d82619641c61102705c787da72"; // Dillan's account / Dillan's token

        WebApiHandler webHandler = new WebApiHandler(baseUrl, userToken);
        
        //HttpResponseMessage webResponse = webHandler.Post("/v2/image/recognition/type/v1.0?language=eng", byteArray);
        HttpResponseMessage webResponse = webHandler.Post("/v2/image/recognition/complete/v1.0?skip_types=[2,3,4,5]&language=eng", byteArray);

        string foodDetails = "";
        if(webResponse.IsSuccessStatusCode) {
            foodDetails = webResponse.Content.ReadAsStringAsync().Result;
            Debug.Log($"Food found! {foodDetails}");      
        }
        else {
            Debug.Log($"Http request unsuccessful: {webResponse.Content.ReadAsStringAsync().Result}");            
        }
                
        ShowRewardView(foodDetails);
    }

    void ShowHomeView()
    {
        Debug.Log("Showing home view");
        Background.sprite = HomeImage;
        rawImage.enabled = false;
    }

    void ShowRewardView(string foodDetails)
    {
        string s = "";
        if(foodDetails.Contains("banana")) // broken :( not enough time to change APIs need to hard code the result screen
            s = "(has banana!)";

        Debug.Log("Showing reward view " + s);
        Background.sprite = RewardImageBad;
        rawImage.enabled = true;

        count++; // 1, 2, 3 etc.

        Debug.Log($"count = {count}");        
        // every 3rd try, show the good result (and transfer tokens)
        if(count % 3 == 0)
        {
            Background.sprite = RewardImageGood;
            SendRewards();
        }

    }

    void SendRewards()
    {

    }


}