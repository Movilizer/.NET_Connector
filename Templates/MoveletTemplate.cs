using System.Collections.Generic;

using MWS.Helper;
using MWS.Movilizer;

namespace MWS.Templates
{
    public class MoveletTemplate
    {
        public MovilizerMovelet _movelet;
        public List<QuestionTemplate> _questions;

        public MoveletTemplate(MovilizerMovelet movelet)
        {
            _movelet = SerializeHelper.CloneObject(movelet);

            // extract questions
            _questions = new List<QuestionTemplate>();
            MovilizerQuestion[] questions = _movelet.question;
            if (questions != null)
            {
                foreach (MovilizerQuestion question in questions)
                {
                    _questions.Add(new QuestionTemplate(question));
                }
            }
        }

        public QuestionTemplate GetQuestion(string qKey)
        {
            foreach (QuestionTemplate qTemplate in _questions)            
            {
                if (qTemplate._question.key == qKey)
                {
                    return qTemplate;
                }
            }
            return null;
        }

        public void AddQuestion(QuestionTemplate q)
        {
            _questions.Add(q);
        }

        public MovilizerMovelet ToMovelet()
        {
            int qCount = 0;
            _movelet.question = new MovilizerQuestion[_questions.Count];

            foreach(QuestionTemplate qTemplate in _questions)
            {
                _movelet.question[qCount++] = qTemplate.ToQuestion();
            }
            return _movelet;
        }

        public void SerializeToFile(string path)
        {
            XmlHelper.SerializeToFile(path + _movelet.moveletKey + ".xml", ToMovelet());
        }
    }
}
