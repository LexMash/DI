using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public class Test
    {
        Dictionary<Type, IList> cashed = new();

        List<string> strings = new();

        public Test()
        {
            strings.Add("aa");
            strings.Add("bb");

            cashed[typeof(string)] = strings;

            List<string> request = cashed[typeof(string)].ConvertTo<List<string>>();

            foreach (string str in request)
            {
                Debug.Log(str);
            }
            
        }
    }
}
