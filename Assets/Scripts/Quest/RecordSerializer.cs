using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class RecordSerializer
{

    public RecordSerializer()
    {
        
    } 

    public static bool WriteQuest(string filename, List<KillQuest.KillInfo> infoList, string levelId)
    {
        try
        {
            string dirPath = Application.persistentDataPath + "/records";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            string filePath = Path.Combine(dirPath, filename);
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
            writer.WriteStartElement("timestamp");
            DateTime timestamp = DateTime.Now;
            writer.WriteElementString("date", timestamp.ToShortDateString());
            writer.WriteElementString("time", timestamp.ToShortTimeString());
            writer.WriteEndElement();

            writer.WriteElementString("level", levelId);

            writer.WriteStartElement("kills");
            foreach (KillQuest.KillInfo info in infoList)
            {
                writer.WriteStartElement("kill");
                writer.WriteElementString("type", info.type.ToString());
                writer.WriteElementString("weight", (info.type.baseWeight * info.scale).ToString());
                writer.WriteElementString("distance", info.distance.ToString());
                writer.WriteElementString("shot", info.bodyArea.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Close();
        } catch (Exception e)
        {
            Debug.LogWarning(e.ToString());
            return false;
        }
        return true;
    }
}