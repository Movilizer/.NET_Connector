using System.Collections.Generic;


namespace MWS.Templates
{
    public class DocumentUpdateTemplate
    {
        public MovilizerDocumentUpdate _documentUpdate;

        public DocumentUpdateTemplate(MovilizerDocumentUpdate documentUpdate)
        {
            _documentUpdate = SerializeHelper.CloneObject(documentUpdate);
        }

        public MovilizerDocumentUpdate ToDocumentUpdate()
        {
            return _documentUpdate;
        }
    }
}
