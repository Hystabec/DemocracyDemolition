using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// using these to read and write to files
/// </summary>
using System;
using System.IO;

/// <summary>
/// Makes a serializable class that will hold the Data
/// </summary>
[Serializable]
public class testData
{
    public string name;
    public int score;
}

/// <summary>
/// creates a class to hold of list of the data class - this is as you cant save a list directly to a file,
/// so it needs the be converted to a class first
/// </summary>
[Serializable]
public class listOfTestData
{
    public List<testData> dataList;
}

public class testSaving : MonoBehaviour
{
    void Start()
    {
        //the location that the file will be saved too
        string filePath = Application.dataPath + "/test.json";

        ///WRITING TO FILE

        //creates a new list holder class
        listOfTestData lotd = new listOfTestData();
        //creates the list inside the class
        lotd.dataList = new List<testData>();

        //adds data to the list inside the list holder class - this is some random test data
        lotd.dataList.Add(new testData { name = "Bob", score = 8 });
        lotd.dataList.Add(new testData { name = "Steve", score = 8832 });

        //converts the class to a Json string that can be saved into a file
        string dataAsJson = JsonUtility.ToJson(lotd);
        
        //writes the converted string to the file
        File.WriteAllText(filePath, dataAsJson);

        Debug.Log(filePath);

        ///READING FROM FILE

        //reads all the data out of the file into a string
        string fileContent = File.ReadAllText(filePath);

        //converts the string into the list holder class
        listOfTestData loadedList = JsonUtility.FromJson<listOfTestData>(fileContent);

        //loops through each of the elements in the list and outputs names and scores to the console
        foreach(testData td in loadedList.dataList)
        {
            Debug.Log("Name: " + td.name);
            Debug.Log("Score: " + td.score);
        }
    }
}
