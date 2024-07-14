using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace public_func
{


    public static class Public_Functions
{
        public static Sprite LoadSpriteFromResources(string path)
    {
    // Note: 'path' should be relative to the 'Resources' folder and without the file extension
    return Resources.Load<Sprite>(path);
    }










    }

}