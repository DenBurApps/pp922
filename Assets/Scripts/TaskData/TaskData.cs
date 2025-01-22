using System;
using System.Collections;
using System.Collections.Generic;
using TaskListScreen;
using UnityEngine;

[Serializable]
public class TaskData
{
    public string Name;
    public DateTime Date;
    public string Category;
    public PriorityType PriorityType;
    public string Comment;
    public bool IsComplete;

    public TaskData(string name, DateTime date, string category, PriorityType priorityType)
    {
        Name = name;
        Date = date;
        Category = category;
        PriorityType = priorityType;
    }
}
