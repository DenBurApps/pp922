using System;
using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using UnityEngine;

namespace AddTask
{
    public class TimeSelector : MonoBehaviour
    {
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _unselectedColor;

        [SerializeField] private SimpleScrollSnap _hourScrollSnap;
        [SerializeField] private SimpleScrollSnap _minuteScrollSnap;
        [SerializeField] private TMP_Text[] _hourText;
        [SerializeField] private TMP_Text[] _minuteText;

        private string _hour;
        private string _minute;

        public event Action<string> HourInputed;
        public event Action<string> MinuteInputed;

        private void OnEnable()
        {
            _hourScrollSnap.OnPanelCentered.AddListener(SetHour);
            _minuteScrollSnap.OnPanelCentered.AddListener(SetMinute);
        }

        private void OnDisable()
        {
            _hourScrollSnap.OnPanelCentered.RemoveListener(SetHour);
            _minuteScrollSnap.OnPanelCentered.RemoveListener(SetMinute);
        }

        private void Start()
        {
            InitializeTimeFields();
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

        private void SetHour(int start, int end)
        {
            _hour = _hourText[start].text;
            SetColorForSelected(_hourText, start);
            HourInputed?.Invoke(_hour);
        }

        private void SetMinute(int start, int end)
        {
            _minute = _minuteText[start].text;
            SetColorForSelected(_minuteText, start);
            MinuteInputed?.Invoke(_minute);
        }

        private void InitializeTimeFields()
        {
            PopulateHours();
            PopulateMinutes();
            SetColorForSelected(_hourText, 0);
            SetColorForSelected(_minuteText, 0);
        }

        private void PopulateHours()
        {
            for (int i = 0; i < _hourText.Length; i++)
            {
                _hourText[i].text = i < 24 ? i.ToString("00") : "";
            }
        }

        private void PopulateMinutes()
        {
            for (int i = 0; i < _minuteText.Length; i++)
            {
                _minuteText[i].text = i < 60 ? i.ToString("00") : "";
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
            _hourScrollSnap.GoToPanel(0);
            _minuteScrollSnap.GoToPanel(0);

            _hour = string.Empty;
            _minute = string.Empty;
        }
    }
}
