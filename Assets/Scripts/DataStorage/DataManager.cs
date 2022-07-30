using Assets.Scripts.UI.Game.Managers;
using Board;
using DataStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using UnityEngine;

namespace Assets.Scripts.DataStorage
{
    public static class DataManager
    {
        public static bool Save(string fileName)
        {
            fileName = Application.persistentDataPath + "/" + fileName + ".json";

            try
            {
                Dictionary<string, object> fieldNames = GetFields();

                object[,] objects = new object[fieldNames.Count, 2];
                int i = 0;

                foreach (var fieldName in fieldNames)
                {
                    objects[i, 0] = fieldName.Key;
                    objects[i, 1] = fieldName.Value;
                    i++;
                }

                Stream fileStream = File.Open(fileName, FileMode.Create);
                SoapFormatter formatter = new SoapFormatter();
                formatter.Serialize(fileStream, objects);
                fileStream.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                return false;
            }
        }

        //public static bool Load(string fileName)
        //{
        //    fileName = Application.persistentDataPath + "/" + fileName + ".json";

            //try
            //{
            //    FieldInfo[] fields = static_class.GetFields(BindingFlags.Static | BindingFlags.Public);
            //    object[,] a;
            //    Stream f = File.Open(filename, FileMode.Open);
            //    SoapFormatter formatter = new SoapFormatter();
            //    a = formatter.Deserialize(f) as object[,];
            //    f.Close();
            //    if (a.GetLength(0) != fields.Length) return false;
            //    int i = 0;
            //    foreach (FieldInfo field in fields)
            //    {
            //        if (field.Name == (a[i, 0] as string))
            //        {
            //            field.SetValue(null, a[i, 1]);
            //        }
            //        i++;
            //    };
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        //}

        private static Dictionary<string, object> GetFields()
        {
            Dictionary<string, object> fields = new();

            fields.Add((typeof(GameManager)).GetField("Players").Name, GameManager.Players);
            fields.Add((typeof(GameManager)).GetField("CurrentPlayer").Name, GameManager.CurrentPlayer);
            fields.Add((typeof(GameManager)).GetField("CurrentDiceThrownNumber").Name, GameManager.CurrentDiceThrownNumber);
            fields.Add((typeof(GameManager)).GetField("Mode").Name, GameManager.Mode);
            fields.Add((typeof(GameManager)).GetField("SwitchingGameMode").Name, GameManager.SwitchingGameMode);
            fields.Add((typeof(GameManager)).GetField("MovingUserMode").Name, GameManager.MovingUserMode);
            fields.Add((typeof(GameManager)).GetField("BasicMovingUserMode").Name, GameManager.BasicMovingUserMode);

            fields.Add((typeof(CardsManager)).GetField("Deck").Name, GameManager.CardsManager.Deck);

            fields.Add((typeof(BoardManager)).GetField("OwnerChangeRequest").Name, BoardManager.OwnerChangeRequest);

            //fields.Add((typeof(BoardManager)).GetField("Fields").Name, BoardManager.Fields);
            //fields.Add((typeof(BoardManager)).GetField("Junctions").Name, BoardManager.Junctions);
            //fields.Add((typeof(BoardManager)).GetField("Paths").Name, BoardManager.Paths);

            return fields;
        }
    }
}

