using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using MWS.Helper;
using MWS.Log;
using MWS.Templates;
using MWS.WindowsService;
using MWS.Movilizer;
using System.ServiceModel;

namespace MWS.WebService
{
    public partial class MovilizerWebService : Movilizer.MovilizerWebServiceV14Client
    {
        private Queue _inQueue;
        private Queue _outQueue;

        private Queue _outTransaction;
        private string _requestAckKey;

        private List<MovilizerMoveletSet> _moveletSets;
        private List<MovilizerMoveletAssignment> _moveletAssignments;
        private List<MovilizerMoveletDelete> _moveletDeletes;
        private List<MovilizerMasterdataPoolUpdate> _masterdataPoolUpdate;
        private List<MovilizerDocumentPoolUpdate> _documentPoolUpdate;
        private List<MovilizerMoveletAssignmentDelete> _moveletDeleteAssignments;
        private List<MovilizerParticipantConfiguration> _ParticipantConfigurations;

        private List<MovilizerUploadDataContainer> _uploadDataContainers;
        private List<MovilizerParticipantReset> _ParticipantReset;

        // pool templates
        private Dictionary<string, object> _masterdataPoolUpdateTemplates;
        private Dictionary<string, object> _documentPoolUpdateTemplates;


        private MovilizerWebService()
            : base(new BasicHttpBinding(BasicHttpSecurityMode.Transport) { MaxReceivedMessageSize = 5242880, SendTimeout = new TimeSpan(0, 5, 0), MaxBufferSize = 5242880 },
                    new EndpointAddress(MovilizerWebServiceConstants.GetWebServiceUrl()))
        {
            // initialize queues
            _inQueue = new Queue();
            _outQueue = new Queue();

            _outTransaction = null;
            _requestAckKey = null;

            // init parameter lists
            _moveletSets = new List<MovilizerMoveletSet>();
            _moveletAssignments = new List<MovilizerMoveletAssignment>();
            _moveletDeletes = new List<MovilizerMoveletDelete>();
            _masterdataPoolUpdate = new List<MovilizerMasterdataPoolUpdate>();
            _ParticipantReset = new List<MovilizerParticipantReset>();
            _documentPoolUpdate = new List<MovilizerDocumentPoolUpdate>();
            _moveletDeleteAssignments = new List<MovilizerMoveletAssignmentDelete>();
            _ParticipantConfigurations = new List<MovilizerParticipantConfiguration>();
            _uploadDataContainers = new List<MovilizerUploadDataContainer>();

            // pool templates
            _masterdataPoolUpdateTemplates = new Dictionary<string, object>();
            _documentPoolUpdateTemplates = new Dictionary<string, object>();
        }

        public void BeginTransaction()
        {
            if (_outTransaction != null)
            {
                throw (new Exception("CommitTransaction or RollbackTransaction must be called first to close the previous one. An open transaction found"));
            }

            _outTransaction = new Queue();
        }

        public void CommitTransaction()
        {
            if (_outTransaction == null)
            {
                throw (new Exception("BeginTransaction must be called before commit or rollback. No open transactions found"));
            }

            _outQueue.Enqueue(_outTransaction);
            _outTransaction = null;
        }

        public void RollbackTransaction()
        {
            if (_outTransaction == null)
            {
                throw (new Exception("BeginTransaction must be called before commit or rollback. No open transactions found"));
            }

            _outTransaction = null;
        }

        public void EnqueueMovelet(MovilizerMovelet movelet, string uname, string phone)
        {
            // create a movelet set
            MovilizerMoveletSet moveletSet = new MovilizerMoveletSet();
            moveletSet.movelet = new MovilizerMovelet[] { movelet };

            // add a participant
            MovilizerParticipant participant = this.CreateParticipant(uname, phone);
            moveletSet.participant = new MovilizerParticipant[] { participant };

            _outQueue.Enqueue(moveletSet);
        }

        public void EnqueueResetParticipant(string phone)
        {
            _ParticipantReset.Add(new MovilizerParticipantReset()
            {
                deviceAddress = phone
            });
        }

        public void EnqueueMoveletSet(MovilizerMoveletSet moveletSet, string uname, string phone)
        {
            // add a single participant
            EnqueueMoveletSet(moveletSet, new string[] { uname }, new string[] { phone });
        }

