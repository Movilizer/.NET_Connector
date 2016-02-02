using MWS.Movilizer;
using System.Collections.Generic;

namespace MWS.Templates
{
    public class ParticipantConfigurationTemplate
    {
        public MovilizerParticipantConfiguration _participantConfiguration;
        public List<MovilizerAttributeEntry> _attributeUpdates;

        public ParticipantConfigurationTemplate(MovilizerParticipantConfiguration participantConfiguration)
        {
            _participantConfiguration = SerializeHelper.CloneObject(participantConfiguration);
            _attributeUpdates = new List<MovilizerAttributeEntry>(_participantConfiguration.attributeUpdate);
        }

        public MovilizerParticipantConfiguration ToMasterdataEntry()
        {
            _participantConfiguration.attributeUpdate = _attributeUpdates.ToArray();
            return _participantConfiguration;
        }

        public void AddAttributeUpdate(MovilizerAttributeEntry attributeUpdate)
        {
            _attributeUpdates.Add(attributeUpdate);
        }
    }
}