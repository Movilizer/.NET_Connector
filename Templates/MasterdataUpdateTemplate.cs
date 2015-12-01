using MWS.Movilizer;
using System.Collections.Generic;


namespace MWS.Templates
{
    public class MasterdataUpdateTemplate
    {
        public MovilizerMasterdataUpdate _masterdataUpdate;
        public List<MasterdataEntryTemplate> _entries;


        public MasterdataUpdateTemplate(MovilizerMasterdataUpdate masterdataUpdate)
        {
            _masterdataUpdate = SerializeHelper.CloneObject(masterdataUpdate);

            // extract entries
            _entries = new List<MasterdataEntryTemplate>();
            MovilizerGenericDataContainerEntry[] entries = _masterdataUpdate.data;
            if (entries != null)
            {
                foreach (MovilizerGenericDataContainerEntry entry in entries)
                {
                    _entries.Add(new MasterdataEntryTemplate(entry));
                }
            }
        }

        public MasterdataEntryTemplate GetEntry(string name)
        {
            foreach (MasterdataEntryTemplate meTemplate in _entries)
            {
                if (meTemplate._masterdataEntry.name == name)
                {
                    return meTemplate;
                }
            }
            return null;
        }

        public MovilizerMasterdataUpdate ToMasterdataUpdate()
        {
            int eCount = 0;
            _masterdataUpdate.data = new MovilizerGenericDataContainerEntry[_entries.Count];

            foreach (MasterdataEntryTemplate entryTemplate in _entries)
            {
                _masterdataUpdate.data[eCount++] = entryTemplate.ToMasterdataEntry();
            }
            return _masterdataUpdate;
        }
    }
}
