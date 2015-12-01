
using MWS.Movilizer;
namespace MWS.Templates
{
    public class MasterdataEntryTemplate
    {
        public MovilizerGenericDataContainerEntry _masterdataEntry;

        public MasterdataEntryTemplate(MovilizerGenericDataContainerEntry masterdataEntry)
        {
            _masterdataEntry = SerializeHelper.CloneObject(masterdataEntry);
        }

        public MovilizerGenericDataContainerEntry ToMasterdataEntry()
        {
            return _masterdataEntry;
        }
    }
}
