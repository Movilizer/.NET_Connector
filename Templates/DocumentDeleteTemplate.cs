namespace MWS.Templates
{
    public class DocumentDeleteTemplate
    {
        public MovilizerDocumentDelete _documentDelete;

        public DocumentDeleteTemplate(MovilizerDocumentDelete documentDelete)
        {
            _documentDelete = SerializeHelper.CloneObject(documentDelete);
        }

        public MovilizerDocumentDelete ToDocumentDelete()
        {
            return _documentDelete;
        }
    }
}
