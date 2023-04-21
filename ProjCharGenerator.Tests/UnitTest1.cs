using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using gen;

namespace NET
{
    [TestClass]
    public class UnitTest1
    {
        bool FileCompare(string file1, string file2)
        {
            string s1 = File.ReadAllText(file1);
            string s2 = File.ReadAllText(file2);
            return s1 == s2;
        }

        [TestMethod]
        public void TestMethod1()
        {
            Weighed_random r = new Weighed_random();
            List<(string, int)> data = new List<(string, int)>
            {
                ("word1", 0),
                ("word2", 0),
                ("word3", 5),
                ("word4", 5)
            };
            string result = r.random_value(data);
            Assert.IsTrue(result == "word3");
        }

        [TestMethod]
        public void TestMethod2()
        {
            Weighed_random r = new Weighed_random();
            List<(string, int)> data = new List<(string, int)>
            {
                ("word1", 17),
                ("word2", 17),
                ("word3", 17),
                ("word4", 17)
            };
            string result = r.random_value(data);
            Assert.IsTrue(result == "word1");
        }

        [TestMethod]
        public void TestMethod3()
        {
            Data_1gram d1g = new Data_1gram("test1.txt");
            List<(string, int)> result = d1g.get_data();
            List<(string, int)> data = new List<(string, int)>
            {
                ("word5", 19),
                ("word2", 36),
                ("word6", 48),
                ("word1", 55),
                ("word3", 59),
                ("word4", 62)
            };

            bool is_equal = data.Count == result.Count;
            for (int i = 0; i < data.Count && is_equal; i++)
            {
                is_equal = data[i].Item1 == result[i].Item1 && data[i].Item2 == result[i].Item2;
            }

            Assert.IsTrue(is_equal);
        }

        [TestMethod]
        public void TestMethod4()
        {
            Data_2gram d2g = new Data_2gram("test2.txt");
            SortedDictionary<string, List<(string, int)>> result = d2g.get_data();
            SortedDictionary<string, List<(string, int)>> data = new SortedDictionary<string, List<(string, int)>>
            {
                { "A", new List<(string, int)> { (" A", 8), (" B", 9) } },
                { "B", new List<(string, int)> { (" A", 16), (" B", 27) } }
            };

            bool is_equal = data.Count == result.Count;
            foreach (KeyValuePair<string, List<(string, int)>> pair in data)
            {
                if(!is_equal)
                {
                    break;
                }

                List<(string, int)> data_value = pair.Value;
                is_equal = result.ContainsKey(pair.Key);
                if (!is_equal)
                {
                    break;
                }

                List<(string, int)> result_value = result[pair.Key];
                is_equal = data_value.Count == result_value.Count;
                for (int i = 0; i < data_value.Count && is_equal; i++)
                {
                    is_equal = data_value[i].Item1 == result_value[i].Item1 && data_value[i].Item2 == result_value[i].Item2;
                }
            }

            Assert.IsTrue(is_equal);
        }

        [TestMethod]
        public void TestMethod5()
        {
            Weighed_random r = new Weighed_random();
            Data_1gram d1g = new Data_1gram("test3.txt");
            string name = "test";
            d1g.generate(10, name, r);

            Assert.IsTrue(FileCompare("test3_.txt", "test.txt"));
        }

        [TestMethod]
        public void TestMethod6()
        {
            Weighed_random r = new Weighed_random();
            Data_1gram d1g = new Data_1gram("test3.txt");
            Data_2gram d2g = new Data_2gram("test4.txt");
            string name = "test";
            d2g.generate(10, name, r, d1g);

            Assert.IsTrue(FileCompare("test4_.txt", "test.txt"));
        }
    }
}
