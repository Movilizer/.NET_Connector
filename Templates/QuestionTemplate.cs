using System.Collections.Generic;

namespace MWS.Templates
{
    public class QuestionTemplate
    {
        public MovilizerQuestion _question;
        List<AnswerTemplate> _answers;
        //Dictionary<string, AnswerTemplate> _restrictions;
        //Dictionary<string, AnswerTemplate> _validations;

        public QuestionTemplate(MovilizerQuestion question)
        {
            _question = SerializeHelper.CloneObject(question);

            // extract answers
            _answers = new List<AnswerTemplate>();
            MovilizerAnswer[] answers = _question.answer;
            if (answers != null)
            {
                foreach (MovilizerAnswer answer in answers)
                {
                    _answers.Add(new AnswerTemplate(answer));
                }
            }
        }

        public AnswerTemplate GetAnswer(string aKey)
        {
            foreach (AnswerTemplate aTemplate in _answers)
            {
                if (aTemplate._answer.key == aKey)
                {
                    return aTemplate;
                }
            }
            return null;
        }

        public void SetAnswerText(string aKey, string text)
        {
            GetAnswer(aKey)._answer.text = text;
        }

        public void SetAnswerPredefinedValue(string aKey, string value)
        {
            GetAnswer(aKey)._answer.predefinedValue = value;
        }

        public void AddAnswer(AnswerTemplate a)
        {
            _answers.Add(a);
        }

        public MovilizerQuestion ToQuestion()
        {
            int aCount = 0;
            _question.answer = new MovilizerAnswer[_answers.Count];

            foreach (AnswerTemplate aTemplate in _answers)
            {
                _question.answer[aCount++] = aTemplate.ToAnswer();
            }
            return _question;
        }
    }
}
