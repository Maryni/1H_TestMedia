using System;
using UnityEngine;

[System.Serializable]
public class Movie
{
    public bool adult;
    public string backdrop_path;
    public int[] genre_ids;
    public int id;
    public string original_language;
    public string original_title;
    public string overview;
    public float popularity;
    public string poster_path;
    public string release_date;
    public string title;
    public bool video;
    public float vote_average;
    public int vote_count;
}

[System.Serializable]
public class MovieList
{
    public int page;
    public Movie[] results;
}






