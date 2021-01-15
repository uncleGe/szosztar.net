using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using szosztar.Data.Interfaces;
using szosztar.Logic.Interfaces;
using szosztar.Models;

namespace szosztar.Logic
{
    public class WordLogic: IWordLogic
    {
        private readonly IDataAccess dataAccess;
        public WordLogic(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task<IList<Word>> ProcessWords()
        {
            return await dataAccess.GetWords();
        }

        public IList<Word> ReturnDummyWords()
        {
            var idBase = 0;
            var wordData = new List<Word>
            {
                new Word
                {
                    id = (idBase+1),
                    english = "First1",
                    hungarian = "Second1",
                },
                new Word
                {
                    id = (idBase+2),
                    english = "First2",
                    hungarian = "Second2",
                },
                new Word
                {
                    id = (idBase+3),
                    english = "First3",
                    hungarian = "Second3",
                },
                new Word
                {
                    id = (idBase+4),
                    english = "First4",
                    hungarian = "Second4",
                }
            };

            return wordData;
        }
    }
    
}
