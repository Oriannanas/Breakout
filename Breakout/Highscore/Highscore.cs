using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
namespace Breakout
{
    public class Highscore
    {
        private string fileName = "Highscore.lst";

        public Score[] scoreList { get; private set; } = new Score[10];

        public Highscore()
        {
            if (File.Exists(fileName))
            {
                Stream TestFileStream = File.OpenRead(fileName);
                BinaryFormatter deserializer = new BinaryFormatter();
                scoreList = (Score[])deserializer.Deserialize(TestFileStream);
                TestFileStream.Close();
            }
            else
            {
                scoreList[0] = new Score("Empty", 0);
                scoreList[1] = new Score("Empty", 0);
                scoreList[2] = new Score("Empty", 0);
                scoreList[3] = new Score("Empty", 0);
                scoreList[4] = new Score("Empty", 0);
                scoreList[5] = new Score("Empty", 0);
                scoreList[6] = new Score("Empty", 0);
                scoreList[7] = new Score("Empty", 0);
                scoreList[8] = new Score("Empty", 0);
                scoreList[9] = new Score("Empty", 0);
                SubmitHighscore();
            }

        }

        public void AddScore(string name, int value)
        {
            for(int i = scoreList.Length -1; i >= 0; i--)
            {
                if(value > scoreList[i].value)
                {
                    if(i < 9)
                    {
                        scoreList[i + 1] = scoreList[i];
                    }
                    scoreList[i] = new Score(name, value);
                }
            }
            SubmitHighscore();
        }

        private void SubmitHighscore()
        {
            Stream TestFileStream = File.Create(fileName);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(TestFileStream, scoreList);
            TestFileStream.Close();
        }

    }
}
