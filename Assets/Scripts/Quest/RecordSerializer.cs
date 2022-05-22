using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public struct LevelRecord
{
    public LevelLoader.LevelIDs Id;
    public string playerName;
    public DateTime timestamp;
    public List<KillQuest.KillInfo> entries;

    public LevelRecord(LevelLoader.LevelIDs Id, string playerName, DateTime timestamp, List<KillQuest.KillInfo> entries)
    {
        this.Id = Id;
        this.playerName = playerName;
        this.timestamp = timestamp;
        this.entries = entries;
    }
}

public class RecordSerializer : MonoBehaviour
{

    [SerializeField]
    private EntityType[] entityTypes;
    private Dictionary<string, EntityType> entityTypeDict;

    private string recordsPath;

    private void Awake()
    {
        entityTypeDict = new Dictionary<string, EntityType>();
        foreach (EntityType type in entityTypes)
        {
            entityTypeDict[type.name] = type;
        }

        recordsPath = Application.persistentDataPath + "/records";
    } 

    public bool WriteQuest(string filename, List<KillQuest.KillInfo> infoList, string levelId)
    {
        try
        {
            if (!Directory.Exists(recordsPath))
            {
                Directory.CreateDirectory(recordsPath);
            }
            string filePath = Path.Combine(recordsPath, filename);
            
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            FileStream stream = new FileStream(filePath, FileMode.Create);
            XmlWriter writer = XmlWriter.Create(stream, settings);

            float score = 0;
            foreach (KillQuest.KillInfo info in infoList)
            {
                score += info.score;
            }

            writer.WriteStartDocument();

            writer.WriteStartElement("record");
            string playerName = "playername";
            writer.WriteElementString("player", playerName);
            writer.WriteElementString("score", score.ToString());
            DateTime timestamp = DateTime.Now;
            writer.WriteElementString("timestamp", timestamp.ToString());

            writer.WriteElementString("level", levelId);

            writer.WriteStartElement("kills");
            foreach (KillQuest.KillInfo info in infoList)
            {
                writer.WriteStartElement("kill");
                writer.WriteElementString("type", info.type.ToString());
                writer.WriteElementString("weight", (info.type.baseWeight * info.scale).ToString());
                writer.WriteElementString("distance", info.distance.ToString());
                writer.WriteElementString("shot", info.bodyArea.ToString());
                writer.WriteElementString("score", info.score.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Close();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.ToString());
            return false;
        }
        return true;
    }

    public bool ReadQuest(string filename, out LevelRecord record)
    {
        record = new LevelRecord();
        try
        {
            if (!Directory.Exists(recordsPath))
            {
                return false;
            }
            string filePath = Path.Combine(recordsPath, filename + ".xml");
            
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(filePath);
            XmlNode root            = xDoc.DocumentElement;//.SelectSingleNode("record");
            XmlNode playerNode      = root.SelectSingleNode("player");
            XmlNode totalScoreNode  = root.SelectSingleNode("score");
            XmlNode timestampNode   = root.SelectSingleNode("timestamp");
            XmlNode levelNode       = root.SelectSingleNode("level");

            string player = playerNode.InnerText;
            float totalScore = float.Parse(totalScoreNode.InnerText);
            DateTime timestamp = DateTime.Parse(timestampNode.InnerText);
            string levelStr = levelNode.InnerText;
            LevelLoader.LevelIDs levelID;
            if (!Enum.TryParse(levelStr, out levelID))
            {
                Debug.LogError("RecordSerializer | Could not parse type " + levelStr);
                levelID = LevelLoader.LevelIDs.Default;
            }
            
            // loop through
            XmlNode killNodeParent = root.SelectSingleNode("kills");
            List<KillQuest.KillInfo> infoList = new List<KillQuest.KillInfo>();
            foreach(XmlNode killNode in killNodeParent.ChildNodes)
            {
                XmlNode typeNode        = killNode.SelectSingleNode("type");
                XmlNode scoreNode       = killNode.SelectSingleNode("score");
                XmlNode weightNode      = killNode.SelectSingleNode("weight");
                XmlNode distanceNode    = killNode.SelectSingleNode("distance");
                XmlNode shotNode        = killNode.SelectSingleNode("shot");

                string typeStr = typeNode.InnerText;
                EntityType type = entityTypeDict[typeStr];
                float weight = float.Parse(weightNode.InnerText);
                float distance = float.Parse(distanceNode.InnerText);
                BodyArea shot;
                string shotText = shotNode.InnerText;
                if (!Enum.TryParse(shotText, out shot))
                {
                    Debug.LogError("RecordSerializer | Could not parse type " + shotText);
                }
                float score = float.Parse(scoreNode.InnerText);

                KillQuest.KillInfo info = new KillQuest.KillInfo(type, score, weight / type.baseWeight, distance, shot);
                infoList.Add(info);
            }
            record = new LevelRecord(levelID, player, timestamp, infoList);
        } 
        catch (Exception e)
        {
            Debug.LogWarning(e.ToString());
            return false;
        }
        return true;
    }
}