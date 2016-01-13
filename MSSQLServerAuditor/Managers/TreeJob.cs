using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MSSQLServerAuditor.Model;

namespace MSSQLServerAuditor.Managers
{
	public class TreeJob
	{
		private readonly object        _syncObj;
		private readonly List<TreeJob> _childJobs;

		public string       Title              { get; set; }
		public Action       Action             { get; set; }
		public TreeJobState State              { get; set; }
		public int          PromisedChildCount { get; set; }
		public TreeJob      Parent             { get; set; }

		public TreeJob(ConcreteTemplateNodeDefinition node)
		{
			if (node.TemplateNode != null)
			{
				Title = node.TemplateNode.Parent == null
					? node.TemplateNode.Title
					: node.TemplateNode.Parent.Title + "\\" + node.TemplateNode.Title;
			}

			this._syncObj   = new object();
			this._childJobs = new List<TreeJob>();
		}

		public bool RemoveChildJob(TreeJob child)
		{
			lock (this._syncObj)
			{
				return this._childJobs.Remove(child);
			}
		}

		public void AddChildJob(TreeJob child)
		{
			lock (this._syncObj)
			{
				this._childJobs.Add(child);
			}
		}

		public IEnumerable<TreeJob> ChildJobs
		{
			get
			{
				lock (this._syncObj)
				{
					return new List<TreeJob>(this._childJobs);
				}
			}
		}
	}
}
