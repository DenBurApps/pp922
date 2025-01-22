using System;
using UnityEngine;
using UnityEngine.UI;

namespace TaskListScreen
{
    [RequireComponent(typeof(ScreenVisabilityHandler))]
    public class PrioritySelector : MonoBehaviour
    {
        [SerializeField] private Button _low, _medium, _high;
        [SerializeField] private Image[] _images;
        [SerializeField] private PriorityType _currentType;

        private ScreenVisabilityHandler _screenVisabilityHandler;

        public event Action<PriorityType> TypeSelected; 

        private void Awake()
        {
            _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        }

        private void OnEnable()
        {
            _low.onClick.AddListener((() => OnButtonClicked(_low)));
            _medium.onClick.AddListener((() => OnButtonClicked(_medium)));
            _high.onClick.AddListener((() => OnButtonClicked(_high)));
        }

        private void OnDisable()
        {
            _low.onClick.RemoveListener((() => OnButtonClicked(_low)));
            _medium.onClick.RemoveListener((() => OnButtonClicked(_medium)));
            _high.onClick.RemoveListener((() => OnButtonClicked(_high)));
        }

        private void Start()
        {
            _screenVisabilityHandler.DisableScreen();
            EnableImage();
        }

        public void EnableScreen()
        {
            _screenVisabilityHandler.EnableScreen();
        }

        private void OnButtonClicked(Button button)
        {
            if (button == _low)
                _currentType = PriorityType.Low;
            else if (button == _medium)
                _currentType = PriorityType.Medium;
            else
                _currentType = PriorityType.High;
            
            EnableImage();
            _screenVisabilityHandler.DisableScreen();
            TypeSelected?.Invoke(_currentType);
        }

        private void EnableImage()
        {
            foreach (var image in _images)
            {
                image.gameObject.SetActive(false);
            }

            if (_currentType == PriorityType.Low)
                _images[0].gameObject.SetActive(true);
            else if (_currentType == PriorityType.Medium)
                _images[1].gameObject.SetActive(true);
            else
                _images[2].gameObject.SetActive(true);
        }
    }

    public enum PriorityType
    {
        Low,
        Medium,
        High
    }
}