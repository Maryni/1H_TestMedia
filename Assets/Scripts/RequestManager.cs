using UnityEngine;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using System;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class RequestManager : MonoBehaviour
{
    private HttpClient _client;
    [SerializeField] private Data data;

    [SerializeField] private string url;
    [SerializeField] private string login;
    [SerializeField] private string password;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private List<Texture> images = new List<Texture>();


    public void Start()
    {
        data = new Data();
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(login, password);
    }

    public async Task<string> GetRequest()
    {
        var response = await _client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
        else
        {
            throw new Exception("Request failed with status code " + response.StatusCode);
        }
    }

    public async void Load()
    {
        var jsonData = await GetRequest();
        data = JsonUtility.FromJson<Data>(jsonData);
        DownloadImage("/fiVW06jE7z9YnO4trhaMEdclSiC.jpg");


        Debug.Log($"jsonData = {jsonData}");
        Debug.Log($"data = {data}");
        //Debug.Log($"count results = {data.results.Count}");

        uiManager.SetValue(data, images);

    }
    IEnumerator DownloadImage(string mediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture("https://image.tmdb.org/t/p/w500"+mediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            images.Add(((DownloadHandlerTexture)request.downloadHandler).texture);
    }
    
}