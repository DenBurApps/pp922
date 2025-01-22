using System;
using DanielLochner.Assets;
using UnityEngine;
using UnityEngine.UI;

namespace EditTask
{
    [RequireComponent(typeof(ScreenVisabilityHandler))]
    public class DeleteConfirmScreen : MonoBehaviour
    {
        [SerializeField] private Button _yes;
        [SerializeField] private Button _no;

        private ScreenVisabilityHandler _screenVisabilityHandler;

        public event Action YesClicked;
        
        private void Awake()
        {
            _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        }

        private void Start()
        {
            _screenVisabilityHandler.DisableScreen();
        }

        public void Enable()
        {
            _screenVisabilityHandler.EnableScreen();
        }
        
        private void OnEnable()
        {
            _yes.onClick.AddListener(OnYesClicked);
            _no.onClick.AddListener(OnNoClicked);
        }

        private void OnDisable()
        {
            _yes.onClick.RemoveListener(OnYesClicked);
            _no.onClick.RemoveListener(OnNoClicked);
        }

        private void OnYesClicked()
        {
            YesClicked?.Invoke();
            _screenVisabilityHandler.DisableScreen();
        }

        private void OnNoClicked()
        {
            _screenVisabilityHandler.DisableScreen();
        }
    }
}
