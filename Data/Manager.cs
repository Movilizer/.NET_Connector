using System;

using MWS.Templates;
using MWS.WebService;
using MWS.WindowsService;
using MWS.Movilizer;
using System.Collections.Generic;
using MWS.Helper;

namespace MWS.Data
{
    public abstract class Manager
    {
        protected MovilizerWebService _service;

        /// <summary>
        /// Manager Constructor which generates and reads a (default) RegistryConfiguration and generates a MovilizerWebService Instance.
        /// </summary>
        public Manager()
        {
            Configuration.ReadConfiguration(new RegistryConfigurator());
            _service = Singleton<MovilizerWebService>.Instance;
        }

        /// <summary>
        /// DEPRICATED: supports legacy versions of the .NET Connector in which the MovilizerWebService instance was passed as a parameter.
        /// The Configuration needs to be loaded before calling this Constructor.
        /// </summary>
        /// <param name="service"></param>
        public Manager(MovilizerWebService service)
        {
            Configuration.ReadConfiguration(new RegistryConfigurator());
            _service = service;
        }

        /// <summary>
        /// Manager Constructor which reads a specific Configuration and generates a MovilizerWebService Instance.
        /// </summary>
        public Manager(IConfigurator configuration) 
        {
            Configuration.ReadConfiguration(configuration);
            _service = Singleton<MovilizerWebService>.Instance;
        }

        public void BeginTransaction() =>
            _service.BeginTransaction();

        public void CommitTransaction() =>
            _service.CommitTransaction();

        public void RollbackTransaction() =>
            _service.RollbackTransaction();

        public void PostMovilizerRequest() =>
            _service.PostMovilizerRequest();

        public void AssignMoveletTo(string moveletKey, string phone) =>
            _service.EnqueueMoveletAssignment(moveletKey, null, phone, phone);

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

        public void DeleteMoveletAssignment(string moveletKey, string phone) =>
            _service.EnqueueMoveletAssignmentDeletion(moveletKey, phone);

        public void DeleteMoveletAssignment(string moveletKey, string moveletKeyExt, string phone) =>
            _service.EnqueueMoveletAssignmentDeletion(moveletKey, moveletKeyExt, phone);

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

        public void DeleteAllMoveletAssignments(string phone) =>
            _service.EnqueueAllMoveletsAssignmentDeletion(phone);

        public void DeleteAllMoveletAssignments(string[] phones)
        {
            foreach (string phone in phones)
            {
                _service.EnqueueAllMoveletsAssignmentDeletion(phone);
            }
        }

        public void DeleteMovelet(string moveletKey) =>
            _service.EnqueueMoveletDeletion(moveletKey, null);

        public void DeleteMovelet(string[] moveletKeys)
        {
            foreach (string moveletKey in moveletKeys)
            {
                _service.EnqueueMoveletDeletion(moveletKey, null);
            }
        }

        public void DeleteMovelets(List<MovilizerMoveletDelete> moveletDeletes) =>
             moveletDeletes.ForEach(mmd => _service.EnqueueMoveletDeletion(mmd));

        public void DeleteMovelet(string moveletKey, string moveletKeyExt) =>
            _service.EnqueueMoveletDeletion(moveletKey, moveletKeyExt);

        public void SendMoveletTo(MoveletTemplate mTemplate, string[] phones) =>
            SendMoveletTo(mTemplate, phones, phones);

        public void SendMoveletTo(MoveletTemplate mTemplate, string phone) =>
            SendMoveletTo(mTemplate, phone, phone);

        public void SendMoveletTo(MoveletTemplate mTemplate, string uname, string phone) =>
            SendMoveletTo(mTemplate, new string[] { uname }, new string[] { phone });

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

        public void SendMoveletSetTo(MoveletSetTemplate msTemplate, string[] phones) =>
            SendMoveletSetTo(msTemplate, phones, phones);


        public void SendMoveletSetTo(MoveletSetTemplate msTemplate, string phone) =>
            SendMoveletSetTo(msTemplate, new string[] { phone });


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

        public void SendMasterdataUpdate(MasterdataUpdateTemplate mduTemplate, string pool) =>
            _service.GetOrCreateMasterdataPoolUpdateTemplate(pool).UpdateMasterdata(mduTemplate);

        public void SendMasterdataDelete(MasterdataDeleteTemplate mduTemplate, string pool) =>
            _service.GetOrCreateMasterdataPoolUpdateTemplate(pool).DeleteMasterdata(mduTemplate);

        public void SendDocumentUpdate(DocumentUpdateTemplate docTemplate, string pool) =>
            _service.GetOrCreateDocumentPoolUpdateTemplate(pool).UpdateDocument(docTemplate);

        public void SendParticipantConfigurationsUpdate(List<MovilizerParticipantConfiguration> participantConfigurations) => 
            participantConfigurations.ForEach(mpc => _service.EnqueueParticipantConfiguration(mpc));


        public void AssignMoveletTo(List<MovilizerMoveletAssignment> assignments) =>
             assignments.ForEach(mma => _service.EnqueueMoveletAssignment(mma));


        public abstract void RunServiceCycle();
    }
}
