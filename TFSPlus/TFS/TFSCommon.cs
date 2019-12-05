using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Xml;
using System.Diagnostics;


namespace TFSPlus.TFS
{
    public abstract class TFSCommon
    {
        public TfsTeamProjectCollection TFSTeamProject { get; set; }
        public WorkItemStore ItemStore { get; set; }
        public Project RecentProject { get; set; }
        public WorkItem Item { get; set; }
        public String QueryConditionFromWiq { get; set; }

        public bool ReadQuery(string queryPath)
        {
            XmlDocument queryxml = new XmlDocument();
            try
            {
                queryxml.Load(queryPath);
            }
            catch (System.IO.FileLoadException)
            {
                return false;
            }

            XmlNode top = queryxml.SelectSingleNode("WorkItemQuery");
            XmlNode server = top.SelectSingleNode("TeamFoundationServer");
            XmlNode project = top.SelectSingleNode("TeamProject");
            XmlNode wiql = top.SelectSingleNode("Wiql");
            QueryConditionFromWiq = wiql.InnerText.Replace("@project",String.Format("'{0}'",project.InnerText));

            return Init(server.InnerText, project.InnerText);
        }

        public bool Init()
        {
            try
            {
                TFSTeamProject = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(@"http://tfs:8080/tfs/defaultcollection"));
                ItemStore = TFSTeamProject.GetService<WorkItemStore>();
                RecentProject = ItemStore.Projects["XR_LouTang"];
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Init(string tfsad, string projectname)
        {
            try
            {
                TFSTeamProject = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(tfsad));
                ItemStore = TFSTeamProject.GetService<WorkItemStore>();
                RecentProject = ItemStore.Projects[projectname];
                return true;
            }
            catch
            {
                return false;
            }
        }

        public abstract bool Operate();

        public void Finish()
        { 
        }

        public void Link(int parent, int child, String linkType)
        {
            try
            {
                Console.WriteLine(String.Format("正在关联{0}--{1}",parent,child));
                WorkItemLinkTypeEnd linkTypeEnd = ItemStore.WorkItemLinkTypes.LinkTypeEnds[linkType];
                Item = ItemStore.GetWorkItem(parent);
                Item.Links.Add(new RelatedLink(linkTypeEnd, child));
                Item.Save();
            }
            catch
            {
                Console.WriteLine(String.Format("{0}--{1}关联已存在", parent, child));
            }

        }
        
    }
}
