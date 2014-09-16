
namespace MWS.Templates
{
    public class ReplyQuestionTemplate
    {
        public MovilizerReplyQuestion _replyQuestion;

        public ReplyQuestionTemplate(MovilizerReplyQuestion replyQuestion)
        {
            _replyQuestion = replyQuestion;
        }

        public MovilizerReplyParameterValue GetParameterValue(string pKey)
        {
            foreach (MovilizerReplyParameterValue parameterValue in _replyQuestion.parameterValue)
            {
                if (parameterValue.parameterKey.Equals(pKey))
                {
                    return parameterValue;
                }
            }
            return null;
        }

        public bool ContainsParameterValue(MovilizerReplyParameterValue pValue)
        {
            foreach (MovilizerReplyParameterValue parameterValue in _replyQuestion.parameterValue)
            {
                if (parameterValue.parameterKey.Equals(pValue.parameterKey) &&
                    parameterValue.valueFreeText.Equals(pValue.valueFreeText))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsParameterValues(MovilizerReplyParameterValue[] pValues)
        {
            int counter = 0;
            foreach (MovilizerReplyParameterValue parameterValue in _replyQuestion.parameterValue)
            {
                foreach (MovilizerReplyParameterValue pValue in pValues)
                {
                    if (parameterValue.parameterKey.Equals(pValue.parameterKey) &&
                        parameterValue.valueFreeText.Equals(pValue.valueFreeText))
                    {
                        counter++;
                    }
                }
            }
            return counter == pValues.Length;
        }
    }
}
