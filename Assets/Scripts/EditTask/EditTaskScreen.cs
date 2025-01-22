using System;
using AddTask;
using OpenTask;
using TaskListScreen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EditTask
{
    [RequireComponent(typeof(ScreenVisabilityHandler))]
    public class EditTaskScreen : MonoBehaviour
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
        [SerializeField] private Button _saveTask;
        [SerializeField] private DeleteConfirmScreen _deleteConfirmScreen;
        [SerializeField] private Button _deleteButton;
        [SerializeField] private OpenTaskScreen _openTaskScreen;
        
        private PriorityType _currentType;
        private ScreenVisabilityHandler _screenVisabilityHandler;
        private TaskPlane _currentPlane;
        
        public event Action DataEdited;
        public event Action<TaskPlane> DataDeleted;
        
        private void Awake()
        {
            _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        }
        
        private void OnEnable()
        {
            _saveTask.onClick.AddListener(SaveTask);
            _deleteConfirmScreen.YesClicked += OnConfirmDeleteClicked;

            _low.onClick.AddListener(OnLowPriorityClicked);
            _medium.onClick.AddListener(OnMediumPriorityClicked);
            _high.onClick.AddListener(OnHighPriorityClicked);

            _nameInput.onValueChanged.AddListener((arg0) => ToggleAddTaskButton());
            _categoryInput.onValueChanged.AddListener((arg0) => ToggleAddTaskButton());
            
            _deleteButton.onClick.AddListener(OnDeleteButtonClicked);

            _openTaskScreen.EditClicked += EnableScreen;
        }

        private void OnDisable()
        {
            _saveTask.onClick.RemoveListener(SaveTask);
            
            _deleteConfirmScreen.YesClicked -= OnConfirmDeleteClicked;

            _low.onClick.RemoveListener(OnLowPriorityClicked);
            _medium.onClick.RemoveListener(OnMediumPriorityClicked);
            _high.onClick.RemoveListener(OnHighPriorityClicked);
            
            _deleteButton.onClick.RemoveListener(OnDeleteButtonClicked);
            
            _openTaskScreen.EditClicked -= EnableScreen;

            _nameInput.onValueChanged.RemoveListener((arg0) => ToggleAddTaskButton());
            _categoryInput.onValueChanged.RemoveListener((arg0) => ToggleAddTaskButton());
        }

        private void Start()
        {
            _screenVisabilityHandler.DisableScreen();
        }
        
        public void EnableScreen(TaskPlane taskPlane)
        {
            _screenVisabilityHandler.EnableScreen();
            ResetUIElements();
            _currentPlane = taskPlane;
            
            var taskData = taskPlane.TaskData;

            _nameInput.text = taskData.Name;
            _dateTimeSelector.SetData(taskData.Date);
            _categoryInput.text = taskData.Category;
            _commentInput.text = taskData.Comment ?? string.Empty;
            
            _currentType = taskData.PriorityType;
            UpdateTypeButtons();

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
            _saveTask.interactable = 
                !string.IsNullOrWhiteSpace(_nameInput.text) && 
                !string.IsNullOrWhiteSpace(_categoryInput.text);
        }

        private void SaveTask()
        {
            var task = new TaskData(_nameInput.text, _dateTimeSelector.SelectedDate, _categoryInput.text, _currentType)
            {
                Comment = string.IsNullOrWhiteSpace(_commentInput.text) ? null : _commentInput.text
            };

            _currentPlane.UpdateData(task);
            _screenVisabilityHandler.DisableScreen();
            DataEdited?.Invoke();
        }

        private void OnDeleteButtonClicked()
        {
            _deleteConfirmScreen.Enable();
        }

        private void OnConfirmDeleteClicked()
        {
            DataDeleted?.Invoke(_currentPlane);
            _screenVisabilityHandler.DisableScreen();
        }
    }
}
