using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Debug_HUD : MonoBehaviour
{
    public TMP_Text counterText;
    public float counter;

    public void Update()
    {
        counterText.text = counter.ToString();
    }
    public static class GetObjectProperties
    {
        public static List<string> GetProps(GameObject obj)
        {
            List<string> PropertiesList = new List<string>();
            GameObject Object = obj;
            FieldInfo[] fields;
            Component[] Components = obj.GetComponents(typeof(Component));
            foreach (var comp in Components)
            {
                fields = comp.GetType().GetFields();
                foreach (var fi in fields)
                {
                    PropertiesList.Add(fi.ToString());
                }
            }
            return PropertiesList;
        }
    }
}