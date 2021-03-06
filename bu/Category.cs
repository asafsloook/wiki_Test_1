﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI.WebControls;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Xml.Linq;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

public class Category
{
    public Category()
    {

    }

    int categoryId;
    public int CategoryId { get; set; }

    string name;
    public string Name { get; set; }


    /// <summary>
    /// Get the main/root categories from wikipedia
    /// </summary>
    /// 
    public List<string> getMainCategories()
    {
        string ResponseText;
        HttpWebRequest myRequest =
        (HttpWebRequest)WebRequest.Create("https://en.wikipedia.org/w/api.php?format=json&action=query&list=categorymembers&cmtitle=Category:Main_topic_classifications&cmlimit=100");
        using (HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse())
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                ResponseText = reader.ReadToEnd();
            }
        }

        JObject root = JObject.Parse(ResponseText);
        var dig = root["query"]["categorymembers"];

        List<string> mainCategories = new List<string>();

        foreach (var item in dig)
        {
            mainCategories.Add(item["title"].ToString().Replace("Category:", ""));
        }

        return mainCategories;
    }

    public void updateCategories(string[] arr, string IMEI)
    {
        //check categories and insert if nessececececery
        //return arr of category IDS
        List<int> catIDS = checkCategories(arr);


        //update/insert to userCategories
        DBConnection db = new DBConnection();
        db.updateUserCategories(catIDS, IMEI);
    }

    public List<int> checkCategories(string[] arr)
    {
        DBConnection db = new DBConnection();
        return db.checkCategories(arr);
    }
}