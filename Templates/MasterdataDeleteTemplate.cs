using MWS.Movilizer;
namespace MWS.Templates
{
    public class MasterdataDeleteTemplate
    {
        public MovilizerMasterdataDelete _masterdataDelete;

        public MasterdataDeleteTemplate(MovilizerMasterdataDelete masterdataDelete)
        {
            _masterdataDelete = SerializeHelper.CloneObject(masterdataDelete);
        }

        public MovilizerMasterdataDelete ToMasterdataDelete()
        {
            return _masterdataDelete;
        }
    }
}
