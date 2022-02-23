using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class Utility {

    [System.Serializable]
    /// <summary>
    /// serializable entry of a dictionary, for use in the editor
    /// </summary>
    /// <typeparam name="K">key</typeparam>
    /// <typeparam name="V">val</typeparam>
    public class DictEntry<K, V>
    {
        public K key;
        public V val;

        public DictEntry(K key, V val)
        {
            this.key = key;
            this.val = val;
        } 
    }

    /// <summary>
    /// creates a K-V dictionary from an array of DictEntry objects
    /// </summary>
    /// <param name="dictArr">an array of type DictEntry K-V</param>
    /// <typeparam name="K">the Key type</typeparam>
    /// <typeparam name="V">the Val type</typeparam>
    /// <returns>the dictionary</returns>
    public static Dictionary<K, V> GetDict<K, V>(DictEntry<K, V>[] dictArr)
    {
        Dictionary<K, V> dict = new Dictionary<K, V>();
        for (int i = 0; i < dictArr.Length; i++)
        {
            dict[dictArr[i].key] = dictArr[i].val;
        }
        return dict;
    }

    /* given a point x and a line stemming from point t forward, find the point closest to x that lies on t */
    public static Vector3 GetClosestPointOnLine(Transform t, Vector3 x)
    {
        Vector3 p = t.position;
        Vector3 q = t.position + t.forward * 5;
        float k = Vector3.Dot(x - p, q - p) / Vector3.Dot(q - p, q - p);
        return p + k * (q - p);
    }

    /*  returns the closest n gameObjects within a radius with a certain tag 
        the array returned may be fully or partially empty 
        Runs in O(nk)*/
    public static GameObject[] GetNClosestWithTag(Vector3 pos, float radius, int n, string tag)
    {
        Tuple<float, GameObject>[] closest = new Tuple<float, GameObject>[n]; // store distance to object, and the object, in a tuple
        int j = 0;
        Collider[] cols = Physics.OverlapSphere(pos, radius); // get all colliders in radius
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].tag == tag)
            {
                float val = (pos - cols[i].transform.position).sqrMagnitude; // get distance of current object
                // if only 1 object, skip the complex step
                if (n == 1)
                {
                    if (j == 0 || val < closest[0].Item1)
                    {
                        closest[0] = Tuple.Create(val, cols[i].gameObject);
                        j = 1;
                    }
                }
                else
                {
                    if (j < n || val < closest[n - 2].Item1) // if any slot is empty or new val is smaller than largest in closest
                    {
                        // add to correct position
                        int idx = n - 2;
                        for (int k = 0; k < idx; k++)
                        {
                            if (k >= j || val < closest[k].Item1) // if the current slot is open OR new val is smaller than val at k
                            {
                                idx = k; // prevent further searching
                            }
                        }
                        // now shift all entries in array down by 1, starting after idx
                        for (int k = n - 1; k > idx; k--)
                        {
                            closest[k] = closest[k - 1];
                        }
                        closest[idx] = Tuple.Create(val, cols[i].gameObject); // add new object at correct position
                        j++;
                    }
                }
            }
        }
        //return only the 2nd item from each tuple
        GameObject[] keepers = new GameObject[n];
        for (int i = 0; i < n; i++)
        {
            if (closest[i] != null)
            {
                keepers[i] = closest[i].Item2;
            }
        }
        return keepers;
    }

    /*  can be used in place of WaitForSeconds when timeScale != 1.0 */
    public static IEnumerator WaitForRealSeconds(float seconds)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup-startTime < seconds) {
            yield return null;
        }
    }

    /// <summary>
    /// randomizes the contents of a list
    /// </summary>
    /// <param name="lst">the list we are shuffling</param>
    /// <typeparam name="T">The type of the list</typeparam>
    public static void Shuffle<T>(IList<T> lst) {
        var count = lst.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = lst[i];
            lst[i] = lst[r];
            lst[r] = tmp;
        }
    }

    /// <summary>
    /// Calculates the strength of a sound signal at a specified distance from the source
    /// The intensity is assumed to be normal at 10m away
    /// and drops by 6 dB every time the distance doubles
    /// </summary>
    /// <param name="volume">the volume of the original signal in dB</param>
    /// <param name="distance">the distance between the source and listener in meters</param>
    /// <returns></returns>
    public static float CalculateVolumeAtDistance(float volume, float distance)
    {
        float loss = -6f * Mathf.Log(distance / 10f, 2);
        return volume + loss;
    }

    #region conversions

    /// <summary>
    /// converts pounds (lb) to kilograms (kg)
    /// </summary>
    /// <param name="lb">weight in pounds</param>
    /// <returns>weight in kilograms</returns>
    public static float PoundsToKg(float lb)
    {
        return lb / 2.205f;
    }

    /// <summary>
    /// converts kilograms (kg) to pounds (lb)
    /// </summary>
    /// <param name="kg">weight in kilograms</param>
    /// <returns>weight in pounds</returns>
    public static float KgToPounds(float kg)
    {
        return kg * 2.205f;
    }

    /// <summary>
    /// converts feet (ft) to meters (m)
    /// </summary>
    /// <param name="ft">distance in feet</param>
    /// <returns>distance in meters</returns>
    public static float FeetToMeters(float ft)
    {
        return ft / 3.281f;
    }

    /// <summary>
    /// converts meters (m) to feet (ft)
    /// </summary>
    /// <param name="m">distance in meters</param>
    /// <returns>distance in feet</returns>
    public static float MetersToFeet(float m)
    {
        return m * 3.281f;
    }

    #endregion

#if (UNITY_EDITOR) 
    [MenuItem("Assets/Save RenderTexture to file")]
    public static void SaveRTToFile()
    {
        RenderTexture rt = Selection.activeObject as RenderTexture;

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        RenderTexture.active = null;

        byte[] bytes;
        bytes = tex.EncodeToPNG();
        
        string path = AssetDatabase.GetAssetPath(rt) + ".png";
        System.IO.File.WriteAllBytes(path, bytes);
        AssetDatabase.ImportAsset(path);
        Debug.Log("Saved to " + path);
    }

    [MenuItem("Assets/Save RenderTexture to file", true)]
    public static bool SaveRTToFileValidation()
    {
        return Selection.activeObject is RenderTexture;
    }
#endif

}