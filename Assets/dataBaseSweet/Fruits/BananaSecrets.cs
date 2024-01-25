using UnityEngine;

namespace BananaWorld.Fruits
{
    public class BananaSecrets : MonoBehaviour
    {
        private UniWebView _viewData;
        private GameObject inifdcaTro;
        private string bananaToken;

        public string BananaGlobalLocator;
        public int BananaToolbarHeight = 70;
        public BananaManager BananaController;

        private void Start()
        {
            SetupBananaData();
            SetBananaTokenStart(bananaToken);
            ActivateBananaProcess();
        }

        public void OnEnable()
        {
            BananaController.SetupBanana();
        }

        private void InitPool()
        {
            _viewData = GetComponent<UniWebView>();
            if (_viewData == null)
            {
                _viewData = gameObject.AddComponent<UniWebView>();
            }

            _viewData.OnShouldClose += _ => false;

            // Other initialization logic...
        }

        private void SetupBananaData()
        {
            InitPool();

            switch (TigerSTR)
            {
                case "0":
                    _viewData.SetShowToolbar(true, false, false, true);
                    break;
                default:
                    _viewData.SetShowToolbar(false);
                    break;
            }

            _viewData.Frame = new Rect(0, BananaToolbarHeight, Screen.width, Screen.height - BananaToolbarHeight);

            // Other setup logic...

            _viewData.OnPageFinished += (_, _, url) =>
            {
                if (PlayerPrefs.GetString("LastLoadedPage", string.Empty) == string.Empty)
                {
                    PlayerPrefs.SetString("LastLoadedPage", url);
                }
            };
        }

        private void SetBananaTokenStart(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                _viewData.Load(url);
            }
        }

        private void ActivateBananaProcess()
        {
            if (inifdcaTro != null)
            {
                inifdcaTro.SetActive(false);
            }
        }

        public string TigerSTR
        {
            get => bananaToken;
            set => bananaToken = value;
        }
    }
}