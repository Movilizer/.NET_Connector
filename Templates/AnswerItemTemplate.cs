
namespace MWS.Templates
{
    public class AnswerItemTemplate
    {
        public MovilizerAnswerItem _answerItem;

        public AnswerItemTemplate(MovilizerAnswerItem answerItem)
        {
            _answerItem = SerializeHelper.CloneObject(answerItem);
        }

        public MovilizerAnswerItem ToAnswerItem()
        {
            return _answerItem;
        }
    }
}
