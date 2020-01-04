using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TextureLoader
{
    public static class Util
    {
        static Dictionary<string, Texture2D> TextureLookup = new Dictionary<string, Texture2D>();
        public static Texture2D LoadTexture(string filePath)
        {
            if (TextureLookup.ContainsKey(filePath))
            {
                return TextureLookup[filePath];
            }
            else
            {
                var fileData = File.ReadAllBytes(filePath);
                var texture = new Texture2D(2, 2);
                texture.LoadImage(fileData);
                TextureLookup[filePath] = texture;
                return texture;
            }
        }
        public static void AddContainer<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key, TValue entry)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Add(entry);
            } else
            {
                dictionary[key] = new List<TValue>() { entry };
            }
        }
    }
}
