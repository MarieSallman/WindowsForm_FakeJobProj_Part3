using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace P5
{
    public class FakeIssueRepository : IIssueRepository
    {
        public const string NO_ERROR = "";
        public const string EMPTY_TITLE_ERROR = "A Title is required.";
        public const string EMPTY_DISCOVERY_DATETIME_ERROR = "Must select a Discovery Date/Time.";
        public const string FUTURE_DISCOVERY_DATETIME_ERROR = "Issues can't be from the future.";
        public const string EMPTY_DISCOVERER_ERROR = "A Discoverer is required.";

        private static List<Issue> _Issues = new List<Issue>();

        public FakeIssueRepository()
        {
            if (_Issues.Count == 0)
            {
                _Issues.Add(new Issue
                {
                    Id = 1,
                    ProjectId = 1,
                    Title = "First Issue",
                    DiscoveryDate = new DateTime(2019, 3, 16, 10, 11, 27), //I modified these because displaying the min and max value is very difficult in a dateTime box
                    Discoverer = "Bishop, Dave",
                    InitialDescription = "The first issue ever created.",
                    Component = "FormMain",
                    IssueStatusId = 1
                });
                _Issues.Add(new Issue
                {
                    Id = 2,
                    ProjectId = 1,
                    Title = "Minor Problem",
                    DiscoveryDate = new DateTime(2020, 2, 15, 6, 44, 01),
                    Discoverer = "Bishop, Dave",
                    InitialDescription = "This is a minor issue.",
                    Component = "FormCreateProject",
                    IssueStatusId = 2
                });
                _Issues.Add(new Issue
                {
                    Id = 3,
                    ProjectId = 1,
                    Title = "Mediocre Problem",
                    DiscoveryDate = DateTime.Now,
                    Discoverer = "Bishop, Dave",
                    InitialDescription = "This is a medicore issue.",
                    Component = "FormModifyProject",
                    IssueStatusId = 3
                });
            }
        }
        private string ValidateIssue(Issue issue)
        {
            string newIssueName = issue.Title.Trim();
            bool aModifiedIssueCanHaveTheSameNameAsItself = false;
            if (IsDupliclate(newIssueName))
            {
                foreach(Issue issue1 in _Issues)
                {
                    if(issue.Id == issue1.Id) //this checks to see if Id already exists in _issues. if it does, that means it must be trying to modify
                    {
                        aModifiedIssueCanHaveTheSameNameAsItself = true;
                    }
                }
                if(aModifiedIssueCanHaveTheSameNameAsItself == false)
                {
                    return "Issue name already exists";
                }
            }
            if (newIssueName == "")
            {
                return EMPTY_TITLE_ERROR;
            }
            if (issue.Discoverer == "")
            {
                return EMPTY_DISCOVERER_ERROR;
            }
            if (issue.DiscoveryDate == null)
            {
                return EMPTY_DISCOVERY_DATETIME_ERROR;
            }
            if (issue.DiscoveryDate > DateTime.Now)
            {
                return FUTURE_DISCOVERY_DATETIME_ERROR;
            }
            return NO_ERROR;
        }
        private bool IsDupliclate(string title)
        {
            bool isDuplicate = false;
            foreach(Issue i in _Issues)
            {
                if(title == i.Title)
                {
                    isDuplicate = true;
                }
            }
            return isDuplicate;
        }


        public string Add(Issue issue)
        {
            string validate = ValidateIssue(issue);
            int currentMaxId = 0;
            if (validate == NO_ERROR)
            {
                foreach (Issue l in _Issues)
                {
                    if(l.Id > currentMaxId)
                    {
                        currentMaxId = l.Id;
                    }
                }
                ++currentMaxId;

                
                _Issues.Add(new Issue
                {
                    Id = currentMaxId,
                    ProjectId = issue.ProjectId,
                    Title = issue.Title,
                    DiscoveryDate = issue.DiscoveryDate,
                    Discoverer = issue.Discoverer,
                    InitialDescription = issue.InitialDescription,
                    Component = issue.Component,
                    IssueStatusId = issue.IssueStatusId
                });

                return NO_ERROR;
            }
            else
            {
                return validate;
            }
            
        }

        public List<Issue> GetAll(int ProjectId)
        {
            _Issues.OrderBy(sort => sort.Id);
            return _Issues;
        }

        public bool Remove(Issue issue)
        {
            int index = 0;
            foreach (Issue issues in _Issues)
            {
                if(issue.Id == issues.Id && issue.Title == issues.Title)
                {
                    _Issues.RemoveAt(index);
                    return true;
                }
                index++;
            }
            return false;
        }

        public string Modify(Issue issue)
        {
            string validate = ValidateIssue(issue);
            if(validate == NO_ERROR)
            {
                int index = 0;
                foreach (Issue i in _Issues)
                {
                    if (i.Id == issue.Id)
                    {
                        _Issues[index] = issue;
                        return NO_ERROR;
                    }
                    index++;
                }
                return NO_ERROR;
            }
            else
            {
                return validate;
            }
        }

        public int GetTotalNumberOfIssues(int ProjectId)
        {
            int total = 0;
            foreach(Issue issue in _Issues)
            {
                if(issue.ProjectId == ProjectId)
                {
                    total++;
                }
            }
            return total;
        }

        public List<string> GetIssuesByMonth(int ProjectId)
        {
            List<string> issuesByMonth = new List<string>();
            foreach (Issue i in _Issues)
            {
                if (i.ProjectId == ProjectId)
                {
                    issuesByMonth.Add(i.DiscoveryDate.Year.ToString() + i.DiscoveryDate.Month.ToString());
                }
            }
            return issuesByMonth;
        }

        public List<string> GetIssuesByDiscoverer(int ProjectId)
        {
            _Issues.Sort((x, y) => string.Compare(x.Discoverer, y.Discoverer));
            var retList = new List<string>();
            foreach (Issue issue in _Issues)
            {
                retList.Add(issue.Discoverer);
            }
            return retList;
        }

        public Issue GetIssueById(int Id)
        {
            List<Issue> issuesById = new List<Issue>();
            int index = 0;
            foreach (Issue issue in _Issues)
            {
                if (issue.Id == Id)
                {
                    return _Issues[index];
                }
                index++;
            }
            return null; //probably should check if null on the caller
        }
    }
}