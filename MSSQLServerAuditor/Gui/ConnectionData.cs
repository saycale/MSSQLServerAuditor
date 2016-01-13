using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MSSQLServerAuditor.Model;

namespace MSSQLServerAuditor.Gui
{
    /// <summary>
    /// Data connection.
    /// </summary>
    public class ConnectionData
    {
        public           DateTime          CreationDateTime { get; private set; }
        private readonly MsSqlAuditorModel _model;
        private          TemplateNodeInfo  _staticTreeRoot;

        public Stack<string> StartupTemplateInfoIdStack
        {
            get;
            private set;
        }

        public TemplateNodeInfo RootInstance { get; set; }

        public string StartupTemplateId
        {
            get;
            private set;
        }

        public TemplateNodeInfo RootOfTemplate
        {
            get;
            private set;
        }

        private ConnectionData(MsSqlAuditorModel model)
        {
            this.CreationDateTime = DateTime.Now;
            this._model           = model;
        }

        /// <summary>
        /// Initializes the object ConnectionData.
        /// </summary>
        /// <param name="model">Model SqlAuditor.</param>
        /// <param name="connectionGroup">Info group connection.</param>
        public ConnectionData(MsSqlAuditorModel model, ConnectionGroupInfo connectionGroup)
            : this(model)
        {
            ConnectionGroup = connectionGroup;
            ReloadTemplate();
        }

        /// <summary>
        /// Reload template.
        /// </summary>
        public void ReloadTemplate()
        {
            if (ConnectionGroup == null)
            {
                throw new InvalidOperationException();
            }

            var           fileName                   = this._model.FilesProvider.GetTreeTemplateFileName(ConnectionGroup.TemplateFileName, ConnectionGroup.TemplateDir);
            string        startupTemplateId          = null;
            Stack<string> startupTemplateInfoIdStack = null;

            this.RootOfTemplate             = this._model.LoadTemplateNodes(fileName, ConnectionGroup.IsExternal, out startupTemplateId, out startupTemplateInfoIdStack);
            this.StartupTemplateId          = startupTemplateId;
            this.StartupTemplateInfoIdStack = startupTemplateInfoIdStack;

            if (RootOfTemplate.Childs.Count > 0)
            {
                ConnectionGroup.TemplateId = RootOfTemplate.Childs.First().TemplateId;
            }
        }

        /// <summary>
        /// Info group connection.
        /// </summary>
        public ConnectionGroupInfo ConnectionGroup { get; private set; }

        ///// <summary>
        ///// List of node template.
        ///// </summary>
        //public List<TemplateNodeInfo> TemplateNodes { get; private set; }

        public TemplateNodeInfo RootOfStaticTree
        {
            get
            {
                if (_staticTreeRoot == null)
                {
                    _staticTreeRoot = RootOfTemplate.InstatiateStaticTree(this, null, RootInstance != null ? RootInstance.TemplateNodeId : null);
                }

                return _staticTreeRoot;
            }
        }

        /// <summary>
        /// Reset Static Tree Root
        /// </summary>
        public void ResetStaticTreeRoot()
        {
            this._staticTreeRoot = null;
        }

        /// <summary>
        /// Name group connection or title raw data.
        /// </summary>
        public string Title
        {
            get
            {
                return ConnectionGroup != null ? ConnectionGroup.Name : "Untitled";
            }
        }

        /// <summary>
        /// Display name.
        /// </summary>
        public string UserNameAsDisplayed
        {
            get { return ConnectionGroup.UserLoginName; }
        }

        /// <summary>
        /// Source connection
        /// </summary>
        public string SourceConnectionName
        {
            get
            {
                return ConnectionGroup.Name;
            }
        }

        /// <summary>
        /// Aviable with connection.
        /// </summary>
        public bool IsLiveConnection
        {
            get { return _model.DefaultVaultProcessor == _model.GetVaultProcessor(ConnectionGroup); }
        }

    }
}
