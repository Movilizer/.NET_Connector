using MWS.Movilizer;
using System;
using System.Collections.Generic;
using System.Text;

namespace MWS.Templates
{
    public class DocumentPoolUpdateTemplate
    {
        public MovilizerDocumentPoolUpdate _documentPoolUpdate;

        List<DocumentUpdateTemplate> _documentUpdates;
        List<DocumentDeleteTemplate> _documentDeletes;

        public DocumentPoolUpdateTemplate(string pool)
        {
            _documentPoolUpdate = new MovilizerDocumentPoolUpdate();
            _documentPoolUpdate.pool = pool;

            _documentUpdates = new List<DocumentUpdateTemplate>();
            _documentDeletes = new List<DocumentDeleteTemplate>();
        }

        public void UpdateDocument(DocumentUpdateTemplate duTemplate)
        {
            _documentUpdates.Add(duTemplate);
        }

        public void DeleteDocument(DocumentDeleteTemplate ddTemplate)
        {
            _documentDeletes.Add(ddTemplate);
        }

        public MovilizerDocumentPoolUpdate ToDocumentPoolUpdate()
        {
            int count = 0;
            _documentPoolUpdate.update = new MovilizerDocumentUpdate[_documentUpdates.Count];

            foreach (DocumentUpdateTemplate duTemplate in _documentUpdates)
            {
                _documentPoolUpdate.update[count++] = duTemplate.ToDocumentUpdate();
            }

            count = 0;
            _documentPoolUpdate.delete = new MovilizerDocumentDelete[_documentDeletes.Count];

            foreach (DocumentDeleteTemplate ddTemplate in _documentDeletes)
            {
                _documentPoolUpdate.delete[count++] = ddTemplate.ToDocumentDelete();
            }

            return _documentPoolUpdate;
        }
    }
}
