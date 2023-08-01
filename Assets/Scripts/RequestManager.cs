using UnityEngine;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;


public enum ChangeType
{
    None,
    Left,
    Right
}

public class RequestManager : MonoBehaviour
{
    private HttpClient _client;

    [SerializeField] private string url;
    [SerializeField] private string login;
    [SerializeField] private string password;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private List<Texture2D> images = new List<Texture2D>();
    [SerializeField] private List<string> imageUrls = new List<string>();
    [SerializeField] private List<string> titles = new List<string>(); 
    [SerializeField] private List<string> overviews = new List<string>();
    private event Action actionOnFinishLoad;

    private string jsonData;
    private int i = 0;

    private Queue<string> imageUrlsQueue = new Queue<string>();


    private void Start()
    {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(login, password);
        actionOnFinishLoad += SetDefautlValue;
    }

    private void OnDisable()
    {
        actionOnFinishLoad -= SetDefautlValue;
    }

    public async void Load()
    {
        if(string.IsNullOrEmpty(jsonData))
        {
            jsonData = await GetRequest();
            LoadData();
            LoadImages();

            StartCoroutine(WaitUntilAllImagesLoaded(actionOnFinishLoad));
        }
    }

    public void Right() => Change(ChangeType.Right);
    public void Left() => Change(ChangeType.Left);

    private void SetDefautlValue()
    {
        uiManager.SetValue(images[i], titles[i], overviews[i]);
    }

    private void Change(ChangeType changeType)
    {
        switch (changeType)
        {
            case ChangeType.Left: 
                if(i > 0)
                    i--;

                uiManager.SetValue(images[i], titles[i], overviews[i]); 
                break;

            case ChangeType.Right:
                if (i < titles.Count)
                    i++;

                uiManager.SetValue(images[i], titles[i], overviews[i]);
                break;

            default: break;
        }
    }


    #region Loads

    private async Task<string> GetRequest()
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

    private IEnumerator WaitUntilAllImagesLoaded(Action actionOnFinishLoad)
    {
        while (images.Count < imageUrls.Count || imageUrls.Count == 0)
        {
            yield return null;
        }

        actionOnFinishLoad?.Invoke();
    }

    private void LoadData()
    {
        MovieList movieList = JsonUtility.FromJson<MovieList>(jsonData);
        foreach (Movie movie in movieList.results)
        {
            titles.Add(movie.title);
            overviews.Add(movie.overview);
            imageUrls.Add(movie.backdrop_path);
        }
    }

    private void LoadImages()
    {
        foreach (string url in imageUrls)
        {
            imageUrlsQueue.Enqueue(url);
        }

        StartCoroutine(DownloadImagesFromQueue());
    }

    private IEnumerator DownloadImagesFromQueue()
    {
        while (imageUrlsQueue.Count > 0)
        {
            string url = imageUrlsQueue.Dequeue(); 
            yield return StartCoroutine(DownloadImage(url)); 
        }
    }

    private IEnumerator DownloadImage(string mediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture("https://image.tmdb.org/t/p/w500"+mediaUrl);
        yield return request.SendWebRequest();
        
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            Texture2D currentTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            images.Add(currentTexture);
        }
    }

    #endregion Loads
}