using System.Collections.Generic;
using System.Linq;
using Proyecto26;
using SimpleJSON;
using UnityEngine;

public class FirebaseRankingManager : MonoBehaviour
{
    #region Secret
    public const string url = "https://portal-strike-default-rtdb.asia-southeast1.firebasedatabase.app/";
    public const string secret = "yJ2cZA1WjQOkhRlAZdwdjpZVCFN1rONkxmeVSqVF";
    #endregion

    [Header("Main")]
    public RankUIManager rankUIManager;
    private List<PlayerData> sortPlayerDatas;

    [System.Serializable]
    public class Ranking
    {
        public List<PlayerData> playerDatas = new List<PlayerData>();
    }

    public Ranking ranking;

    // -------------------------------------

    [Header("New Data")]
    public PlayerData currentPlayerData;

    // -------------------------------------

    [Header("Test")]
    public int testNum;

    [System.Serializable]
    public class TestData
    {
        public int num = 1;
        public string name = "name";
    }

    public TestData testData;

    // -------------------------------------

    private void Start()
    {
        // 1. Test Calculate Sort
        // DebugSetupWithLocalData();

        // 2. Test Set & Get data
        //TestSetData();
        //TestGetData();
        //TestSetData2();
        //TestGetData2();

        // 3. Test Real case
        // SetLocalDataToDatabase();

        // 4. Real one
        ReloadSortingData();
    }

    public void FindYourDataInRanking()
    {
        rankUIManager.yourRankData.playerData = ranking.playerDatas.Where(data => data.playerName == currentPlayerData.playerName).FirstOrDefault();
        rankUIManager.yourRankData.UpdateData();
    }

    //public void DebugSetupWithLocalData()
    //{
    //    ranking.playerDatas = rankUIManager.playerDatas;
    //    CalculateRankFromScore();
    //}

    public void CalculateRankFromScore()
    {
        sortPlayerDatas = ranking.playerDatas.OrderByDescending(data => data.playerScore).ToList();
        sortPlayerDatas.ForEach(data => data.rankNumber = sortPlayerDatas.IndexOf(data) + 1);
        ranking.playerDatas = sortPlayerDatas;
    }

    //public void TestSetData()
    //{
    //    string urlData = $"{url}/.json?auth={secret}";
    //    RestClient.Put<TestData>(urlData, testData).Then(response =>
    //    {
    //        Debug.Log("Upload Data Complete");
    //    }).Catch(error =>
    //    {
    //        Debug.Log("Error on set to server");
    //    });
    //}

    //public void TestGetData()
    //{
    //    string urlData = $"{url}/.json?auth={secret}";
    //    RestClient.Get(urlData).Then(ResponseHelper =>
    //    {
    //        Debug.Log(ResponseHelper.Text);
    //        JSONNode jsonNode = JSONNode.Parse(ResponseHelper.Text);

    //        testNum = jsonNode["num"];
    //    }).Catch(error =>
    //    {
    //        Debug.Log("Error to get data");
    //    });
    //}

    //public void TestSetData2()
    //{
    //    string urlData = $"{url}/TestData.json?auth={secret}";
    //    RestClient.Put<TestData>(urlData, testData).Then(response =>
    //    {
    //        Debug.Log("Upload Data Complete");
    //    }).Catch(error =>
    //    {
    //        Debug.Log("Error on set to server");
    //    });
    //}

    //public void TestGetData2()
    //{
    //    string urlData = $"{url}/TestData.json?auth={secret}";
    //    RestClient.Get(urlData).Then(ResponseHelper =>
    //    {
    //        Debug.Log(ResponseHelper.Text);
    //        JSONNode jsonNode = JSONNode.Parse(ResponseHelper.Text);

    //        testNum = jsonNode["num"];
    //    }).Catch(error =>
    //    {
    //        Debug.Log("Error to get data");
    //    });
    //}

    //public void SetLocalDataToDatabase()
    //{
    //    string urlData = $"{url}/ranking.json?auth={secret}";
    //    RestClient.Put<Ranking>(urlData, ranking).Then(response =>
    //    {
    //        Debug.Log("Upload Data Complete");
    //    }).Catch(error =>
    //    {
    //        Debug.Log("error on set to server");
    //    });
    //}

    //public void AddData()
    //{
    //    string urlData = $"{url}/ranking/playerDatas.json?auth={secret}";
    //    RestClient.Get(urlData).Then(response =>
    //    {
    //        Debug.Log(response.Text);
    //        JSONNode jsonNode = JSONNode.Parse(response.Text);

    //        string urlPlayerData = $"{url}/ranking/playerDatas/{jsonNode.Count}.json?auth={secret}";

    //        RestClient.Put<PlayerData>(urlPlayerData, currentPlayerData).Then(response =>
    //        {
    //            Debug.Log("Upload Data Complete");
    //        }).Catch(error =>
    //        {
    //            Debug.Log("error on set to server");
    //        });
    //    }).Catch(error =>
    //    {
    //        Debug.Log("error");
    //    });
    //}

    public void ReloadSortingData()
    {
        string urlData = $"{url}/ranking/playerDatas.json?auth={secret}";
        RestClient.Get(urlData).Then(response =>
        {
            Debug.Log(response.Text);
            JSONNode jsonNode = JSONNode.Parse(response.Text);

            ranking = new Ranking();
            ranking.playerDatas = new List<PlayerData>();
            for (int i = 0; i < jsonNode.Count; i++)
            {
                ranking.playerDatas.Add(new PlayerData(jsonNode[i]["rankNumber"], jsonNode[i]["playerName"], jsonNode[i]["playerScore"]));
            }
            CalculateRankFromScore();

            string urlPlayerData = $"{url}/ranking.json?auth={secret}";

            RestClient.Put<Ranking>(urlPlayerData, ranking).Then(response =>
           {
               Debug.Log("Upload Data Conplete");
               rankUIManager.playerDatas = ranking.playerDatas;
               rankUIManager.ReloadRankData();
               FindYourDataInRanking();

           }).Catch(error =>
           {
               Debug.Log("error on set to server");
           });
        }).Catch(error =>
        {
            Debug.Log("error");
        });
    }

    public void AddDataWithSorting()
    {
        string urlData = $"{url}/ranking/playerDatas.json?auth={secret}";
        RestClient.Get(urlData).Then(response =>
        {
            Debug.Log(response.Text);
            JSONNode jsonNode = JSONNode.Parse(response.Text);

            ranking = new Ranking();
            ranking.playerDatas = new List<PlayerData>();
            for (int i = 0; i < jsonNode.Count; i++)
            {
                ranking.playerDatas.Add(new PlayerData(jsonNode[i]["rankNumber"], jsonNode[i]["playerName"], jsonNode[i]["playerScore"]));
            }

            PlayerData checkPlayerData = ranking.playerDatas.FirstOrDefault(data => data.playerName == currentPlayerData.playerName);
            if (checkPlayerData != null)
            {
                checkPlayerData.playerScore = currentPlayerData.playerScore;
            }
            else
            {
                ranking.playerDatas.Add(currentPlayerData);
            }

            CalculateRankFromScore();

            string urlPlayerData = $"{url}/ranking/.json?auth={secret}";

            RestClient.Put<PlayerData>(urlPlayerData, ranking).Then(response =>
            {
                Debug.Log("Upload Data Complete");
                rankUIManager.playerDatas = ranking.playerDatas;
                rankUIManager.ReloadRankData();
            }).Catch(error =>
            {
                Debug.Log("error on set to server");
            });
        }).Catch(error =>
        {
            Debug.Log("error");
        });
    }
}