using Assets.Scripts.Board.States;
using Assets.Scripts.DataStorage;
using Board;
using Board.States;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Soap;
using UnityEngine;
using static Player.Cards;

namespace DataStorage
{
    public static class DataManager
    {
        public static FieldState[] FieldStates = new FieldState[BoardManager.FieldsNumber];
        public static JunctionState[] JunctionStates = new JunctionState[BoardManager.JunctionsNumber];
        public static PathState[] PathStates = new PathState[BoardManager.PathsNumber];
        public static int MAX_SLOT_NUMBER = 5;

        private const string SAVE_NAME = "SaveName";
        private const string SLOT_NUMBER = "SlotNumber";
        private const string OWNER_CHANGE_REQUEST = "OwnerChangeRequest";
        private const string DECK = "Deck";
        private const string STATE = "State";
        private const string FIELD_STATES = "FieldStates";
        private const string JUNCTION_STATES = "JunctionStates";
        private const string PATH_STATES = "PathStates";
        private static string fileDirectory = $"{Application.persistentDataPath}";

        public static bool Save(int saveSlotNumber, string saveName)
        {
            string fileName = GetFileName(saveSlotNumber);
            
            try
            {
                Dictionary<string, object> fieldNames = GetFields(saveName, saveSlotNumber);

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

        public static bool Load()
        {
            string fileName = GetFileName(GameManager.LoadSlotNumber);

            try
            {
                object[,] objects;
                Stream f = File.Open(fileName, FileMode.Open);
                SoapFormatter formatter = new SoapFormatter();
                objects = formatter.Deserialize(f) as object[,];
                f.Close();

                for (int i = 0; i < objects.Length/2; i++)
                {
                    switch(objects[i, 0] as string)
                    {
                        case OWNER_CHANGE_REQUEST:
                            foreach (var ownerChangeRequest in (List<OwnerChangeRequest>) objects[i, 1])
                                BoardManager.OwnerChangeRequest.Add(ownerChangeRequest);
                            break;
                        case DECK:
                            GameManager.CardsManager.Deck = (List<CardType>)objects[i, 1];
                            break;
                        case STATE:
                            GameManager.State.SetState((GameState)objects[i, 1]);
                            break;
                        case FIELD_STATES:
                            var fieldStates = (FieldState[])objects[i, 1];
                            for (int j = 0; j < BoardManager.FieldsNumber; j++)
                                FieldStates[j] = fieldStates[j];
                            break;
                        case JUNCTION_STATES:
                            var junctionStates = (JunctionState[])objects[i, 1];
                            for (int j = 0; j < BoardManager.JunctionsNumber; j++)
                                JunctionStates[j] = junctionStates[j];
                            break;
                        case PATH_STATES:
                            var pathStates = (PathState[])objects[i, 1];
                            for (int j = 0; j < BoardManager.PathsNumber; j++)
                                PathStates[j] = pathStates[j];
                            break;
                    }
                };

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>List of files' data of saved games</returns>
        public static List<FileData> GetFiles()
        {
            List<FileData> files = new();
            
            var filesFromDirectory = Directory.GetFiles(fileDirectory);
            for (int i = 0; i < MAX_SLOT_NUMBER; i++)
            {
                var fileDirectory = filesFromDirectory.Where(file => file.Contains($"game{i}.save")).FirstOrDefault();
                if (fileDirectory != null)
                {
                    try
                    {
                        FileData fileData = new FileData();
                        fileData.CreationDate = File.GetCreationTime(fileDirectory);

                        object[,] objects;
                        Stream f = File.Open(fileDirectory, FileMode.Open);
                        SoapFormatter formatter = new SoapFormatter();
                        objects = formatter.Deserialize(f) as object[,];
                        f.Close();

                        for (int j = 0; j < objects.Length / 2; j++)
                        {
                            switch (objects[j, 0] as string)
                            {
                                case SLOT_NUMBER:
                                    fileData.SlotNumber = (int)objects[j, 1];
                                    break;
                                case SAVE_NAME:
                                    fileData.Name = (string)objects[j, 1];
                                    break;
                            }
                        };

                        files.Add(fileData);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                    }
                }
            }

            return files;
        }

        private static string GetFileName(int slotNumber)
        {
            return $"{fileDirectory}/game{slotNumber}.save";
        }

        private static Dictionary<string, object> GetFields(string saveName, int slotNumber)
        {
            Dictionary<string, object> fields = new();

            fields.Add(SAVE_NAME, saveName);
            fields.Add(SLOT_NUMBER, slotNumber);
            fields.Add(OWNER_CHANGE_REQUEST, BoardManager.OwnerChangeRequest);
            fields.Add(DECK, GameManager.CardsManager.Deck);
            fields.Add(STATE, GameManager.State);

            FieldState[] fieldStates = new FieldState[BoardManager.FieldsNumber];
            for (int i = 0; i < BoardManager.FieldsNumber; i++)
                fieldStates[i] = (FieldState)BoardManager.Fields[i].State;
            fields.Add(FIELD_STATES, fieldStates);

            JunctionState[] junctionStates = new JunctionState[BoardManager.JunctionsNumber];
            for (int i = 0; i < BoardManager.JunctionsNumber; i++)
                junctionStates[i] = (JunctionState)BoardManager.Junctions[i].State;
            fields.Add(JUNCTION_STATES, junctionStates);

            PathState[] pathStates = new PathState[BoardManager.PathsNumber];
            for (int i = 0; i < BoardManager.PathsNumber; i++)
                pathStates[i] = (PathState)BoardManager.Paths[i].State;
            fields.Add(PATH_STATES, pathStates);

            return fields;
        }
    }
}

