using UnityEngine;

public static class Util
{
    public static Sprite CreateSpriteFromBytes(byte[] bytes)
    {
        //byteからTexture2D作成。画像サイズは気にしなくてもいい
        Texture2D texture = new Texture2D(1024, 1024);
        
        if (!texture.LoadImage(bytes)) {Debug.Log("Failed to load image");}

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
    
    public static Sprite CreateSpriteFromRawBytes(byte[] bytes)
    {
        //rawbyteからTexture2D作成。画像サイズは気にしなくてもいい
        Texture2D texture = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);

        texture.LoadRawTextureData(bytes);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
}