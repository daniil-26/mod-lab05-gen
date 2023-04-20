using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace gen
{
    class Program
    {
        public class Data_1gram
        {
            List<(string, int)> data = new List<(string, int)>();

            public Data_1gram(string file)
            {
                int commulative_weight = 0;
                StreamReader f = new StreamReader(file);
                while(!f.EndOfStream)
                {
                    string s = f.ReadLine();
                    string[] values = s.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    commulative_weight += int.Parse(values[0]);
                    string word = values[1];
                    data.Add((word, commulative_weight));
                }
            }

            public List<(string, int)> get_data()
            {
                return data;
            }

            public void generate(int n, string name, Weighed_random r)
            {
                StreamWriter writer = new StreamWriter(name + ".txt");
                for (int i = 0; i < n; i++)
                {
                    writer.Write(r.random_value(data) + ' ');
                }
                writer.Close();
            }
        }

        public class Data_2gram
        {
            SortedDictionary<string, List<(string, int)>> data = new SortedDictionary<string, List<(string, int)>>();

            public Data_2gram(string file)
            {
                StreamReader f = new StreamReader(file);
                while (!f.EndOfStream)
                {
                    string s = f.ReadLine();
                    string[] values = s.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    int weight = int.Parse(values[0]);
                    string key = values[1];
                    string word = values.Length == 3 ? ' ' + values[2] : values[2] + ' ' + values[3];

                    if (data.ContainsKey(key))
                    {
                        data[key].Add((word, data[key].Last().Item2 + weight));
                    }
                    else
                    {
                        data.Add(key, new List<(string, int)>());
                        data[key].Add((word, weight));
                    }
                }
            }

            public SortedDictionary<string, List<(string, int)>> get_data()
            {
                return data;
            }

            public void generate(int n, string name, Weighed_random r, Data_1gram d1g)
            {
                StreamWriter writer = new StreamWriter(name + ".txt");
                string word = r.random_value(d1g.get_data());
                writer.Write(word);
                for (int i = 1; i < n; i++)
                {
                    word = word.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Last();
                    while (!data.ContainsKey(word))
                    {
                        word = r.random_value(d1g.get_data());
                    }
                    word = r.random_value(data[word]);
                    writer.Write(word);
                }
                writer.Close();
            }
        }

        public class Weighed_random
        {
            Random rnd = new Random();

            public string random_value(List<(string, int)> data)
            {
                string value = data.First().Item1;
                int k = rnd.Next(data.Last().Item2);
                foreach ((string, int) x in data)
                {
                    if (x.Item2 > k)
                    {
                        value = x.Item1;
                        break;
                    }
                }
                return value;
            }
        }
        

        static void Main()
        {
            Data_1gram letters_1gram = new Data_1gram("letters_1gram.txt");
            Data_2gram letters_2gram = new Data_2gram("letters_2gram.txt");
            Data_1gram words_1gram = new Data_1gram("words_1gram.txt");
            Data_2gram words_2gram = new Data_2gram("words_2gram.txt");

            Weighed_random r = new Weighed_random();

            letters_2gram.generate(1000, "task 1", r, letters_1gram);
            words_1gram.generate(1000, "task 2", r);
            words_2gram.generate(1000, "task 3", r, words_1gram);
        }
    }
}
