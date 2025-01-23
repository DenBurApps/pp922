using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AddTask
{
    public class DateTimeSelector : MonoBehaviour
    {
        [SerializeField] private DateSelector _dateSelector;
        [SerializeField] private TimeSelector _timeSelector;
        [SerializeField] private TMP_Text _dateText;
        [SerializeField] private Button _dateOpener;

        private string _year;
        private string _day;
        private string _month;
        private string _hour;
        private string _minute;

        private DateTime _selectedDate;

        public string Year => _year;
        public string Day => _day;
        public string Month => _month;
        public DateTime SelectedDate => _selectedDate;
        public DateTime Date { get; private set; }

        private void OnEnable()
        {
            _dateOpener.onClick.AddListener(ToggleDateSelector);

            _dateSelector.YearInputed += SetYear;
            _dateSelector.MonthInputed += SetMonth;
            _dateSelector.DayInputed += SetDay;

            _timeSelector.HourInputed += SetHour;
            _timeSelector.MinuteInputed += SetMinute;
        }

        private void OnDisable()
        {
            _dateOpener.onClick.RemoveListener(ToggleDateSelector);

            _dateSelector.YearInputed -= SetYear;
            _dateSelector.MonthInputed -= SetMonth;
            _dateSelector.DayInputed -= SetDay;

            _timeSelector.HourInputed -= SetHour;
            _timeSelector.MinuteInputed -= SetMinute;
        }

        private void Start()
        {
            ResetValues();
            SetDate();
            DisplayCurrentDateTime();
        }

        private void SetYear(string year)
        {
            _year = year;
            SetDate();
        }

        private void SetMonth(string month)
        {
            _month = month;
            SetDate();
        }

        private void SetDay(string day)
        {
            _day = day;
            SetDate();
        }

        private void SetHour(string hour)
        {
            _hour = hour;
            UpdateDateText();
        }

        private void SetMinute(string minute)
        {
            _minute = minute;
            UpdateDateText();
        }

        private void SetDate()
        {
            if (string.IsNullOrEmpty(_year) || string.IsNullOrEmpty(_month) || string.IsNullOrEmpty(_day))
            {
                Debug.LogWarning("Date components are missing. Cannot set date.");
                return;
            }

            try
            {
                string dateString = $"{_year}-{_month}-{_day}";
                _selectedDate = DateTime.Parse(dateString);
                UpdateDateText();
            }
            catch (FormatException ex)
            {
                Debug.LogError($"Invalid date format: {_day}-{_month}-{_year}. Exception: {ex.Message}");
                _dateText.text = "Invalid Date";
            }
        }

        private void UpdateDateText()
        {
            if (_selectedDate == default)
            {
                Debug.LogWarning("Selected date is not set. Cannot update date text.");
                return;
            }
            
            int hour = string.IsNullOrEmpty(_hour) ? _selectedDate.Hour : int.Parse(_hour);
            int minute = string.IsNullOrEmpty(_minute) ? _selectedDate.Minute : int.Parse(_minute);
            
            DateTime updatedDate = _selectedDate.Date.AddHours(hour).AddMinutes(minute);
            
            Date = updatedDate;
            _dateText.text = updatedDate.ToString("dd.MM.yyyy, HH:mm");
        }

        public void DisplayCurrentDateTime()
        {
            DateTime currentDateTime = DateTime.Now;
            _selectedDate = currentDateTime;
            _year = currentDateTime.Year.ToString();
            _month = currentDateTime.Month.ToString("D2");
            _day = currentDateTime.Day.ToString("D2");
            _hour = currentDateTime.Hour.ToString("D2");
            _minute = currentDateTime.Minute.ToString("D2");
            _dateText.text = currentDateTime.ToString("dd.MM.yyyy, HH:mm");
        }

        public void SetData(DateTime Date)
        {
            _selectedDate = Date;
            _dateText.text = _selectedDate.ToString("dd.MM.yyyy, HH:mm");
        }

        private void ToggleDateSelector()
        {
            if (_dateSelector.isActiveAndEnabled)
            {
                _dateSelector.Disable();
                _timeSelector.Disable();
            }
            else
            {
                _dateSelector.Enable();
                _timeSelector.Enable();
            }
        }

        public void ResetValues()
        {
            _dateSelector.gameObject.SetActive(false);
            _timeSelector.gameObject.SetActive(false);
            _selectedDate = DateTime.Now;
            _year = _selectedDate.Year.ToString();
            _month = _selectedDate.Month.ToString("D2");
            _day = _selectedDate.Day.ToString("D2");
            _hour = _selectedDate.Hour.ToString("D2");
            _minute = _selectedDate.Minute.ToString("D2");
            UpdateDateText();
        }
    }
}
