using MWS.Movilizer;
using System.Collections.Generic;

namespace MWS.Templates
{
    public class AnswerTemplate
    {
        public MovilizerAnswer _answer;
        List<AnswerItemTemplate> _answerItems;

        public AnswerTemplate(MovilizerAnswer answer)
        {
            _answer = SerializeHelper.CloneObject(answer);

            // extract answer items
            _answerItems = new List<AnswerItemTemplate>();
            MovilizerAnswerItem[] answerItems = _answer.item;
            if (answerItems != null)
            {
                foreach (MovilizerAnswerItem answerItem in answerItems)
                {
                    _answerItems.Add(new AnswerItemTemplate(answerItem));
                }
            }
        }

        public AnswerItemTemplate GetAnswerItem(string clientKey)
        {
            foreach (AnswerItemTemplate aiTemplate in _answerItems)
            {
                if (aiTemplate._answerItem.clientKey == clientKey)
                {
                    return aiTemplate;
                }
            }
            return null;
        }

        public void AddAnswerItem(AnswerItemTemplate ai)
        {
            _answerItems.Add(ai);
        }

        public MovilizerAnswer ToAnswer()
        {
            int aiCount = 0;
            _answer.item = new MovilizerAnswerItem[_answerItems.Count];

            foreach (AnswerItemTemplate aiTemplate in _answerItems)
            {
                _answer.item[aiCount++] = aiTemplate.ToAnswerItem();
            }
            return _answer;
        }
    }
}
