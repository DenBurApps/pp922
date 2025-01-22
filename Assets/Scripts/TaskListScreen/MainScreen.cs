using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AddTask;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TaskListScreen
{
    [RequireComponent(typeof(ScreenVisabilityHandler))]
    public class MainScreen : MonoBehaviour
    {
        [SerializeField] private Color _selectedButtonColor;
        [SerializeField] private Color _unselectedButtonColor;

        [SerializeField] private PrioritySelector _prioritySelector;
        [SerializeField] private TMP_InputField _searchInput;
        [SerializeField] private Button _prioritySelectButton;
        [SerializeField] private TMP_Text _priorityText;
        [SerializeField] private Button _inProcessButton, _completeButton;
        [SerializeField] private List<TaskPlane> _taskPlanes;
        [SerializeField] private GameObject _emptyPlanes;
        [SerializeField] private GameObject _emptySearch;
        [SerializeField] private Button _addButton, _settingsButton;
        [SerializeField] private AddTaskScreen _addTaskScreen;

        private ScreenVisabilityHandler _screenVisabilityHandler;
        private PriorityType _currentPriority;
        private bool _isShowingCompleteTasks;

        private void Awake()
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        }

        private void OnEnable()
        {
            _prioritySelectButton.onClick.AddListener(EnablePrioritySelector);
            _inProcessButton.onClick.AddListener(OnInProcessClicked);
            _completeButton.onClick.AddListener(OnCompleteClicked);
            _searchInput.onValueChanged.AddListener(SearchInputed);

            _prioritySelector.TypeSelected += UpdateType;
            
            _addButton.onClick.AddListener(OpenAddTask);

            _addTaskScreen.DataAdded += AddTask;
            _addTaskScreen.TaskListClicked += _screenVisabilityHandler.EnableScreen;
        }

        private void OnDisable()
        {
            _prioritySelectButton.onClick.RemoveListener(EnablePrioritySelector);
            _inProcessButton.onClick.RemoveListener(OnInProcessClicked);
            _completeButton.onClick.RemoveListener(OnCompleteClicked);
            _searchInput.onValueChanged.RemoveListener(SearchInputed);

            _prioritySelector.TypeSelected -= UpdateType;
            
            _addButton.onClick.RemoveListener(OpenAddTask);

            _addTaskScreen.DataAdded -= AddTask;
            _addTaskScreen.TaskListClicked -= _screenVisabilityHandler.EnableScreen;
        }

        private void Start()
        {
            _screenVisabilityHandler.EnableScreen();
            _isShowingCompleteTasks = false;
            _currentPriority = PriorityType.Low;
            DisableAllPlanes();
            _emptyPlanes.SetActive(true);
            _emptySearch.SetActive(false);
        }

        private void UpdateType(PriorityType type)
        {
            _currentPriority = type;
            _priorityText.text = $"Priority ({type.ToString().ToLower()})";
            FilterAndDisplayPlanes();
        }

        private void DisableAllPlanes()
        {
            foreach (var plane in _taskPlanes)
            {
                plane.Disable();
            }
        }

        private void EnablePrioritySelector()
        {
            _prioritySelector.EnableScreen();
        }

        private void OnInProcessClicked()
        {
            _isShowingCompleteTasks = false;
            UpdateButtonColors(_inProcessButton, _completeButton);
            FilterAndDisplayPlanes();
        }

        private void OnCompleteClicked()
        {
            _isShowingCompleteTasks = true;
            UpdateButtonColors(_completeButton, _inProcessButton);
            FilterAndDisplayPlanes();
        }

        private void UpdateButtonColors(Button selectedButton, Button unselectedButton)
        {
            selectedButton.image.color = _selectedButtonColor;
            unselectedButton.image.color = _unselectedButtonColor;
        }

        private void SearchInputed(string searchText)
        {
            DisableAllPlanes();

            if (string.IsNullOrEmpty(searchText))
            {
                _emptySearch.SetActive(false);
                FilterAndDisplayPlanes();
                return;
            }
            
            _emptyPlanes.SetActive(false);

            if (_taskPlanes == null || !_taskPlanes.Any())
            {
                _emptySearch.SetActive(true);
                return;
            }

            var matchingPlanes = _taskPlanes
                .Where(plane => plane != null && plane.TaskData != null)
                .Where(plane => plane.TaskData.Name.StartsWith(searchText, StringComparison.OrdinalIgnoreCase));

            if (!matchingPlanes.Any())
            {
                _emptySearch.SetActive(true);
                return;
            }

            _emptySearch.SetActive(false);
            foreach (var plane in matchingPlanes)
            {
                plane.gameObject.SetActive(true);
            }
        }


        private void FilterAndDisplayPlanes()
        {
            DisableAllPlanes();

            if (_taskPlanes == null || !_taskPlanes.Any())
            {
                _emptyPlanes.SetActive(true);
                return;
            }

            var filteredPlanes = _taskPlanes
                .Where(plane => plane != null && plane.TaskData != null)
                .Where(plane => plane.TaskData.PriorityType == _currentPriority && plane.TaskData.IsComplete == _isShowingCompleteTasks);

            if (!filteredPlanes.Any())
            {
                _emptyPlanes.SetActive(true);
                return;
            }

            _emptyPlanes.SetActive(false);
            foreach (var plane in filteredPlanes)
            {
                plane.gameObject.SetActive(true);
            }
        }

        private void AddTask(TaskData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            
            _screenVisabilityHandler.EnableScreen();
            _taskPlanes.FirstOrDefault(plane => !plane.IsActive && plane.TaskData == null)?.EnablePlane(data);
            FilterAndDisplayPlanes();
        }

        private void ToggleEmptyList()
        {
            _emptyPlanes.SetActive(!_taskPlanes.Any(plane => plane.IsActive));
        }

        private void OpenAddTask()
        {
            _addTaskScreen.EnableScreen();
            _screenVisabilityHandler.DisableScreen();
        }

    }
}
