using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TaskListScreen
{
    public class TaskPlane : MonoBehaviour
    {
        private const string LowPriorityText = "Low";
        private const string MediumPriorityText = "Medium";
        private const string HighPriorityText = "High";
        
        [SerializeField] private Color _lowPriorityColor;
        [SerializeField] private Color _mediumPriorityColor;
        [SerializeField] private Color _highPriorityColor;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _dateText;
        [SerializeField] private TMP_Text _categoryText;
        [SerializeField] private TMP_Text _priorityText;
        [SerializeField] private Button _openButton;
        [SerializeField] private Image _completeImage;

        public event Action<TaskPlane> Opened;  
        
        public TaskData TaskData { get; private set; }
        public bool IsActive { get; private set; }

        private void OnEnable()
        {
            _openButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _openButton.onClick.RemoveListener(OnButtonClicked);
        }

        public void EnablePlane(TaskData data)
        {
            TaskData = data ?? throw new ArgumentNullException(nameof(data));
            
            gameObject.SetActive(true);
            IsActive = true;
            UpdateUIText();

            _completeImage.gameObject.SetActive(TaskData.IsComplete);
        }

        public void Enable()
        {
            IsActive = true;
            gameObject.SetActive(false);
        }

        public void Disable()
        {
            IsActive = false;
            gameObject.SetActive(false);
        }

        private void UpdateUIText()
        {
            _nameText.text = TaskData.Name;
            _categoryText.text = TaskData.Category;
            SetPriority();
            DisplayCurrentDateTime(TaskData.Date);
        }
        
        public void DisplayCurrentDateTime(DateTime currentDateTime)
        {
            _dateText.text = currentDateTime.ToString("dd.MM.yyyy, HH:mm");
        }

        private void SetPriority()
        {
            if (TaskData.PriorityType == PriorityType.Low)
            {
                _priorityText.text = LowPriorityText;
                _priorityText.color = _lowPriorityColor;
            }
            else if(TaskData.PriorityType == PriorityType.Medium)
            {
                _priorityText.text = MediumPriorityText;
                _priorityText.color = _mediumPriorityColor;
            }
            else
            {
                _priorityText.text = HighPriorityText;
                _priorityText.color = _highPriorityColor;
            }
        }

        private void OnButtonClicked()
        {
            Opened?.Invoke(this);
        }
    }
}
