using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using NativeCameraNamespace;
using TMPro;
using WEDX.Services;

public class PhotoButtonLogic : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;

    public void TakePicture( int maxSize )
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture( ( path ) =>
        {
            Debug.Log( "Image path: " + path );
            if( path != null )
            {
                // Create a Texture2D from the captured image
                Texture2D texture = NativeCamera.LoadImageAtPath( path, maxSize );
                if( texture == null )
                {
                    Debug.Log( "Couldn't load texture from " + path );
                    return;
                }

                // Assign texture to a temporary quad and destroy it after 5 seconds
                GameObject quad = GameObject.CreatePrimitive( PrimitiveType.Quad );
                quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
                quad.transform.forward = Camera.main.transform.forward;
                quad.transform.localScale = new Vector3( 1f, texture.height / (float) texture.width, 1f );
                
                Material material = quad.GetComponent<Renderer>().material;
                if( !material.shader.isSupported ) // happens when Standard shader is not included in the build
                {
                    //material.shader = Shader.Find( "Legacy Shaders/Diffuse" );                
                    material.shader = Shader.Find( "Sprites/Default" );
                }   

                material.mainTexture = texture;
                    
                Destroy( quad, 5f );

                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
                Destroy( texture, 5f );
            }
        }, maxSize );

        Debug.Log( "Permission result: " + permission );
    }

    [ContextMenu("Test Get")]
    public async void TestGet()
    {
        //var url = "https://jsonplaceholder.typicode.com/todos/1";
        var url = "https://api.logmeal.es/v2/users/APIUsers";

        var appToken = "bdaf59031c9b25a32baa11e7d119870f300d851f";


        using var www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Token", "Bearer " + appToken);

        var operation = www.SendWebRequest();

        while(!operation.isDone)
            await Task.Yield();

        if(www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Success {www.downloadHandler.text}");
            //Debug.Log($"Success: " {www.downloadHandler.text});
        }
        else
        {
            Debug.Log($"Failure: {www.error}");
        }
    }
}