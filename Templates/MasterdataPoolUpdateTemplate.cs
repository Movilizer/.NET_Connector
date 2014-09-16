using System;
using System.Collections.Generic;
using System.Text;

namespace MWS.Templates
{
    public class MasterdataPoolUpdateTemplate
    {
        public MovilizerMasterdataPoolUpdate _masterdataPoolUpdate;

        List<MasterdataUpdateTemplate> _masterdataUpdates;
        List<MasterdataDeleteTemplate> _masterdataDeletes;
        List<MasterdataReferenceTemplate> _masterdataReferences;

        public MasterdataPoolUpdateTemplate(string pool)
        {
            _masterdataPoolUpdate = new MovilizerMasterdataPoolUpdate();
            _masterdataPoolUpdate.pool = pool;

            _masterdataUpdates = new List<MasterdataUpdateTemplate>();
            _masterdataDeletes = new List<MasterdataDeleteTemplate>();
            _masterdataReferences = new List<MasterdataReferenceTemplate>();
        }

        public void UpdateMasterdata(MasterdataUpdateTemplate mduTemplate)
        {
            _masterdataUpdates.Add(mduTemplate);
        }

        public void DeleteMasterdata(MasterdataDeleteTemplate mddTemplate)
        {
            _masterdataDeletes.Add(mddTemplate);
        }

        public MovilizerMasterdataPoolUpdate ToMasterdataPoolUpdate()
        {
            int count = 0;
            _masterdataPoolUpdate.update = new MovilizerMasterdataUpdate[_masterdataUpdates.Count];

            foreach (MasterdataUpdateTemplate mduTemplate in _masterdataUpdates)
            {
                _masterdataPoolUpdate.update[count++] = mduTemplate.ToMasterdataUpdate();
            }

            count = 0;
            _masterdataPoolUpdate.delete = new MovilizerMasterdataDelete[_masterdataDeletes.Count];

            foreach (MasterdataDeleteTemplate mddTemplate in _masterdataDeletes)
            {
                _masterdataPoolUpdate.delete[count++] = mddTemplate.ToMasterdataDelete();
            }

            count = 0;
            _masterdataPoolUpdate.reference = new MovilizerMasterdataReference[_masterdataReferences.Count];

            foreach (MasterdataReferenceTemplate mdrTemplate in _masterdataReferences)
            {
                _masterdataPoolUpdate.reference[count++] = mdrTemplate.ToMasterdataReference();
            }

            return _masterdataPoolUpdate;
        }
    }
}