        public void EnqueueMoveletSet(MovilizerMoveletSet moveletSet, string[] unames, string[] phones)
        {
            // add participants
            List<MovilizerParticipant> participants = new List<MovilizerParticipant>();
            for (int i = 0; i < unames.Length; ++i)
            {
                participants.Add(this.CreateParticipant(unames[i], phones[i]));
            }

            moveletSet.participant = participants.ToArray();

            _outQueue.Enqueue(moveletSet);
        }

        public void EnqueueMoveletSet(MovilizerMoveletSet moveletSet)
        {
            _outQueue.Enqueue(moveletSet);
        }

        public void EnqueueMoveletAssignment(string moveletKey, string moveletKeyExt, string uname, string phone)
        {
            // create movelet assignment
            _outQueue.Enqueue(this.CreateMoveletAssignment(moveletKey, moveletKeyExt, uname, phone));
        }

        public void EnqueueMoveletDeletion(string moveletKey, string moveletKeyExt)
        {
            // create movelet deletion
            _outQueue.Enqueue(this.CreateMoveletDelete(moveletKey, moveletKeyExt));
        }

        public void EnqueueMoveletDeletion(MovilizerMoveletDelete moveletDelete)
        {
            _outQueue.Enqueue(moveletDelete);
        }

        public void EnqueueMoveletAssignmentDeletion(string moveletKey, string phone)
        {
            // create movelet deletion
            _outQueue.Enqueue(this.CreateMoveletAssignmentDelete(moveletKey, null, phone, true));
        }

        public void EnqueueMoveletAssignmentDeletion(string moveletKey, string moveletKeyExt, string phone)
        {
            // create movelet deletion
            _outQueue.Enqueue(this.CreateMoveletAssignmentDelete(moveletKey, moveletKeyExt, phone, true));
        }

        public void EnqueueAllMoveletsAssignmentDeletion(string phone)
        {
            // create movelet deletion
            _outQueue.Enqueue(this.CreateMoveletAssignmentDelete(null, null, phone, true));
        }

        public void EnqueueMasterdataPoolUpdate(MovilizerMasterdataPoolUpdate masterdataPoolUpdate)
        {
            _outQueue.Enqueue(masterdataPoolUpdate);
        }

        public void EnqueueDocumentPoolUpdate(MovilizerDocumentPoolUpdate documentPoolUpdate)
        {
            _outQueue.Enqueue(documentPoolUpdate);
        }

        public void EnqueueParticipantConfiguration(MovilizerParticipantConfiguration participantConfiguration)
        {
            _outQueue.Enqueue(participantConfiguration);
        }

        public void EnqueueMoveletAssignment(MovilizerMoveletAssignment assignment)
        {
            // create movelet assignment
            _outQueue.Enqueue(assignment);
        }

        public MasterdataPoolUpdateTemplate GetOrCreateMasterdataPoolUpdateTemplate(string pool)
        {
            MasterdataPoolUpdateTemplate poolUpdate;
            if (_masterdataPoolUpdateTemplates.ContainsKey(pool))
            {
                poolUpdate = _masterdataPoolUpdateTemplates[pool] as MasterdataPoolUpdateTemplate;
            }
            else
            {
                poolUpdate = new MasterdataPoolUpdateTemplate(pool);
                _masterdataPoolUpdateTemplates[pool] = poolUpdate;
                //_outQueue.Enqueue(poolUpdate);
            }
            return poolUpdate;
        }

        public DocumentPoolUpdateTemplate GetOrCreateDocumentPoolUpdateTemplate(string pool)
        {
            DocumentPoolUpdateTemplate poolUpdate;
            if (_documentPoolUpdateTemplates.ContainsKey(pool))
            {
                poolUpdate = _documentPoolUpdateTemplates[pool] as DocumentPoolUpdateTemplate;
            }
            else
            {
                poolUpdate = new DocumentPoolUpdateTemplate(pool);
                _documentPoolUpdateTemplates[pool] = poolUpdate;
                //_outQueue.Enqueue(poolUpdate);
            }
            return poolUpdate;
        }

        protected void EnqueueInboundQueueObjects(object[] objs)
        {
            int len = (objs != null) ? objs.Length : 0;
            for (int i = 0; i < len; ++i)
            {
                _inQueue.Enqueue(objs[i]);
            }
        }

        public object DequeueResponseObject()
        {
            return _inQueue.Count > 0 ? _inQueue.Dequeue() : null;
        }

