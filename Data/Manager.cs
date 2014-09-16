using System;

using MWS.Templates;
using MWS.WebService;
using MWS.WindowsService;

namespace MWS.Data
{
    public abstract class Manager
    {
        protected MovilizerWebService _service;

        public Manager(MovilizerWebService webService) 
        {
            _service = webService;
        }

        public void BeginTransaction()
        {
            _service.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _service.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _service.RollbackTransaction();
        }

        public void PostMovilizerRequest()
        {
            _service.PostMovilizerRequest();
        }

        public void AssignMoveletTo(string moveletKey, string phone)
        {
            _service.EnqueueMoveletAssignment(moveletKey, null, phone, phone);
        }

        public void AssignMoveletTo(string moveletKey, string[] phones)
        {
            foreach (string phone in phones)
            {
                _service.EnqueueMoveletAssignment(moveletKey, null, phone, phone);
            }
        }

        public void AssignMoveletTo(string moveletKey, string[] unames, string[] phones)
        {
            for (int i = 0; i < unames.Length; ++i)
            {
                _service.EnqueueMoveletAssignment(moveletKey, null, unames[i], phones[i]);
            }
        }

        public void AssignMoveletTo(string moveletKey, string moveletKeyExt, string[] unames, string[] phones)
        {
            for (int i = 0; i < unames.Length; ++i)
            {
                _service.EnqueueMoveletAssignment(moveletKey, moveletKeyExt, unames[i], phones[i]);
            }
        }

        public void DeleteMoveletAssignment(string moveletKey, string phone)
        {
            _service.EnqueueMoveletAssignmentDeletion(moveletKey, phone);
        }

        public void DeleteMoveletAssignment(string moveletKey, string moveletKeyExt, string phone)
        {
            _service.EnqueueMoveletAssignmentDeletion(moveletKey, moveletKeyExt, phone);
        }

        public void DeleteMoveletAssignment(string moveletKey, string[] phones)
        {
            foreach (string phone in phones)
            {
                _service.EnqueueMoveletAssignmentDeletion(moveletKey, phone);
            }            
        }

        public void DeleteMoveletAssignment(string moveletKey, string moveletKeyExt, string[] phones)
        {
            foreach (string phone in phones)
            {
                _service.EnqueueMoveletAssignmentDeletion(moveletKey, moveletKeyExt, phone);
            }
        }

        public void DeleteAllMoveletAssignments(string phone)
        {
            _service.EnqueueAllMoveletsAssignmentDeletion(phone);
        }

        public void DeleteAllMoveletAssignments(string[] phones)
        {
            foreach (string phone in phones)
            {
                _service.EnqueueAllMoveletsAssignmentDeletion(phone);
            }
        }

        public void DeleteMovelet(string moveletKey)
        {
            _service.EnqueueMoveletDeletion(moveletKey, null);
        }

        public void DeleteMovelet(string[] moveletKeys)
        {
            foreach (string moveletKey in moveletKeys)
            {
                _service.EnqueueMoveletDeletion(moveletKey, null);
            }
        }

        public void DeleteMovelet(string moveletKey, string moveletKeyExt)
        {
            _service.EnqueueMoveletDeletion(moveletKey, moveletKeyExt);
        }

        public void SendMoveletTo(MoveletTemplate mTemplate, string[] phones)
        {
            SendMoveletTo(mTemplate, phones, phones);
        }

        public void SendMoveletTo(MoveletTemplate mTemplate, string phone)
        {
            SendMoveletTo(mTemplate, phone, phone);
        }

        public void SendMoveletTo(MoveletTemplate mTemplate, string uname, string phone)
        {
            SendMoveletTo(mTemplate, new string[] { uname }, new string[] { phone });
        }

        public void SendMoveletTo(MoveletTemplate mTemplate, string[] unames, string[] phones)
        {           
            string debugOutput = Configuration.GetDebugOutputPath();
            if (!String.IsNullOrEmpty(debugOutput))
            {
                // backup the movelet as xml              
                mTemplate.SerializeToFile(debugOutput);
            }

            MovilizerMovelet movelet = mTemplate.ToMovelet();
            _service.EnqueueMoveletDeletion(movelet.moveletKey, movelet.moveletKeyExtension);

            _service.EnqueueMovelet(movelet, unames[0], phones[0]);
            for (int i = 1; i < unames.Length; ++i)
            {
                _service.EnqueueMoveletAssignment(movelet.moveletKey, movelet.moveletKeyExtension, unames[i], phones[i]);
            }            
        }

        public void SendMoveletSetTo(MoveletSetTemplate msTemplate, string[] phones)
        {
            SendMoveletSetTo(msTemplate, phones, phones);
        }

        public void SendMoveletSetTo(MoveletSetTemplate msTemplate, string phone)
        {
            SendMoveletSetTo(msTemplate, new string[] { phone });
        }

        public void SendMoveletSetTo(MoveletSetTemplate msTemplate, string[] unames, string[] phones)
        {
            string debugOutput = Configuration.GetDebugOutputPath();
            if (!String.IsNullOrEmpty(debugOutput))
            {
                // backup the movelets as xml
                msTemplate.SerializeToFile(debugOutput);
            }

            MovilizerMoveletSet moveletSet = msTemplate.ToMoveletSet();
            foreach (MovilizerMovelet movelet in moveletSet.movelet)
            {
                _service.EnqueueMoveletDeletion(movelet.moveletKey, movelet.moveletKeyExtension);
            }

            _service.EnqueueMoveletSet(moveletSet, unames, phones);
        }

        public void SendMasterdataUpdate(MasterdataUpdateTemplate mduTemplate, string pool)
        {
            MasterdataPoolUpdateTemplate mdpuTemplate = _service.GetOrCreateMasterdataPoolUpdateTemplate(pool);
            mdpuTemplate.UpdateMasterdata(mduTemplate);
        }

        public void SendMasterdataDelete(MasterdataDeleteTemplate mduTemplate, string pool)
        {
            MasterdataPoolUpdateTemplate mdpuTemplate = _service.GetOrCreateMasterdataPoolUpdateTemplate(pool);
            mdpuTemplate.DeleteMasterdata(mduTemplate);
        }

        public void SendDocumentUpdate(DocumentUpdateTemplate docTemplate, string pool)
        {
            DocumentPoolUpdateTemplate docpuTemplate = _service.GetOrCreateDocumentPoolUpdateTemplate(pool);
            docpuTemplate.UpdateDocument(docTemplate);
        }

        public abstract void RunServiceCycle();
    }
}
