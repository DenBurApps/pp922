using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private GameObject _privacyCanvas;
    [SerializeField] private GameObject _termsCanvas;
    [SerializeField] private GameObject _contactCanvas;
    [SerializeField] private GameObject _versionCanvas;
    [SerializeField] private TMP_Text _versionText;
    [SerializeField] private Button _addTaskButton;
    [SerializeField] private Button _taskListButton;
    private string _version = "Application version:\n";

    public event Action AddTaskClicked;
    public event Action TaskListClicked;

    private void Awake()
    {
        _settingsCanvas.SetActive(false);
        _privacyCanvas.SetActive(false);
        _termsCanvas.SetActive(false);
        _contactCanvas.SetActive(false);
        _versionCanvas.SetActive(false);
        SetVersion();
    }

    private void OnEnable()
    {
        _taskListButton.onClick.AddListener(OnTaskListClicked);
        _addTaskButton.onClick.AddListener(OnAddTaskClicked);
    }

    private void OnDisable()
    {
        _taskListButton.onClick.RemoveListener(OnTaskListClicked);
        _addTaskButton.onClick.RemoveListener(OnAddTaskClicked);
    }

    private void SetVersion()
    {
        _versionText.text = _version + Application.version;
    }

    public void ShowSettings()
    {
        _settingsCanvas.SetActive(true);
    }

    public void RateUs()
    {
#if UNITY_IOS
        Device.RequestStoreReview();
#endif
    }

    private void OnAddTaskClicked()
    {
        AddTaskClicked?.Invoke();
        _settingsCanvas.SetActive(false);
    }

    private void OnTaskListClicked()
    {
        TaskListClicked?.Invoke();
        _settingsCanvas.SetActive(false);
    }
}