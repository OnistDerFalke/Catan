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
                Dictionary<string, Type> fieldNames = GetFieldNames();

                object[,] objects = new object[fieldNames.Count, 2];
                int i = 0;

                foreach (var fieldName in fieldNames)
                {
                    objects[i, 0] = fieldName.Value.GetField(fieldName.Key).Name;
                    objects[i, 1] = fieldName.Value.GetField(fieldName.Key).GetValue(null);
                    i++;
                }

                Stream fileStream = File.Open(fileName, FileMode.Create);
                SoapFormatter formatter = new SoapFormatter();
                formatter.Serialize(fileStream, objects);
                fileStream.Close();
                return true;
            }
            catch
            {
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

        private static Dictionary<string, Type> GetFieldNames()
        {
            Dictionary<string, Type> fieldNames = new();

            fieldNames.Add("Players", typeof(GameManager));
            fieldNames.Add("CurrentPlayer", typeof(GameManager));
            fieldNames.Add("CurrentDiceThrownNumber", typeof(GameManager));
            fieldNames.Add("Mode", typeof(GameManager));
            fieldNames.Add("SwitchingGameMode", typeof(GameManager));
            fieldNames.Add("MovingUserMode", typeof(GameManager));
            fieldNames.Add("BasicMovingUserMode", typeof(GameManager));

            //fieldNames.Add("Deck", typeof(CardsManager));

            //fieldNames.Add("OwnerChangeRequest", typeof(BoardManager));

            //fieldNames.Add("Fields", typeof(BoardManager));
            //fieldNames.Add("Junctions", typeof(BoardManager));
            //fieldNames.Add("Paths", typeof(BoardManager));

            return fieldNames;
        }
    }
}