        protected void EnqueueResponse(MovilizerResponse response)
        {
            EnqueueInboundQueueObjects(response.replyMovelet);
            EnqueueInboundQueueObjects(response.uploadContainer);
            EnqueueInboundQueueObjects(response.masterdataAck);
            EnqueueInboundQueueObjects(response.masterdataDeleted);

            // save acknowledge key for the next request
            _requestAckKey = response.requestAcknowledgeKey;
        }

        public MovilizerResponse PostMovilizerRequest(bool synchronousReponse = false, int numResponses = 1000)
        {
            // create request object
            MovilizerRequest request = this.ComposeRequest();
            request.synchronousResponse = synchronousReponse;
            request.numResponses = numResponses;

            string debugOutput = Configuration.GetDebugOutputPath();
            if (!String.IsNullOrEmpty(debugOutput))
            {
                XmlHelper.SerializeToFile(debugOutput + "MovilizerRequest.xml", request);
            }

            // consume web service
            MovilizerResponse response = null;
            int countdown = 3;
            while (response == null && countdown > 0)
            {
                try
                {
                    countdown--;
                    response = Movilizer(request);
                }
                catch (Exception e)
                {
                    LogFactory.WriteError(e.ToString());

                    if (countdown > 0)
                    {
                        // sleep for 10 seconds and try again
                        Thread.Sleep(10000);
                    }
                    else if (Configuration.ForceRequeingOnError())
                    {
                        // Requeue waiting message
                        LogFactory.WriteWarning("Error exceeded 3 consecutive retries, reqeuing messages for further processing.");

                        _moveletSets.AddRange(request.moveletSet);
                        _moveletAssignments.AddRange(request.moveletAssignment);
                        request.moveletDelete = _moveletDeletes.ToArray();
                        request.masterdataPoolUpdate = _masterdataPoolUpdate.ToArray();
                        request.moveletAssignmentDelete = _moveletDeleteAssignments.ToArray();
                        request.documentPoolUpdate = _documentPoolUpdate.ToArray();
                        request.participantReset = _ParticipantReset.ToArray();
                    }
                }
            }

            if (response != null)
            {

                this.EnqueueResponse(response);

                // log status messages
                MovilizerStatusMessage[] statusMessages = response.statusMessage;
                if (statusMessages != null)
                {
                    foreach (MovilizerStatusMessage statusMessage in statusMessages)
                    {
                        LogFactory.WriteEntry(statusMessage);
                    }
                }

                // log movelet errors 
                MovilizerMoveletError[] moveletErrors = response.moveletError;
                if (moveletErrors != null)
                {
                    foreach (MovilizerMoveletError moveletError in moveletErrors)
                    {
                        LogFactory.WriteEntry(moveletError);
                    }
                }
            }

            return response;
        }

        private void InitializeMoveletRequest(MovilizerRequest request)
        {
            request.systemId = Configuration.GetSystemId();
            request.systemPassword = Configuration.GetSystemPassword();

            // send acknowlegement key
            request.requestAcknowledgeKey = _requestAckKey;
        }

        private void ProcessOutboundQueue(Queue outQueue)
        {
            while (outQueue.Count > 0)
            {
                object obj = outQueue.Dequeue();

                if (obj is MovilizerMoveletSet)
                {
                    _moveletSets.Add(obj as MovilizerMoveletSet);
                }
                else if (obj is MovilizerMoveletAssignment)
                {
                    _moveletAssignments.Add(obj as MovilizerMoveletAssignment);
                }
                else if (obj is MovilizerMoveletDelete)
                {
                    _moveletDeletes.Add(obj as MovilizerMoveletDelete);
                }
                else if (obj is MovilizerMasterdataPoolUpdate)
                {
                    _masterdataPoolUpdate.Add(obj as MovilizerMasterdataPoolUpdate);
                }
                else if (obj is MovilizerDocumentPoolUpdate)
                {
                    _documentPoolUpdate.Add(obj as MovilizerDocumentPoolUpdate);
                }
                else if (obj is MovilizerMoveletAssignmentDelete)
                {
                    _moveletDeleteAssignments.Add(obj as MovilizerMoveletAssignmentDelete);
                }
                else if (obj is MovilizerParticipantReset)
                {
                    _ParticipantReset.Add(obj as MovilizerParticipantReset);
                }
                else if (obj is MovilizerParticipantConfiguration)
                {
                    _ParticipantConfigurations.Add(obj as MovilizerParticipantConfiguration);
                }
                else if (obj is Queue)
                {
                    ProcessOutboundQueue(obj as Queue);
                }
                else
                {
                    throw new InvalidDataException("Invalid data element found in the outbound queue.");
                }
            }
        }

