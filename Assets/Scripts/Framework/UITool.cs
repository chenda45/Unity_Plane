using System;
using UnityEngine;
using System.Text;
using System.Collections; 

public class UITool
{
    public static UIBase FindChild(Transform tranform, string name) 
    { 
        if(name.ToLower().Contains("(button)"))
        {
             
        }
        else if (name.ToLower().Contains("(sprite)"))
        {
            
        }
        else if (name.ToLower().Contains("(input)"))
        {

        }
        else if (name.ToLower().Contains("(texture)"))
        {

        }
        else if (name.ToLower().Contains("(checkbox)"))
        {

        }
        else if (name.ToLower().Contains("(checkbox)"))
        {

        }
        return null;
    }

    /// <summary>
    /// panel路径
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetPanelRelativePath(string name) 
    {
        StringBuilder s = new StringBuilder();
        s.Append("resources/uiresources/panel/");
        s.Append(name);
        s.Append(".x");
        return s.ToString().ToLower();
    }

    /// <summary>
    /// atlas路径
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetAtlasRelativePath(string name)
    {
        StringBuilder s = new StringBuilder();
        s.Append("resources/uiresources/atlas/");
        s.Append(name);
        s.Append(".x");
        return s.ToString().ToLower();
    }

    /// <summary>
    /// Component路径
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetComponentRelativePath(string name)
    {
        StringBuilder s = new StringBuilder();
        s.Append("resources/uiresources/component/");
        s.Append(name);
        s.Append(".x");
        return s.ToString().ToLower();
    }
} 
