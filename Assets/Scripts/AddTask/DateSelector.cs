using System;
using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using UnityEngine;

namespace AddTask
{
    public class DateSelector : MonoBehaviour
    {
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _unselectedColor;

        [SerializeField] private SimpleScrollSnap _dayScrollSnap;
        [SerializeField] private SimpleScrollSnap _monthScrollSnap;
        [SerializeField] private SimpleScrollSnap _yearScrollSnap;
        [SerializeField] private TMP_Text[] _numbersText;
        [SerializeField] private TMP_Text[] _wordsText;
        [SerializeField] private TMP_Text[] _yearText;

        private string _year;
        private string _month;
        private string _day;

        public event Action<string> DayInputed;
        public event Action<string> MonthInputed;
        public event Action<string> YearInputed;


        private void OnEnable()
        {
            _dayScrollSnap.OnPanelCentered.AddListener(SetDay);
            _monthScrollSnap.OnPanelCentered.AddListener(SetMonth);
            _yearScrollSnap.OnPanelCentered.AddListener(SetYear);
        }

        private void OnDisable()
        {
            _dayScrollSnap.OnPanelCentered.RemoveListener(SetDay);
            _monthScrollSnap.OnPanelCentered.RemoveListener(SetMonth);
            _yearScrollSnap.OnPanelCentered.RemoveListener(SetYear);
        }

        private void Start()
        {
            InitializeTextFields();
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            Reset();
            gameObject.SetActive(false);
        }

        private void SetDay(int start, int end)
        {
            _day = _numbersText[start].text;
            SetColorForSelected(_numbersText, start);
            DayInputed?.Invoke(_day);
        }

        private void SetMonth(int start, int end)
        {
            _month = _wordsText[start].text;
            SetColorForSelected(_wordsText, start);
            MonthInputed?.Invoke(_month);
        }

        private void SetYear(int start, int end)
        {
            _year = _yearText[start].text;
            SetColorForSelected(_yearText, start);
            YearInputed?.Invoke(_year);
        }

        private void InitializeTextFields()
        {
            PopulateDays();
            PopulateMonths();
            PopulateYears();
            SetColorForSelected(_numbersText, 0);
            SetColorForSelected(_wordsText, 0);
            SetColorForSelected(_yearText, 0);
        }

        private void PopulateDays()
        {
            int daysInMonth = 31;
            for (int i = 0; i < _numbersText.Length; i++)
            {
                _numbersText[i].text = i < daysInMonth ? (i + 1).ToString("00") : "";
            }
        }

        private void PopulateMonths()
        {
            string[] monthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            for (int i = 0; i < _wordsText.Length; i++)
            {
                _wordsText[i].text = i < monthNames.Length - 1 ? monthNames[i] : "";
            }
        }

        private void PopulateYears()
        {
            int currentYear = DateTime.Now.Year;
            for (int i = 0; i < _yearText.Length; i++)
            {
                _yearText[i].text = (currentYear - i).ToString();
            }
        }

        private void SetColorForSelected(TMP_Text[] texts, int selectedIndex)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].color = i == selectedIndex ? _selectedColor : _unselectedColor;
            }
        }

        private void Reset()
        {
            _dayScrollSnap.GoToPanel(0);
            _monthScrollSnap.GoToPanel(0);
            _yearScrollSnap.GoToPanel(0);

            _month = string.Empty;
            _year = string.Empty;
            _day = string.Empty;
        }
    }
}