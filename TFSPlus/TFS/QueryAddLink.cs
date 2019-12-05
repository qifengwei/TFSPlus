using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Text.RegularExpressions;

namespace TFSPlus.TFS
{
    public class QueryAddLink:TFSCommon
    {
        public WorkItemCollection SearchResult { get; set; }

        public override bool Operate()
        {
            try
            {
                SearchResult = ItemStore.Query(CorrectQuery());
                Dictionary<string,int> idWithTitle = IDWithTitle(SearchResult);
                CreatLink(idWithTitle);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public String CorrectQuery()
        {
            String correctQuery = QueryConditionFromWiq.Replace("@me", String.Format("'{0}'",TFSTeamProject.AuthorizedIdentity.DisplayName));
            return correctQuery;
        }

        public Dictionary<string, int> IDWithTitle(WorkItemCollection items)
        {
            Dictionary<string, int> idWithTitle = new Dictionary<string, int>();
            foreach (WorkItem item in items)
            {
                string key = UC(item.Title);
                if (key != null && key != "")
                {
                    if (!idWithTitle.ContainsKey(key))
                    {
                        try
                        {
                            idWithTitle.Add(key, item.Id);
                        }
                        catch (ArgumentNullException)
                        {
                            Console.WriteLine("Key为Null");
                        }
                        catch (ArgumentException)
                        {
                            Console.WriteLine("存在两个UC编号一样的uc");
                        }
                        
                    }
                } 
            }
            return idWithTitle;
        }

        public String UC(String title)
        {
            return Regex.Match(title,@"UC-\d+").Value;
        }

        public void CreatLink(Dictionary<string,int> child)
        {
            foreach (WorkItem item in SearchResult)
            {
                String pattern = @"UC-\d+";
                Match match = Regex.Match(item.Description, pattern, RegexOptions.IgnoreCase);
                if (match.Value!= "")
                {
                    if (child.ContainsKey(match.Value))
                    {
                        base.Link(item.Id, child[match.Value], "Shared Steps");
                    }
                    else 
                    {
                        String tempQuery = String.Format("SELECT [System.ID] FROM WorkItems WHERE [System.TeamProject] = '{0}' AND [System.Title] CONTAINS '{1}'", RecentProject.Name,match.Value);
                        WorkItemCollection eItems = ItemStore.Query(tempQuery);
                        foreach (WorkItem eItem in eItems)
                        {
                            if (UC(eItem.Title) == match.Value) base.Link(item.Id, eItem.Id, "Shared Steps");
                        }
                    }
                        
                }

            }
        }
    }
}
