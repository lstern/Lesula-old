﻿using Lesula.Client.Contracts.Base;
using System.Collections.Generic;
using System.Linq;

public class WordMapper : Mapper
{
    /// <summary>
    /// Maps a single input key/value pair into an intermediate key/value pair
    /// </summary>
    /// <param name="input">input key/pair</param>
    /// <returns>intermediate key/value pair</returns>
    public override List<JobData> Map(JobData input)
    {
        var book = (Book)input;
        var words = (from word in book.Body.Split(new []{' ', '\t'})
                     select (JobData)new Word { Data = word }).ToList();

        return words;
    }
}
