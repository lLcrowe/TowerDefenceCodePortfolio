using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace lLCroweTool.QC.EditorOnly
{

    public static class DataCSVEditor
    {
        /// <summary>
        ///데이터나오는게 딕셔너리중첩2형식
        ///키 => 라인(줄)
        ///값 => 해당라인의 데이터들 Header와 해당라인의 값
        /// </summary>
        public static class CSVReader
        {
            static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
            static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
            static char[] TRIM_CHARS = { '\"' };

            /// <summary>
            /// 리소스폴더에서 CSV읽기
            /// </summary>
            /// <param name="path">/포함해서 경로</param>
            /// <param name="fileName">파일이름</param>
            /// <returns></returns>
            public static List<Dictionary<string, object>> Read(string path, string fileName, ref bool isExistData)
            {
                var list = new List<Dictionary<string, object>>();
                TextAsset data = Resources.Load(path + fileName) as TextAsset;
                if (data == null)
                {
                    Debug.Log("데이터가 비었습니다");
                    isExistData = false;
                    return list;
                }
                isExistData = true;

                var lines = Regex.Split(data.text, LINE_SPLIT_RE);

                if (lines.Length <= 1) return list;


                var header = Regex.Split(lines[0], SPLIT_RE);//변수명
                for (var i = 1; i < lines.Length; i++)
                {

                    var values = Regex.Split(lines[i], SPLIT_RE);
                    if (values.Length == 0 || values[0] == "") continue;

                    var entry = new Dictionary<string, object>();
                    for (var j = 0; j < header.Length && j < values.Length; j++)
                    {
                        string value = values[j];
                        value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                        object finalvalue = value;
                        int n;
                        float f;
                        if (int.TryParse(value, out n))
                        {
                            finalvalue = n;
                        }
                        else if (float.TryParse(value, out f))
                        {
                            finalvalue = f;
                        }
                        entry[header[j]] = finalvalue;
                    }
                    list.Add(entry);
                }
                return list;
            }

            public static List<Dictionary<string, object>> Read(string path, string fileName, string extend)
            {
                var list = new List<Dictionary<string, object>>();
                TextAsset data = (TextAsset)AssetDatabase.LoadAssetAtPath(path + "/" + fileName, typeof(TextAsset));
                if (data == null)
                {
                    Debug.Log("데이터가 비었습니다");
                    return list;
                }


                var lines = Regex.Split(data.text, LINE_SPLIT_RE);

                if (lines.Length <= 1) return list;

                var header = Regex.Split(lines[0], SPLIT_RE);
                for (var i = 1; i < lines.Length; i++)
                {

                    var values = Regex.Split(lines[i], SPLIT_RE);
                    if (values.Length == 0 || values[0] == "") continue;

                    var entry = new Dictionary<string, object>();
                    for (var j = 0; j < header.Length && j < values.Length; j++)
                    {
                        string value = values[j];
                        value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                        object finalvalue = value;
                        int n;
                        float f;
                        if (int.TryParse(value, out n))
                        {
                            finalvalue = n;
                        }
                        else if (float.TryParse(value, out f))
                        {
                            finalvalue = f;
                        }
                        entry[header[j]] = finalvalue;
                    }
                    list.Add(entry);
                }
                return list;
            }


            public static List<Dictionary<string, object>> Read(TextAsset textFile)
            {
                var list = new List<Dictionary<string, object>>();
                TextAsset data = textFile;
                if (data == null)
                {
                    Debug.Log("데이터가 비었습니다");
                    return list;
                }


                var lines = Regex.Split(data.text, LINE_SPLIT_RE);

                if (lines.Length <= 1) return list;

                var header = Regex.Split(lines[0], SPLIT_RE);
                for (var i = 1; i < lines.Length; i++)
                {

                    var values = Regex.Split(lines[i], SPLIT_RE);
                    if (values.Length == 0 || values[0] == "") continue;

                    var entry = new Dictionary<string, object>();
                    for (var j = 0; j < header.Length && j < values.Length; j++)
                    {
                        string value = values[j];
                        value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                        object finalvalue = value;
                        int n;
                        float f;
                        if (int.TryParse(value, out n))
                        {
                            finalvalue = n;
                        }
                        else if (float.TryParse(value, out f))
                        {
                            finalvalue = f;
                        }
                        entry[header[j]] = finalvalue;
                    }
                    list.Add(entry);
                }
                return list;
            }
        }

        public static class CSVWritter
        {
            static StringBuilder stringBuilder = new StringBuilder();
            public static void WriteCsv(List<string[]> lineData, string filePath, string fileName)
            {
                string[][] output = new string[lineData.Count][];

                for (int i = 0; i < output.Length; i++)
                {
                    output[i] = lineData[i];
                }

                int length = output.GetLength(0);
                string delimiter = ",";

                for (int index = 0; index < length; index++)
                    stringBuilder.AppendLine(string.Join(delimiter, output[index]));

                //Stream fileStream = new FileStream(filePath + "/" + fileName + ".csv", FileMode.CreateNew, FileAccess.Write);            
                //자동으로 UTF-8로 나옴//맞는지 의문//알아서 UTF-8로 나옴
                StreamWriter writer = File.CreateText(filePath + "/" + fileName + ".csv");
                writer.WriteLine(stringBuilder);
                writer.Dispose();
                writer.Close();
                AssetDatabase.Refresh();
            }
        }
    }
}