        private void ProcessOutboundPool(Dictionary<string, object> outPool)
        {
            foreach (string pool in outPool.Keys)
            {
                object obj = outPool[pool];

                if (obj is MasterdataPoolUpdateTemplate)
                {
                    _masterdataPoolUpdate.Add((obj as MasterdataPoolUpdateTemplate).ToMasterdataPoolUpdate());
                }
                else if (obj is DocumentPoolUpdateTemplate)
                {
                    _documentPoolUpdate.Add((obj as DocumentPoolUpdateTemplate).ToDocumentPoolUpdate());
                }
                else
                {
                    throw new InvalidDataException("Invalid data element found in the outbound pool.");
                }
            }
            // clear outbound pool
            outPool.Clear();
        }

        protected MovilizerRequest ComposeRequest()
        {
            // process outbound pools
            ProcessOutboundPool(_masterdataPoolUpdateTemplates);
            ProcessOutboundPool(_documentPoolUpdateTemplates);

            // process outbound queue
            ProcessOutboundQueue(_outQueue);

            // create request
            MovilizerRequest request = new MovilizerRequest();
            InitializeMoveletRequest(request);

            // assign parameters
            request.moveletSet = _moveletSets.ToArray();
            request.moveletAssignment = _moveletAssignments.ToArray();
            request.moveletDelete = _moveletDeletes.ToArray();
            request.masterdataPoolUpdate = _masterdataPoolUpdate.ToArray();
            request.moveletAssignmentDelete = _moveletDeleteAssignments.ToArray();
            request.documentPoolUpdate = _documentPoolUpdate.ToArray();
            request.participantReset = _ParticipantReset.ToArray();
            request.participantConfiguration = _ParticipantConfigurations.ToArray();

            // clear temps
            _moveletSets.Clear();
            _moveletAssignments.Clear();
            _moveletDeletes.Clear();
            _masterdataPoolUpdate.Clear();
            _documentPoolUpdate.Clear();
            _moveletDeleteAssignments.Clear();
            _ParticipantReset.Clear();
            _ParticipantConfigurations.Clear();

            // clear pools
            _masterdataPoolUpdateTemplates.Clear();
            _documentPoolUpdateTemplates.Clear();

            return request;
        }

        #region Protected WS Object Factory Methods

        protected MovilizerMoveletDelete CreateMoveletDelete(string moveletKey, string moveletKeyExt)
        {
            MovilizerMoveletDelete delete = new MovilizerMoveletDelete();

            delete.moveletKey = moveletKey;

            if (moveletKeyExt != null)
            {
                delete.moveletKeyExtension = moveletKeyExt;
            }
            else
            {
                delete.ignoreExtensionKey = true;
            }

            return delete;
        }

        protected MovilizerMoveletAssignmentDelete CreateMoveletAssignmentDelete(string moveletKey, string moveletKeyExt, string deviceAddress, bool hardDelete)
        {
            MovilizerMoveletAssignmentDelete delete = new MovilizerMoveletAssignmentDelete();

            delete.moveletKey = moveletKey;

            if (moveletKeyExt != null)
            {
                delete.moveletKeyExtension = moveletKeyExt;
            }
            else
            {
                delete.ignoreExtensionKey = true;
            }

            delete.deviceAddress = deviceAddress;
            delete.hardDelete = hardDelete;

            return delete;
        }

        protected MovilizerMoveletAssignment CreateMoveletAssignment(string moveletKey, string moveletKeyExt, string uname, string phone)
        {
            MovilizerMoveletAssignment assignment = new MovilizerMoveletAssignment();

            assignment.moveletKey = moveletKey;

            if (moveletKeyExt != null)
            {
                assignment.moveletKeyExtension = moveletKeyExt;
            }
            else
            {
                assignment.ignoreExtensionKey = true;
            }

            MovilizerParticipant participant = CreateParticipant(uname, phone);
            assignment.participant = new MovilizerParticipant[] { participant };

            return assignment;
        }

        protected MovilizerParticipant CreateParticipant(string uname, string deviceAddress)
        {
            MovilizerParticipant participant = new MovilizerParticipant();

            participant.deviceAddress = deviceAddress;
            participant.name = uname;
            participant.participantKey = uname;

            return participant;
        }

        #endregion

    }
}