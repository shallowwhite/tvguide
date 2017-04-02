using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Text;


namespace TVGuide.App
{
    class TVGuide
    {
        public TVGuide()
        {
            buildData();
        }

        public List<TVGuideChannel> buildData()
        {
            string currentDate = DateTime.Now.ToShortDateString().Replace('/', '-');
            string xmltvWebSource = "http://www.xmltv.co.uk/asset_xmltv.xml";
            string xmltvSource = currentDate + ".xml";
            
            if (!File.Exists(xmltvSource))
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(xmltvWebSource, xmltvSource);
                }
            }

            List<TVGuideChannel> data = new List<TVGuideChannel>();

            var xmltvDocument = new XmlDocument();
            xmltvDocument.Load(xmltvSource);
            var xmltvProgrammes = xmltvDocument.GetElementsByTagName("programme");
            var xmltvChannels = xmltvDocument.GetElementsByTagName("channel");

            for (int i = 0; i < xmltvChannels.Count; i++)
            {
                string channelID = xmltvChannels[i].Attributes["id"].Value;

                var channel = new TVGuideChannel();
                channel.Name = xmltvChannels[i]["display-name"].InnerText;

                //for (int j = 0; j < xmltvProgrammes.Count; j++)
                //{
                //    string programChannelID = xmltvProgrammes[j].Attributes["channel"].Value;

                //    if (channelID == programChannelID)
                //    {
                //        var program = new TVGuideChannelProgram();
                //        program.Title = xmltvProgrammes[j]["title"].Value;

                //        channel.Programs.Add(program);
                //    }
                //}
                data.Add(channel);         
            }         
            return data;
        }
        
    }
}
