using System.Collections.Generic;

using MWS.Helper;
using MWS.Movilizer;

namespace MWS.Templates
{
    public class ReplyMoveletTemplate
    {
        static MovilizerReplyAnswer[] EMPTY_ANSWERS = new MovilizerReplyAnswer[0];

        public MovilizerReplyMovelet _replyMovelet;
        List<ReplyQuestionTemplate> _replyQuestions;
        List<ReplyQuestionTemplate> _filteredReplyQuestions;

        public ReplyMoveletTemplate(MovilizerReplyMovelet replyMovelet)
        {
            _replyMovelet = replyMovelet;
            _replyQuestions = new List<ReplyQuestionTemplate>();
            foreach (MovilizerReplyQuestion replyQuestion in replyMovelet.replyQuestion)
            {
                _replyQuestions.Add(new ReplyQuestionTemplate(replyQuestion));
            }
            _filteredReplyQuestions = null;
        }

        public MovilizerReplyQuestion GetReplyQuestion(string qKey)
        {
            List<ReplyQuestionTemplate> replyQuestions = 
                _filteredReplyQuestions != null ? _filteredReplyQuestions : _replyQuestions;

            foreach (ReplyQuestionTemplate replyQuestion in replyQuestions)
            {
                if (replyQuestion._replyQuestion.questionKey.Equals(qKey))
                {
                    return replyQuestion._replyQuestion;
                }
            }
            return null;
        }

        public MovilizerReplyAnswer[] GetReplyAnswers(string qKey)
        {
            MovilizerReplyQuestion replyQuestion = GetReplyQuestion(qKey);

            if (replyQuestion != null && replyQuestion.replyAnswer != null)
            {
                return replyQuestion.replyAnswer;
            }
            else
            {
                return EMPTY_ANSWERS;
            }
        }

        public MovilizerReplyAnswer GetReplyAnswer(string qKey, string aKey)
        {
            MovilizerReplyQuestion replyQuestion = GetReplyQuestion(qKey);

            if (replyQuestion != null && replyQuestion.replyAnswer != null)
            {
                foreach (MovilizerReplyAnswer replyAnswer in replyQuestion.replyAnswer)
                {
                    if (replyAnswer.answerKey.Equals(aKey))
                    {
                        return replyAnswer;
                    }
                }
            }
            return null;
        }

        public void ClearReplyQuestionFilter()
        {
            _filteredReplyQuestions = null;
        }

        public MovilizerReplyParameterValue[] GetParameterValues(string qKey, string pKey)
        {
            List<MovilizerReplyParameterValue> parameterValues = new List<MovilizerReplyParameterValue>();
            List<ReplyQuestionTemplate> replyQuestions =
                _filteredReplyQuestions != null ? _filteredReplyQuestions : _replyQuestions;

            foreach (ReplyQuestionTemplate replyQuestion in replyQuestions)
            {
                if (replyQuestion._replyQuestion.questionKey.Equals(qKey))
                {
                    foreach (MovilizerReplyParameterValue parameterValue in replyQuestion._replyQuestion.parameterValue)
                    {
                        if (parameterValue.parameterKey.Equals(pKey))
                        {
                            parameterValues.Add(parameterValue);
                        }
                    }
                }
            }
            return parameterValues.ToArray();
        }

        public void FilterReplyQuestionsByParameterValue(MovilizerReplyParameterValue parameterValue)
        {
            FilterReplyQuestionsByParameterValues(new MovilizerReplyParameterValue[] { parameterValue });
        }

        public void FilterReplyQuestionsByParameterValues(MovilizerReplyParameterValue[] parameterValues)
        {
            List<ReplyQuestionTemplate> filteredReplyQuestions = new List<ReplyQuestionTemplate>();

            foreach (ReplyQuestionTemplate replyQuestion in _replyQuestions)
            {
                if (replyQuestion.ContainsParameterValues(parameterValues))
                {
                    filteredReplyQuestions.Add(replyQuestion);
                }
            }

            _filteredReplyQuestions = filteredReplyQuestions;
        }

        public void SerializeToFile(string path)
        {
            XmlHelper.SerializeToFile(path + "Reply_" + _replyMovelet.moveletKey + ".xml", _replyMovelet);
        }
    }
}
