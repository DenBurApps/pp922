using System;
using TaskListScreen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AddTask
{
    [RequireComponent(typeof(ScreenVisabilityHandler))]
    public class AddTaskScreen : MonoBehaviour
    {
        [SerializeField] private Color _selectedButtonColor;
        [SerializeField] private Color _defaultButtonColor;
        
        [SerializeField] private Color _selectedTextColor;
        [SerializeField] private Color _defaultTextColor;
        
        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private DateTimeSelector _dateTimeSelector;
        [SerializeField] private TMP_InputField _categoryInput;
        [SerializeField] private Button _low, _medium, _high;
        [SerializeField] private TMP_Text _lowText, _mediumText, _highText;
        [SerializeField] private TMP_InputField _commentInput;
        [SerializeField] private Button _addTaskButton;
        [SerializeField] private Button _taskListButton, _settingsButton;

        private PriorityType _currentType;
        private ScreenVisabilityHandler _screenVisabilityHandler;
        
        public event Action<TaskData> DataAdded;
        public event Action TaskListClicked;
        public event Action SettingsClicked;

        private void Awake()
        {
            _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        }

        private void OnEnable()
        {
            _addTaskButton.onClick.AddListener(AddNewTask);

            _low.onClick.AddListener(OnLowPriorityClicked);
            _medium.onClick.AddListener(OnMediumPriorityClicked);
            _high.onClick.AddListener(OnHighPriorityClicked);

            _nameInput.onValueChanged.AddListener((arg0) => ToggleAddTaskButton());
            _categoryInput.onValueChanged.AddListener((arg0) => ToggleAddTaskButton());

            _taskListButton.onClick.AddListener(OnTaskListClicked);
            _settingsButton.onClick.AddListener(OnSettingsClicked);
        }

        private void OnDisable()
        {
            _addTaskButton.onClick.RemoveListener(AddNewTask);

            _low.onClick.RemoveListener(OnLowPriorityClicked);
            _medium.onClick.RemoveListener(OnMediumPriorityClicked);
            _high.onClick.RemoveListener(OnHighPriorityClicked);

            _nameInput.onValueChanged.RemoveListener((arg0) => ToggleAddTaskButton());
            _categoryInput.onValueChanged.RemoveListener((arg0) => ToggleAddTaskButton());

            _taskListButton.onClick.RemoveListener(OnTaskListClicked);
            _settingsButton.onClick.RemoveListener(OnSettingsClicked);
        }

        private void Start()
        {
            _screenVisabilityHandler.DisableScreen();
        }

        public void EnableScreen()
        {
            _screenVisabilityHandler.EnableScreen();
            ResetUIElements();
            ToggleAddTaskButton();
        }

        private void OnLowPriorityClicked() => OnPriorityButtonClicked(_low);
        private void OnMediumPriorityClicked() => OnPriorityButtonClicked(_medium);
        private void OnHighPriorityClicked() => OnPriorityButtonClicked(_high);

        private void ResetUIElements()
        {
            _currentType = PriorityType.Low;
            _nameInput.text = string.Empty;
            _dateTimeSelector.ResetValues();
            _dateTimeSelector.DisplayCurrentDateTime();
            _categoryInput.text = string.Empty;
            _commentInput.text = string.Empty;
            UpdateTypeButtons();
        }

        private void OnPriorityButtonClicked(Button button)
        {
            if (button == _low)
                _currentType = PriorityType.Low;
            else if (button == _medium)
                _currentType = PriorityType.Medium;
            else
                _currentType = PriorityType.High;
                
            UpdateTypeButtons();
        }
        
        private void UpdateTypeButtons()
        {
            _low.image.color = _defaultButtonColor;
            _medium.image.color = _defaultButtonColor;
            _high.image.color = _defaultButtonColor;

            _lowText.color = _defaultTextColor;
            _mediumText.color = _defaultTextColor;
            _highText.color = _defaultTextColor;
            
            if (_currentType == PriorityType.Low)
            {
                _low.image.color = _selectedButtonColor;
                _lowText.color = _selectedTextColor;
            }
            else if(_currentType == PriorityType.Medium)
            {
                _medium.image.color = _selectedButtonColor;
                _mediumText.color = _selectedTextColor;
            }
            else
            {
                _high.image.color = _selectedButtonColor;
                _highText.color = _selectedTextColor;
            }
        }

        private void ToggleAddTaskButton()
        {
            _addTaskButton.interactable = 
                !string.IsNullOrWhiteSpace(_nameInput.text) && 
                !string.IsNullOrWhiteSpace(_categoryInput.text);
        }

        private void AddNewTask()
        {
            var task = new TaskData(_nameInput.text, _dateTimeSelector.SelectedDate, _categoryInput.text, _currentType)
            {
                Comment = string.IsNullOrWhiteSpace(_commentInput.text) ? null : _commentInput.text
            };

            DataAdded?.Invoke(task);
            _screenVisabilityHandler.DisableScreen();
        }

        private void OnSettingsClicked()
        {
            SettingsClicked?.Invoke();
            _screenVisabilityHandler.DisableScreen();
        }

        private void OnTaskListClicked()
        {
            TaskListClicked?.Invoke();
            _screenVisabilityHandler.DisableScreen();
        }
    }
}
